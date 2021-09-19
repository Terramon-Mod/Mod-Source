using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Repository.Hierarchy;
using Microsoft.Xna.Framework;
using Terramon.Extensions;
using Terramon.Extensions.ValidationExtensions;
using Terramon.Pokemon;
using Terramon.Pokemon.Moves;
using Terraria;

namespace Terramon.Network.BattlingV2
{
    public class StartBattleWithWildPacket : Packet
    {

        public override string PacketName => "batv2_sbww";

        public void Send(int plID, int wildID)
        {
            var p = GetPacket();
            p.Write(wildID);
            p.Send(256);
        }

        public override void HandleFromClient(BinaryReader reader, int whoAmI)
        {
            var wid = reader.ReadInt32();
            var npc = Main.npc[wid];

            if (npc.modNPC is ParentPokemonNPC pnpc)
            {
                ParentPokemon.det_Wild = true;
                npc.active = false;
                var proj = Projectile.NewProjectileDirect(npc.position, Vector2.Zero, TerramonMod.Instance.ProjectileType(pnpc.HomeClass().Name), 0,0, whoAmI);
                var poke = (ParentPokemon)proj.modProjectile;

                //TODO: Fetch DB for lvl and stats
                //BaseMove.

                var pl = Main.player[whoAmI].TPlayer();

                if (!pl.Validate().NotInBattle().HasActivePetProjectile()
                    .HasNotFaintedPokemons().Result())
                {
                    TerramonMod.Instance.
                        Logger.Error("Player in invalid state send battle start packet. Ignoring...");
                    return;
                }

                var data = new PokemonData()
                {
                    pokemon = poke.Name,
                    Level = 5,
                };

                var p = GetPacket();
                p.Write(whoAmI);
                p.Write(wid);
                p.Write(proj.whoAmI);
                p.Write(data);

                var plo = new BattlePlayerOpponent(pl);
                var wild = new BattleWildOpponent(proj.Pokemon(), data);

                pl.Battlev2 = new BattleModeV2(plo, wild);
            }

        }

        public override void HandleFromServer(BinaryReader reader)
        {
            var plid = reader.ReadInt32();
            var wildid = reader.ReadInt32();
            var projid = reader.ReadInt32();
            var data = new PokemonData(reader.ReadTag());

            var proj = Main.projectile[projid].Pokemon();
            if (Main.npc[wildid].IsWild())
            {
                Main.npc[wildid].active = false;
            }

            var pl = Main.player[plid].TPlayer();
            var plo = new BattlePlayerOpponent(pl);
            var wild = new BattleWildOpponent(proj,data);

            if (!pl.Validate().NotInBattle().HasActivePetProjectile()
                .HasNotFaintedPokemons().Result())
            {
                Main.NewText("Player in invalid state! Cant start clientside battle!");
                return;
            }

            pl.Battlev2 = new BattleModeV2(plo, wild);
        }
    }
}
