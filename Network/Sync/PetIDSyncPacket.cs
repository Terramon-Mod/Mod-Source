using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terramon.Players;
using Terraria;

namespace Terramon.Network.Sync
{
    public class PetIDSyncPacket : Packet
    {
        public override string PacketName => "net_petidsync";

        public void Send(TerramonMod mod, int id)
        {
            var p = GetPacket(mod);
            p.Write(id);
            p.Send();
        }

        public void Resend(int id, int whoAmI)
        {
            var p = GetPacket(TerramonMod.Instance);
            p.Write(whoAmI);
            p.Write(id);
            p.Send(ignoreClient: whoAmI);
        }

        public override void HandleFromClient(BinaryReader reader, int whoAmI)
        {
            var id = reader.ReadInt32();
            var pl = Main.player[whoAmI].GetModPlayer<TerramonPlayer>();
            pl.ActivePetId = id;
            Resend(id, whoAmI);
        }

        public override void HandleFromServer(BinaryReader reader)
        {
            var whoAmI = reader.ReadInt32();
            var id = reader.ReadInt32();
            var pl = Main.player[whoAmI].GetModPlayer<TerramonPlayer>();
            pl.ActivePetId = id;
        }
    }
}
