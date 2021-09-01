using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terramon.Pokemon;
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
                var p = GetPacket();
                p.Write(whoAmI);
                p.Write(wid);
                p.Write(proj.whoAmI);
                p.Write(new PokemonData()
                {
                    pokemon = poke.Name,
                    Level = 5,
                });
            }

        }

        public override void HandleFromServer(BinaryReader reader)
        {
            
        }
    }
}
