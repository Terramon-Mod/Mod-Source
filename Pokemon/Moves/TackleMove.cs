using Microsoft.Xna.Framework;
using Terramon.Players;
using Terraria;
using Terraria.ID;

namespace Terramon.Pokemon.Moves
{
    public class TackleMove : BaseMove
    {
        public override string MoveName => "Tackle";
        public override string MoveDescription => "It's a move that physically damages the target.";
        public override Target Target => Target.Opponent;
        public override int Cooldown => 2 * 60;

        public bool moveDone = false;

        public override int AutoUseWeight(ParentPokemon mon, Vector2 pos, TerramonPlayer player)
        {
            NPC target = GetNearestNPC(pos);
            if (target == null)
                return 0;
            return 40;
        }

        public override bool Update(ParentPokemon mon, TerramonPlayer player)
        {
            return !moveDone;
        }

        public override bool OverrideAI(ParentPokemon mon, TerramonPlayer player)
        {
            if (!moveDone)
            {
                NPC target = GetNearestNPC(mon.projectile.position);
                if (target == null)
                    moveDone = true;
                mon.projectile.velocity = mon.projectile.position - target.position;        //gets a line and direction to the enemy
                mon.projectile.velocity.Normalize();      //sets it to something between 0 and 1
                mon.projectile.velocity *= 7f;        //multiplies that angle by 7
                if (mon.projectile.Distance(target.Center) <= 10f)
                {
                    target.StrikeNPC(32, 6f, mon.projectile.direction);
                    moveDone = true;
                }
            }
            return !moveDone;       //return the opposite of moveDone so that when moveDone goes true it goes back to normal AI
        }
    }
}