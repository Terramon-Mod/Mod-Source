using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Terramon.Players;
using Terramon.Pokemon;
using Terraria;

namespace Terramon.Network.Sync.Battle
{
    public class BaseBattleSyncPacket : Packet
    {
        public override string PacketName => "bat_basesync";

        public void Send(TerramonPlayer pl)
        {
            var p = GetPacket(TerramonMod.Instance);
            var data = pl.Battle?.GetSyncData();
            if (data.HasValue && data.Value.state != BattleState.None)
            {
                switch (data.Value.state)
                {
                    case BattleState.BattleWithPlayer:
                        p.Write((int)data.Value.state);
                        p.Write(data.Value.pl1);
                        p.Write(data.Value.pl2);
                        p.Send();
                        break;
                    case BattleState.BattleWithWild:
                        p.Write((int)data.Value.state);
                        p.Write(data.Value.pl1);
                        p.Write(data.Value.wildID);
                        p.Write(data.Value.wild);
                        break;
                }
            }
            
        }

        public override void HandleFromClient(BinaryReader reader, int whoAmI)
        {
            var state = (BattleState)reader.ReadInt32();
            var p = GetPacket(TerramonMod.Instance);
            p.Write((int)state);
            switch (state)
            {
                case BattleState.BattleWithPlayer:
                    p.Write(reader.ReadInt32());
                    p.Write(reader.ReadInt32());
                    p.Send(ignoreClient: whoAmI);
                    break;
                case BattleState.BattleWithWild:
                    p.Write(reader.ReadInt32());
                    p.Write(reader.ReadInt32());
                    p.Write(reader.ReadTag());
                    p.Send(ignoreClient: whoAmI);
                    break;
            }
        }

        public override void HandleFromServer(BinaryReader reader)
        {
            var state = (BattleState)reader.ReadInt32();
            var id = reader.ReadInt32();
            var pl = Main.player[id].GetModPlayer<TerramonPlayer>();
            switch (state)
            {
                case BattleState.BattleWithPlayer:
                    var pl2 = Main.player[reader.ReadInt32()].GetModPlayer<TerramonPlayer>();
                    if (pl.Battle == null)
                    {
                        pl.Battle = new BattleMode(pl, BattleState.BattleWithPlayer, spl: pl2, lazy: true);
                    }
                    else
                    {
                        pl.Battle.player2 = pl2;
                    }
                    break;
                case BattleState.BattleWithWild:
                    var prid = reader.ReadInt32();
                    var wild = new PokemonData(reader.ReadTag());
                    var wildNPC = (ParentPokemon)Main.projectile[prid].modProjectile;
                    if (pl.Battle == null)
                    {
                        pl.Battle = new BattleMode(pl, BattleState.BattleWithWild, wild, lazy: true)
                        {
                            wildID = prid,
                            WildNPC = wildNPC
                        };
                    }
                    else
                    {
                        pl.Battle.Wild = wild;
                        pl.Battle.wildID = prid;
                        pl.Battle.WildNPC = wildNPC;
                    }
                    break;
            }
        }
    }
}
