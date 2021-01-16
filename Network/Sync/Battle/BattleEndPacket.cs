using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terramon.Players;
using Terramon.Pokemon;
using Terraria;

namespace Terramon.Network.Sync.Battle
{
    public class BattleEndPacket : Packet
    {
        public override string PacketName => "bat_end";

        public void Send(TerramonMod mod)
        {
            GetPacket(mod).Send();
        }

        public void Resend(int whoAmI)
        {
            var p = GetPacket(TerramonMod.Instance);
            p.Write(whoAmI);
            p.Send(ignoreClient: whoAmI);
        }

        public override void HandleFromClient(BinaryReader reader, int whoAmI)
        {
            var pl = Main.player[whoAmI].GetModPlayer<TerramonPlayer>();
            if (pl.Battle != null)
            {
                pl.Battle.State = BattleState.None;
                if (pl.Battle.player2 != null && pl.Battle.player2.Battle != null)
                {
                    pl.Battle.player2.Battle.State = BattleState.None;
                }
                Resend(whoAmI);
            }
        }

        public override void HandleFromServer(BinaryReader reader)
        {
            var whoAmI = reader.ReadInt32();
            var pl = Main.player[whoAmI].GetModPlayer<TerramonPlayer>();
            if (pl.Battle != null)
            {
                pl.Battle.State = BattleState.None;
                if (pl.Battle.player2?.Battle != null)
                {
                    pl.Battle.player2.Battle.State = BattleState.None;
                }
            }
        }
    }
}
