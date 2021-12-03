using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terramon.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace Terramon.Network.Sync
{
    public class PlayerSidebarSync : Packet
    {
        public override string PacketName => "net_plSBSync";

        public void Send(TerramonMod mod, TerramonPlayer pl, int target = -1)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            var p = GetPacket(mod);
            p.Write(pl.Save());
            p.Send();
        }

        protected void Resend(TagCompound tag, int whoAmI)
        {
            var p = GetPacket(TerramonMod.Instance);
            p.Write(whoAmI);
            p.Write(tag);
            p.Send(ignoreClient: whoAmI);
        }


        public override void HandleFromClient(BinaryReader reader, int whoAmI)
        {
            var tag = reader.ReadTag();
            Main.player[whoAmI].GetModPlayer<TerramonPlayer>().Load(tag);
            Resend(tag, whoAmI);
        }

        public override void HandleFromServer(BinaryReader reader)
        {
            var whoAmI = reader.ReadInt32();
            var tag = reader.ReadTag();
            Main.player[whoAmI].GetModPlayer<TerramonPlayer>().Load(tag);
        }
    }
}
