using Microsoft.Xna.Framework;
using Razorwing.Framework.Graphics;
using Razorwing.Framework.Utils;
using System;
using Terramon.Players;
using Terramon.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Pokemon.Moves
{
    public class Crabhammer : DamageMove
    {
        public override string MoveName => "Crabhammer";
        public override string MoveDescription => "The target is hammered with a large pincer. Critical hits land more easily.";
        public override int Damage => 100;
        public override int Accuracy => 90;
        public override int MaxPP => 10;
        public override int MaxBoostPP => 16;
        public override bool MakesContact => true;
        public override bool Special => false;
        public override Target Target => Target.Opponent;
        public override int Cooldown => 60 * 1;
        public override PokemonType MoveType => PokemonType.Water;

        public override int AutoUseWeight(ParentPokemon mon, Vector2 pos, TerramonPlayer player)
        {
            NPC target = GetNearestNPC(pos);
            if (target == null)
                return 0;
            return 30;
        }

        public override bool AnimateTurn(ParentPokemon mon, ParentPokemon target, TerramonPlayer player, PokemonData attacker,
            PokemonData deffender, BattleState state, bool opponent)
        {
            // This should be at the very bottom of AnimateTurn() in every move.
            if (BattleMode.moveEnd)
            {
                AnimationFrame = 0;
                BattleMode.moveEnd = false;
                return false;
            }

            // IGNORE EVERYTHING BELOW WHEN MAKING YOUR OWN MOVES.
            if (AnimationFrame > 1810) return false;

            return true;
        }

    }
}
