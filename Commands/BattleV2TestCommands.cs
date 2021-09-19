using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terramon.Extensions;
using Terramon.Extensions.ValidationExtensions;
using Terramon.Pokemon;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;

#if DEBUG

namespace Terramon.Commands
{
    public class BattleV2StartCommand : ModCommand
    {
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
            {
                caller.Reply(Usage, Color.OrangeRed);
                return;
            }

            switch (args[0].ToLower())
            {
                case "startnearest":
                case "sn":
                {
                    var npc = GetNearestWild(caller.Player.position);
                    if ((caller.Player.position - npc.position).Length() < 200)
                    {
                        var pl = caller.Player.TPlayer();
                        if (pl.Validate().NotInBattle().HasActivePetProjectile()
                            .HasNotFaintedPokemons().Result())
                        {
                            TerramonMod.Instance.
                                Logger.Error("Player in invalid state send battle start packet. Ignoring...");
                            return;
                        }

                        if (npc.modNPC is ParentPokemonNPC pnpc)
                        {
                            ParentPokemon.det_Wild = true;
                            npc.active = false;
                            var proj = Projectile.NewProjectileDirect(npc.position, Vector2.Zero, TerramonMod.Instance.ProjectileType(pnpc.HomeClass().Name), 0, 0, pl.whoAmI);
                            var poke = (ParentPokemon)proj.modProjectile;

                            //TODO: Fetch DB for lvl and stats
                            //BaseMove.

                            //var pl = Main.player[whoAmI].TPlayer();

                            

                            var data = new PokemonData()
                            {
                                pokemon = poke.Name,
                                Level = 5,
                            };
                            data.HP = data.MaxHP;

                            var plo = new BattlePlayerOpponent(pl);
                            var wild = new BattleWildOpponent(proj.Pokemon(), data);

                            pl.Battlev2 = new BattleModeV2(plo, wild);
                        }
                    }

                    break;
                }

                case "windvwild":
                case "wvw":

                    var npc1 = GetNearestWild(caller.Player.position).modNPC as ParentPokemonNPC;
                    var npc2 = GetNearestWild(caller.Player.position, new []{npc1.npc.whoAmI}).modNPC as ParentPokemonNPC;

                    if ((npc1.npc.position - npc2.npc.position).Length() < 400)
                    {
                        var pl = caller.Player.TPlayer();

                        ParentPokemon.det_Wild = true;
                        npc1.npc.active = false;

                        var proj1 = Projectile.NewProjectileDirect(npc1.npc.position, Vector2.Zero,
                            TerramonMod.Instance.ProjectileType(npc1.HomeClass().Name), 0, 0, pl.whoAmI);
                        var poke1 = (ParentPokemon) proj1.modProjectile;

                        ParentPokemon.det_Wild = true;
                        npc2.npc.active = false;

                        var proj2 = Projectile.NewProjectileDirect(npc2.npc.position, Vector2.Zero,
                            TerramonMod.Instance.ProjectileType(npc2.HomeClass().Name), 0, 0, pl.whoAmI);
                        var poke2 = (ParentPokemon)proj1.modProjectile;

                        //TODO: Fetch DB for lvl and stats
                        //BaseMove.

                        //var pl = Main.player[whoAmI].TPlayer();

                        if (!pl.Validate().NotInBattle().Result())
                        {
                            TerramonMod.Instance.Logger.Error(
                                "Player in invalid state send battle start packet. Ignoring...");
                            return;
                        }

                        var data = new PokemonData()
                        {
                            pokemon = poke1.Name,
                            Level = 5,
                        };
                        data.HP = data.MaxHP;


                        var wild1 = new BattleWildOpponent(proj1.Pokemon(), data);
                        data = new PokemonData()
                        {
                            pokemon = poke2.Name,
                            Level = 5,
                        };
                        data.HP = data.MaxHP;

                        var wild2 = new BattleWildOpponent(proj2.Pokemon(), data);

                        pl.Battlev2 = new BattleModeV2(wild1, wild2);

                    }

                    break;
            }
        }

        public override string Usage => "bv2 <command> {startnearest (sn), windvwild{wvw}}";

        public override string Command => "bv2";
        public override CommandType Type => CommandType.World;

        public static NPC GetNearestWild(Vector2 pos, int[] excluding = null)
        {
            int closest = -1;
            float lenght = float.MaxValue, buf;
            for (int i = 0; i < Main.maxNPCs; i++)
                if (!excluding?.Contains(i) ?? true && Main.npc[i].active && (Main.npc[i].modNPC is ParentPokemonNPC))
                {
                    buf = (pos - Main.npc[i].position).LengthSquared();
                    if (buf < lenght)
                    {
                        closest = i;
                        lenght = buf;
                    }
                }

            if (closest == -1 || (pos - Main.npc[closest].position).LengthSquared() > 60000)//400^2
                return null;

            return Main.npc[closest];
        }
    }
}
#endif