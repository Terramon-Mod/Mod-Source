using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terramon.Players;
using Terramon.Pokemon;
using Terraria;

namespace Terramon.Network.Sync.Battle
{
    /// <summary>
    /// This packet starts Battle for selected players on all clients
    /// </summary>
    public class StartBattlePacket : Packet
    {
        public override string PacketName => "bat_start";

        public void Send(TerramonMod mod, BattleState state, Player battleWith)
        {
            var p = GetPacket(mod);
            p.Write((int)state);
            p.Write(battleWith.whoAmI);
            p.Send();
        }

        public void Send(TerramonMod mod, BattleState state, PokemonData data, int projID)
        {
            var p = GetPacket(mod);
            p.Write((int)state);
            p.Write(data);
            p.Write(projID);
            p.Send();
        }

        protected void Resend(BattleState state, Player battleWith, int whoAmI)
        {
            var p = GetPacket(TerramonMod.Instance);
            p.Write(whoAmI);
            p.Write((int)state);
            p.Write(battleWith.whoAmI);
            p.Send(ignoreClient: whoAmI);
        }

        protected void Resend(BattleState state, PokemonData data, int projID, int whoAmI)
        {
            var p = GetPacket(TerramonMod.Instance);
            p.Write(whoAmI);
            p.Write((int)state);
            p.Write(data);
            p.Write(projID);
            p.Send(ignoreClient: whoAmI);
        }

        public override void HandleFromClient(BinaryReader reader, int whoAmI)
        {
            BattleState state = (BattleState)reader.ReadInt32();
            if (state == BattleState.BattleWithPlayer)
            {
                int battleWith = reader.ReadInt32();
                var pl1 = Main.player[whoAmI]?.GetModPlayer<TerramonPlayer>();
                var pl2 = Main.player[battleWith]?.GetModPlayer<TerramonPlayer>();
                if (pl1 != null)
                    pl1.Battle = new BattleMode(pl1, state, pl2?.ActivePet, null, pl2, true);
                else
                {
                    return;
                }
                if (pl2 != null)
                    pl2.Battle = new BattleMode(pl2, state, pl1?.ActivePet, null, pl1, true);
                Resend(state, pl2.player, whoAmI);
            }
            else if (state == BattleState.BattleWithWild)
            {
                var pl1 = Main.player[whoAmI]?.GetModPlayer<TerramonPlayer>();
                var tag = new PokemonData(reader.ReadTag());
                var projID = reader.ReadInt32();
                if (pl1 != null)
                {
                    pl1.Battle = new BattleMode(pl1, state, tag, null, null,
                        true);
                    pl1.Battle.wildID = projID;
                    pl1.Battle.WildNPC = (ParentPokemon)Main.projectile[projID].modProjectile;
                }
                Resend(state, tag, projID, whoAmI);
            }
            else if (state == BattleState.BattleWithTrainer)
            {
                Main.NewText("Battling with trainers not yet completed");
            }
        }

        public override void HandleFromServer(BinaryReader reader)
        {
            int whoAmI = reader.ReadInt32();
            BattleState state = (BattleState)reader.ReadInt32();
            if (state == BattleState.BattleWithPlayer)
            {
                int battleWith = reader.ReadInt32();
                var pl1 = Main.player[whoAmI]?.GetModPlayer<TerramonPlayer>();
                var pl2 = Main.player[battleWith]?.GetModPlayer<TerramonPlayer>();
                if (pl1 != null)
                    pl1.Battle = new BattleMode(pl1, state, pl2?.ActivePet, null, pl2,
                        true);
                else
                {
                    return;
                }
                if (pl2 != null)
                    pl2.Battle = new BattleMode(pl2, state, pl1?.ActivePet, null, pl1, Main.player[battleWith] != Main.LocalPlayer);
            }
            else if (state == BattleState.BattleWithWild)
            {
                var pl1 = Main.player[whoAmI]?.GetModPlayer<TerramonPlayer>();
                var tag = new PokemonData(reader.ReadTag());
                var projID = reader.ReadInt32();
                if (pl1 != null)
                {
                    pl1.Battle = new BattleMode(pl1, state, tag, null, null,
                        true);
                    pl1.Battle.wildID = projID;
                    pl1.Battle.WildNPC = (ParentPokemon)Main.projectile[projID].modProjectile;
                    pl1.Battle.awaitSync = true;
                }

            }
            else if (state == BattleState.BattleWithTrainer)
            {
                Main.NewText("Battling with trainers not yet completed");
            }
        }
    }
}
