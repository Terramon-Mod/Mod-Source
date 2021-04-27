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
    public class Acid : DamageMove
    {
        public override string MoveName => "Acid";
        public override string MoveDescription => "The opposing Pokémon are attacked with a spray of harsh acid. This may also lower their Sp. Def stat.";
        public override int Damage => 40;
        public override int Accuracy => 100;
        public override int MaxPP => 30;
        public override int MaxBoostPP => 48;
        public virtual bool MakesContact => false;
        public override bool Special => true;
        public override Target Target => Target.Opponent;
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
            Vector2 vel = (target.position + (target.Size / 2)) - (mon.projectile.position + (mon.projectile.Size / 2));
            var l = vel.Length();
            vel += target.velocity * (l / 100);//Make predict shoot
            vel.Normalize(); //Direction
            vel *= 15; //Speed
            Projectile.NewProjectile((mon.projectile.position + (mon.projectile.Size / 2)), vel, ProjectileID.DD2PhoenixBowShot, 20, 1f, player.whoAmI);
            return true;
        }

        private int acidBubble;
        private int acidBubble1;

        private int endMoveTimer;

        private string s = "";
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

                TerramonMod.ZoomAnimator.ScreenPosX(mon.projectile.Center.X, 300, Easing.OutExpo);
                TerramonMod.ZoomAnimator.ScreenPosY(mon.projectile.Center.Y - 60, 300, Easing.OutExpo);

                acidBubble = Projectile.NewProjectile(mon.projectile.Center + new Vector2(0, -60), new Vector2(0, 0), ModContent.ProjectileType<AcidBubble>(), 0, 0);
                Main.projectile[acidBubble].maxPenetrate = 99;
                Main.projectile[acidBubble].penetrate = 99;
            }
            else if (AnimationFrame == 170)
            {
                acidBubble1 = Projectile.NewProjectile(Main.projectile[acidBubble].position, new Vector2(0, 0), ModContent.ProjectileType<AcidBubble>(), 0, 0);
                Main.projectile[acidBubble1].alpha = 0;
                Main.projectile[acidBubble1].maxPenetrate = 99;
                Main.projectile[acidBubble1].penetrate = 99;
                Main.projectile[acidBubble].timeLeft = 0;
            }
            else if (AnimationFrame == 300)//At Last frame we destroy new proj
            {
                InflictDamage(mon, target, player, attacker, deffender, state, opponent);

                // create some particles

                for (int i = 0; i < 50; i++)
                {
                    Dust dust1 = Dust.NewDustDirect(target.projectile.position, target.projectile.width, target.projectile.height, 86, 0f, 0f, 0);
                    dust1.alpha = 0;
                    dust1.noGravity = true;
                }

                var id = acidBubble1;
                if (PostTextLoc.Args.Length >= 4)//If we can extract damage number
                    CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]);//Print combat text at attacked mon position
                Main.projectile[id].timeLeft = 0;
                Main.projectile[id].active = false;
                BattleMode.queueEndMove = true;
            }

            if (AnimationFrame > 170 && AnimationFrame < 301)
            {
                Main.projectile[acidBubble1].position = Interpolation.ValueAt(AnimationFrame, mon.projectile.Center + new Vector2(0, -60), target.projectile.position, 170, 300,
                    Easing.OutExpo);
                TerramonMod.ZoomAnimator.ScreenPosX(Main.projectile[acidBubble1].position.X, 1, Easing.None);
                TerramonMod.ZoomAnimator.ScreenPosY(Main.projectile[acidBubble1].position.Y, 1, Easing.None);
            }

            // This should be at the very bottom of AnimateTurn() in every move.
            if (BattleMode.moveEnd)
            {
                endMoveTimer++;

                // Acid deals damage and has a 10% chance of lowering the target's Special Defense by one stage.
                if (endMoveTimer == 1 && Main.rand.NextFloat() > .1323f)
                {
                    endMoveTimer = 0;
                    AnimationFrame = 0;
                    BattleMode.moveEnd = false;
                    return false;
                }

                if (endMoveTimer == 50)
                {
                    s = ModifyStat(deffender, target, GetStat.SpDef, -1, state, opponent).ToString();

                    BattleMode.UI.splashText.SetText(s);

                    TerramonMod.ZoomAnimator.ScreenPosX(target.projectile.position.X + 12, 500, Easing.OutExpo);
                    TerramonMod.ZoomAnimator.ScreenPosY(target.projectile.position.Y, 500, Easing.OutExpo);
                }
                if (endMoveTimer == 190)
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

    public class AcidBubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 20000;
            projectile.scale = 0.8f;
        }

        private int spawntimer;
        private int timer;
        private int pulse;

        private byte pulseMode = 0;

        private bool growing = false;

        internal Vector2 vel;

        public override void AI()
        {
            timer++;
            pulse++;
            if (projectile.alpha != 0) projectile.alpha -= 15;
            if (projectile.scale < 1.3f && !growing)
            {
                projectile.scale += 0.05f;
            }

            if (pulse >= 30 && pulse < 110)
            {
                projectile.scale -= 0.05f;
            }

            if (pulse >= 110)
            {
                projectile.scale += 0.05f;
            }

            if (timer >= 8)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 86, 0f, 0f, 0);
                    dust.velocity *= 0.2f;
                    dust.velocity = vel;
                    dust.noGravity = true;
                }
                timer = 0;
            }

            if (++projectile.frameCounter >= 10)
            {
                projectile.frameCounter = 0;
                projectile.frame = ++projectile.frame % Main.projFrames[projectile.type];
            }
        }
    }
}