using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Razorwing.RPC;
using Terramon.Pokemon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Terramon
{
    public class TerramonWorld : ModWorld
    {
        //Default initialize requred here bc Load called only when world already loaded before
        public Dictionary<string, BattleModeV2> Battles = new Dictionary<string, BattleModeV2>();


        public override void Load(TagCompound tag)
        {
            base.Load(tag);
            Battles = new Dictionary<string, BattleModeV2>();
        }


        public override void PreUpdate()
        {

            List<string> endedBattles = null;
            foreach (var battle in Battles)
            {
                battle.Value.Update();
                //Evade changing collection in loop
                if (battle.Value.State == BattleModeV2.BattleState.None)
                {
                    if (endedBattles == null)
                        endedBattles = new List<string>();

                    endedBattles.Add(battle.Key);
                }
            }

            endedBattles?.ForEach(x =>
            {
                //if someone joined world and get battle
                //in invalid state - this prevent further desync
                if(Main.netMode == NetmodeID.Server)
                    this.RPC(EnshureEndBattle, x, ExecutingSide.Client);
                Battles[x].Cleanup();
                Battles.Remove(x);
            });
        }

        public void StartBattle(BattleOpponent o1, BattleOpponent o2, string id, byte state = (byte)BattleModeV2.BattleState.Intro)
        {
            //var id = Guid.NewGuid().ToString().Take(10).ToString();
            var val = new BattleModeV2(o1, o2)
            {
                BattleID = id,
            };

            Battles.Add(id, val);
            if (state != (byte)BattleModeV2.BattleState.Intro)
                val.State = (BattleModeV2.BattleState)state;

            if (o1 is BattlePlayerOpponent p1)
            {
                p1.Player.Battlev2 = val;
            }
            if (o2 is BattlePlayerOpponent p2)
            {
                p2.Player.Battlev2 = val;
            }
        }

        public string StartNewBattle(BattleOpponent o1, BattleOpponent o2)
        {
            var guid = Guid.NewGuid().ToString();
            var id = guid.Substring(0, 10);
            //guid = guid.Substring(10);
            var val = new BattleModeV2(o1, o2);
            val.BattleID = id;

            Battles.Add(id, val);

            o1.Battle = o2.Battle = val;
            o1.ID = guid.Substring(10, 10);
            //guid = guid.Substring(10);
            o2.ID = guid.Substring(20, 10);

            if (o1 is BattlePlayerOpponent p1)
            {
                p1.Player.Battlev2 = val;
            }
            if (o2 is BattlePlayerOpponent p2)
            {
                p2.Player.Battlev2 = val;
            }

            //Call StartBattle on all clients
            if(Main.netMode == NetmodeID.Server)
                this.RPC(StartBattle, o1, o2, id, (byte)BattleModeV2.BattleState.Intro, ExecutingSide.Client | ExecutingSide.DenySender);

            return id;
        }

        public void EnshureEndBattle(string id)
        {
            if (Battles.ContainsKey(id))
            {
                Battles[id].Cleanup();
                Battles.Remove(id);
            }
        }
    }
}
