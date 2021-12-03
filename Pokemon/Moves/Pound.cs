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
    public class Pound : DamageMove
    {
        public override string MoveName => "Pound";
        public override string MoveDescription => "The target is physically pounded with a long tail, a foreleg, or the like.";
        public override int Damage => 40;
        public override int Accuracy => 100;
        public override int MaxPP => 35;
        public override int MaxBoostPP => 56;
        public override bool MakesContact => true;
        public override bool Special => false;
        public override Target Target => Target.Opponent;
        public override int Cooldown => 60 * 1;
        public override PokemonType MoveType => PokemonType.Normal;

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
            if (AnimationFrame == 1) //At initial frame we pan camera to attacker
            {
                TerramonMod.ZoomAnimator.ScreenPosX(mon.projectile.position.X + 12, 500, Easing.OutExpo);
                TerramonMod.ZoomAnimator.ScreenPosY(mon.projectile.position.Y, 500, Easing.OutExpo);
            }
            else if (AnimationFrame == 140)
            {
                BattleMode.UI.splashText.SetText("");

                TerramonMod.ZoomAnimator.ScreenPosX(target.projectile.position.X + 12, 500, Easing.OutExpo);
                TerramonMod.ZoomAnimator.ScreenPosY(target.projectile.position.Y, 500, Easing.OutExpo);

                MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
            }
            else if (AnimationFrame == 155)
            {
                for (float k = 0; k < MathHelper.TwoPi; k += 1.1f)
                {
                    var d = Dust.NewDustPerfect(target.projectile.Center, 90, Vector2.One.RotatedBy(k) * 2f);
                    d.noGravity = true;
                    var d2 = Dust.NewDustPerfect(target.projectile.Center, 90, Vector2.One.RotatedBy(k) * 2f);
                    d2.noGravity = true;
                    var d3 = Dust.NewDustPerfect(target.projectile.Center, 90, Vector2.One.RotatedBy(k) * 2f);
                    d3.noGravity = true;
                    var d4 = Dust.NewDustPerfect(target.projectile.Center, 90, Vector2.One.RotatedBy(k) * 2f);
                    d4.noGravity = true;
                    var d5 = Dust.NewDustPerfect(target.projectile.Center, 90, Vector2.One.RotatedBy(k) * 2f);
                    d5.noGravity = true;
                }
            }
            else if (AnimationFrame == 180)
            {
                InflictDamage(mon, target, player, attacker, deffender, state, opponent);
                if (PostTextLoc.Args.Length >= 4) //If we can extract damage number
                    CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]); //Print combat text at attacked mon position
                BattleMode.queueEndMove = true;
            }

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

