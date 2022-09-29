using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terramon.Pokemon.Moves.v2
{
    public class SwitchMove : BaseMove
    {
        public override string MoveName => "Switch Cast";

        public override string MoveDescription => "Allow switching one pokemon to other in your squad." +
                                                  "Cannot be assigned to pokemon";

        public override Target Target => Target.Party;

        public override bool PerformInBattle(BattleOpponent atacker, BattleOpponent deffender, BattleModeV2 battle)
        {
            var op = atacker as BattleTrainerOpponent;
            return op.CanSwitch;
        }

        public override bool AnimateTurn(BattleOpponent atacker, BattleOpponent deffender, BattleModeV2 battle, int frame,
            bool skipBtnPressed = false)
        {
            var op = atacker as BattleTrainerOpponent;
            //TODO: make switch animation


            return frame != 30;
        }
    }
}
