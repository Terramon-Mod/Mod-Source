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
    public class AcidArmor : DamageMove
    {
        public override string MoveName => "Acid Armor";
        public override string MoveDescription => "The user alters its cellular structure to liquefy itself, sharply raising its Defense stat.";
        public override int Damage => 0;
        public override int Accuracy => -1;
        public override int MaxPP => 20;
        public override int MaxBoostPP => 32;
        public virtual bool MakesContact => false;
        public override Target Target => Target.Self;
        public override int Cooldown => 60 * 1; //Once per second
        public override PokemonType MoveType => PokemonType.Poison;

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
        private string s;
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

                mon.acidArmor = true;

                Dust bubbleDust = Dust.NewDustDirect(mon.projectile.position + new Vector2(Main.rand.Next(-12, 12), Main.rand.Next(-12, 12)), mon.projectile.width, mon.projectile.height, ModContent.GetInstance<TerramonMod>().DustType("AcidBubbleDust"), 0f, 0f, 0);
            }
            else if (AnimationFrame == 150)
            {
                Dust bubbleDust = Dust.NewDustDirect(mon.projectile.position + new Vector2(Main.rand.Next(-12, 12), Main.rand.Next(-12, 12)), mon.projectile.width, mon.projectile.height, ModContent.GetInstance<TerramonMod>().DustType("AcidBubbleDust"), 0f, 0f, 0);
            }
            else if (AnimationFrame == 160)
            {
                Dust bubbleDust = Dust.NewDustDirect(mon.projectile.position + new Vector2(Main.rand.Next(-12, 12), Main.rand.Next(-12, 12)), mon.projectile.width, mon.projectile.height, ModContent.GetInstance<TerramonMod>().DustType("AcidBubbleDust"), 0f, 0f, 0);
            }
            else if (AnimationFrame == 195)
            {
                Dust bubbleDust = Dust.NewDustDirect(mon.projectile.position + new Vector2(Main.rand.Next(-12, 12), Main.rand.Next(-12, 12)), mon.projectile.width, mon.projectile.height, ModContent.GetInstance<TerramonMod>().DustType("AcidBubbleDust"), 0f, 0f, 0);
            }
            else if (AnimationFrame == 205)
            {
                Dust bubbleDust = Dust.NewDustDirect(mon.projectile.position + new Vector2(Main.rand.Next(-12, 12), Main.rand.Next(-12, 12)), mon.projectile.width, mon.projectile.height, ModContent.GetInstance<TerramonMod>().DustType("AcidBubbleDust"), 0f, 0f, 0);
            }
            else if (AnimationFrame == 280)
            {
                mon.acidArmor = false;
                BattleMode.queueEndMove = true;
            }

            if (AnimationFrame > 140 && AnimationFrame < 280)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust dust1 = Dust.NewDustDirect(mon.projectile.position, mon.projectile.width, mon.projectile.height, 86, 0f, 0f, 0);
                    dust1.alpha = 0;
                    dust1.noGravity = true;
                }
            }

            // This should be at the very bottom of AnimateTurn() in every move.
            if (BattleMode.moveEnd)
            {
                endMoveTimer++;

                // Acid Armor raises the user's Defense by two stages.

                if (endMoveTimer == 1)
                {
                    s = ModifyStat(attacker, mon, GetStat.Defense, 2, state, !opponent).ToString();
                    BattleMode.UI.splashText.SetText(s);
                }
                if (endMoveTimer == 140)
                {
                    endMoveTimer = 0;
                    AnimationFrame = 0;
                    BattleMode.moveEnd = false;
                    return false;
                }
            }

            // IGNORE EVERYTHING BELOW WHEN MAKING YOUR OWN MOVES.
            if (AnimationFrame > 1810) return false;

            return true;
        }
    }
}