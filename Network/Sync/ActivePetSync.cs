using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terramon.Players;
using Terraria;
using Terraria.ID;

namespace Terramon.Network.Sync
{
    public class ActivePetSync : Packet
    {
        public override string PacketName => "net_actPetSync";

        public void Send(TerramonMod mod, TerramonPlayer pl)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            var p = GetPacket(mod);
            p.Write(pl.ActivePartySlot);
            p.Send();
        }

        public void Resend(int whoAmI, int id)
        {
            var p = GetPacket(TerramonMod.Instance);
            p.Write(whoAmI);
            p.Write(id);
            p.Send(ignoreClient: whoAmI);
        }

        public override void HandleFromClient(BinaryReader reader, int whoAmI)
        {
            var slot = reader.ReadInt32();
            var pl = Main.player[whoAmI].GetModPlayer<TerramonPlayer>();
            pl.ActivePartySlot = slot;
            if (pl.Battle != null)
            {
                pl.Battle.awaitSync = false;
                pl.Battle.HandleChange();
            }
            Resend(whoAmI, slot);
        }

        public override void HandleFromServer(BinaryReader reader)
        {
            var whoAmI = reader.ReadInt32();
            var slot = reader.ReadInt32();
            var pl = Main.player[whoAmI].GetModPlayer<TerramonPlayer>();
            pl.ActivePartySlot = slot;
            if (pl.Battle != null)
            {
                pl.Battle.awaitSync = false;
                pl.Battle.HandleChange();
            }
        }
    }
}
