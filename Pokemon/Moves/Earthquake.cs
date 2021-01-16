using Microsoft.Xna.Framework;
using Razorwing.Framework.Graphics;
using Razorwing.Framework.Localisation;
using Razorwing.Framework.Utils;
using System;
using Terramon.Players;
using Terramon.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Pokemon.Moves
{
    public class Earthquake : DamageMove
    {
        public override string MoveName => "Earthquake";
        public override string MoveDescription => "The user sets off an earthquake that strikes every Pokémon around it.";
        public override int Damage => 100;
        public override int Accuracy => 100;
        public override int MaxPP => 10;
        public override int MaxBoostPP => 16;
        public virtual bool MakesContact => false;
        public override bool Special => false;
        public override Target Target => Target.Opponent;
        public override int Cooldown => 60 * 1; //Once per second
        public override PokemonType MoveType => PokemonType.Ground;

        public override int AutoUseWeight(ParentPokemon mon, Vector2 pos, TerramonPlayer player)
        {
            NPC target = GetNearestNPC(pos);
            if (target == null)
                return 0;
            return 30;
        }

        public override bool PerformInWorld(ParentPokemon mon, Vector2 pos, TerramonPlayer player)
        {
            NPC target = GetNearestNPC(pos);
            if (target == null)
                return false;

            player.Attacking = true;
            Vector2 vel = (target.position + (target.Size/2)) - (mon.projectile.position + (mon.projectile.Size/2));
            var l = vel.Length();
            vel += target.velocity * (l / 100);//Make predict shoot
            vel.Normalize(); //Direction
            vel *= 15; //Speed
            Projectile.NewProjectile((mon.projectile.position + (mon.projectile.Size / 2)), vel, ProjectileID.DD2PhoenixBowShot, 20, 1f, player.whoAmI);
            return true;
        }

        private int endMoveTimer;
        private int shakeTimer;
        private string s;
        private bool focusCamTarget = false;
        private bool inflictedDmg = false;
        public override bool AnimateTurn(ParentPokemon mon, ParentPokemon target, TerramonPlayer player, PokemonData attacker,
            PokemonData deffender, BattleState state, bool opponent)
        {
            if (AnimationFrame == 1) //At initial frame we pan camera to attacker
            {
                TerramonMod.ZoomAnimator.ScreenPosX(mon.projectile.position.X + 12, 500, Easing.OutExpo);
                TerramonMod.ZoomAnimator.ScreenPosY(mon.projectile.position.Y, 500, Easing.OutExpo);
            }
            else if (AnimationFrame == 140) //Move animation begin after 140 frames
            {
                BattleMode.UI.splashText.SetText("");

                MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));

                TerramonMod.ZoomAnimator.ScreenPosX(target.projectile.position.X + 12, 500, Easing.OutExpo);
                TerramonMod.ZoomAnimator.ScreenPosY(target.projectile.position.Y, 500, Easing.OutExpo);
            }

            if (AnimationFrame > 155 && AnimationFrame < 260)
            {
                shakeTimer++;
                if (shakeTimer <= 4)
                {
                    TerramonMod.ZoomAnimator.ScreenPosX(target.projectile.position.X + 12, 1, Easing.None);
                    TerramonMod.ZoomAnimator.ScreenPosY(target.projectile.position.Y + 3, 1, Easing.None);
                } else if (shakeTimer > 4)
                {
                    TerramonMod.ZoomAnimator.ScreenPosX(target.projectile.position.X + 12, 1, Easing.None);
                    TerramonMod.ZoomAnimator.ScreenPosY(target.projectile.position.Y - 3, 1, Easing.None);
                }
                if (shakeTimer >= 8)
                {
                    shakeTimer = 0;
                }

                if (!inflictedDmg)
                {
                    Dust bubbleDust = Dust.NewDustDirect(target.projectile.position + new Vector2(0, 20), target.projectile.width, target.projectile.height, ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"), 0f, 0f, 0, new Color(189, 165, 100));
                }
            }

            if (AnimationFrame == 190)
            {
                focusCamTarget = true;
            }

            if (AnimationFrame == 240)
            {
                InflictDamage(mon, target, player, attacker, deffender, state, opponent);
                inflictedDmg = true;
                if (PostTextLoc.Args.Length >= 4)//If we can extract damage number
                    CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]);//Print combat text at attacked mon position
                BattleMode.queueEndMove = true;
            }

            // This should be at the very bottom of AnimateTurn() in every move.
            if (BattleMode.moveEnd)
            {
                AnimationFrame = 0;
                focusCamTarget = false;
                inflictedDmg = false;
                shakeTimer = 0;
                BattleMode.moveEnd = false;
                return false;
            }

            // IGNORE EVERYTHING BELOW WHEN MAKING YOUR OWN MOVES.
            if (AnimationFrame > 1810) return false;

            return true;
        }
    }
}