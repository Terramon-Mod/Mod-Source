using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Razorwing.RPC.Factory;
using Terramon.Players;
using Terramon.Pokemon;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Network.Providers
{

    public class OpponentProvider : IIdentityProvider
    {
        public Type[] workingTypes => new[]
        {
            typeof(BattleWildOpponent), typeof(BattleTrainerOpponent),
            typeof(BattlePlayerOpponent)
        };

        public Identity GetIdentity(object input)
        {
            switch (input)
            {
                case BattlePlayerOpponent pl:
                    if (pl.NeedCopy())
                        return new Identity(nameof(BattlePlayerOpponent), nameof(TerramonMod))
                        {
                            ["pl"] = pl.Player.player.whoAmI,
                            ["id"] = pl.ID,
                            ["new"] = true,
                        };
                    else
                        return new Identity(nameof(BattlePlayerOpponent), nameof(TerramonMod))
                        {
                            ["pl"] = pl.Player.player.whoAmI,
                            ["bid"] = pl.Battle.BattleID,
                            ["id"] = pl.ID,
                        };
                case BattleWildOpponent w:
                    if (w.NeedCopy())
                        return new Identity(nameof(BattleWildOpponent), nameof(TerramonMod))
                        {
                            ["id"] = w.ID,
                            ["pid"] = w.PokeProj.projectile.whoAmI,
                            ["dat"] = w.PokeData.GetCompound(),
                            ["new"] = true,
                        };
                    else
                        return new Identity(nameof(BattleWildOpponent), nameof(TerramonMod))
                        {
                            ["id"] = w.ID,
                            ["bid"] = w.Battle.BattleID,
                        };
            }

            throw new InvalidOperationException($"Can't handle {input.GetType().Name} in {nameof(OpponentProvider)}");
        }

        public object GetObject(Identity identity)
        {
            switch (identity.Type)
            {
                case nameof(BattlePlayerOpponent):
                {
                    if (identity.ContainsKey("new"))
                    {
                        var pl = Main.player[identity.GetInt("pl")].GetModPlayer<TerramonPlayer>();
                        return new BattlePlayerOpponent(pl, pl.player != Main.LocalPlayer)
                        {
                            ID = identity.GetString("id"),
                        };
                    }

                    var battle = ModContent.GetInstance<TerramonWorld>()
                        .Battles[identity.GetString("bid")];
                    var id = identity.GetString("id");
                    return battle.P1.ID == id ? battle.P1 :
                        battle.P2.ID == id ? battle.P2 : null;
                }

                case nameof(BattleWildOpponent):
                {
                    if (identity.ContainsKey("new"))
                    {
                        var proj = (ParentPokemon)Main.projectile[identity.GetInt("pid")].modProjectile;
                        var data = new PokemonData(identity.GetCompound("dat"));
                        return new BattleWildOpponent(proj, data)
                        {
                            ID = identity.GetString("id"),
                        };
                    }

                    var battle = ModContent.GetInstance<TerramonWorld>()
                        .Battles[identity.GetString("bid")];
                    var id = identity.GetString("id");
                    return battle.P1.ID == id ? battle.P1 :
                        battle.P2.ID == id ? battle.P2 : null;
                }
            }

            throw new InvalidOperationException($"Can't handle {identity.Type} in {nameof(OpponentProvider)}");
        }

    }
}
