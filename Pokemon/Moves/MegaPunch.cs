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
    public class MegaPunch : DamageMove
    {
        public override string MoveName => "Mega Punch";
        public override string MoveDescription => "The target is slugged by a punch thrown with muscle-packed power.";
        public override int Damage => 80;
        public override int Accuracy => 85;
        public override int MaxPP => 20;
        public override int MaxBoostPP => 32;
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

        private float startingZoom;
        private float zoomOutTarget = 1.5f;
        private float zoomInTarget = 3.5f;

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

                startingZoom = Main.GameZoomTarget;
            }
            else if (AnimationFrame == 176)
            {
                TerramonMod.ZoomAnimator.GameZoom(zoomOutTarget, 700, Easing.None);

            }
            else if (AnimationFrame == 225)
            {
                MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                TerramonMod.ZoomAnimator.GameZoom(zoomInTarget, 320, Easing.OutExpo);
                Main.LocalPlayer.GetModPlayer<TerramonPlayer>().battleScreenShake = true;
                Projectile.NewProjectile(target.projectile.Center, Vector2.Zero, ModContent.ProjectileType<MegaPunchFist>(), 0, 0);

                for (int i = 0; i < 18; i++)
                {
                    Dust.NewDust(target.projectile.Center - (target.projectile.Size / 2), target.projectile.height, target.projectile.height,
                        DustID.DiamondBolt, 0, 0, 0, Color.Red);
                }
            }
            else if (AnimationFrame == 310)
            {
                Main.LocalPlayer.GetModPlayer<TerramonPlayer>().battleScreenShake = false;
                TerramonMod.ZoomAnimator.GameZoom(startingZoom, 320, Easing.OutExpo);
            }
            else if (AnimationFrame == 340)
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

    public class MegaPunchFist : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 26;
            projectile.friendly = true;
            projectile.alpha = 0;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 3000;
            projectile.scale = 1f;
        }

        public int live;

        public override void AI()
        {
            live++;

            if (live > 70) projectile.alpha += 35;
        }
    }
}

