using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using Terramon.Players;
using Terramon.Pokemon;
using Terraria;
using Terraria.ID;

namespace Terramon.Network.Sync.Battle
{
    public class WildNPCTransformPacket : Packet
    {
        public override string PacketName => "bat_npcTr";

        public void Send(TerramonMod mod, int npcID, int projID)
        {
            var p = GetPacket(mod);
            p.Write(npcID);
            p.Write(projID);
            p.Send();
        }

        public void ReSend(int playerID ,int pID, int projID)
        {
            var p = GetPacket(TerramonMod.Instance);
            p.Write(playerID);
            p.Write(pID);
            p.Write(projID);
            p.Send();
        }

        public override void HandleFromClient(BinaryReader reader, int whoAmI)
        {
            var id = reader.ReadInt32();
            var projID = reader.ReadInt32();
            if (Main.npc[id].active && Main.npc[id].modNPC is ParentPokemonNPC npc)
            {
                Main.npc[id].active = false;
                ReSend(whoAmI, id, projID);
            }
        }

        public override void HandleFromServer(BinaryReader reader)
        {
            var playerID = reader.ReadInt32();
            var id = reader.ReadInt32();
            var projID = reader.ReadInt32();
            if (Main.npc[id].active && Main.npc[id].modNPC is ParentPokemonNPC npc)
            {
                Main.npc[id].active = false;
            }

            var pl = Main.player[playerID].GetModPlayer<TerramonPlayer>();
            if (pl.Battle != null)
            {
                pl.Battle.wildID = projID;
                pl.Battle.WildNPC = (ParentPokemon)Main.projectile[projID]?.modProjectile;
                pl.Battle.awaitSync = false;
            }
        }
    }
}
