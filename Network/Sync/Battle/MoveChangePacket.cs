using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;

namespace Terramon.Network.Sync.Battle
{
    public class MoveChangePacket : Packet
    {
        public override string PacketName => "bat_move";


        public void Send(TerramonMod mod, BaseMove move)
        {
            var p = GetPacket(mod);
            p.Write(move.GetType().Name);
            p.Send();
        }

        protected void Resend(string move, int whoAmI)
        {
            var p = GetPacket(TerramonMod.Instance);
            p.Write(whoAmI);
            p.Write(move);
            p.Send(ignoreClient: whoAmI);
        }

        public override void HandleFromClient(BinaryReader reader, int whoAmI)
        {
            var pl = Main.player[whoAmI].GetModPlayer<TerramonPlayer>();
            if (pl.Battle != null)
            {
                var s = reader.ReadString();
                pl.Battle.SyncMove(s);
                pl.Battle.player2?.Battle?.SyncMove(s, false);
                Resend(s, whoAmI);
            }
        }

        public override void HandleFromServer(BinaryReader reader)
        {
            var whoAmI = reader.ReadInt32();
            var pl = Main.player[whoAmI].GetModPlayer<TerramonPlayer>();
            if (pl.Battle != null)
            {
                var s = reader.ReadString();
                pl.Battle.SyncMove(s);
                pl.Battle.player2?.Battle?.SyncMove(s, false);
            }
        }
    }
}
