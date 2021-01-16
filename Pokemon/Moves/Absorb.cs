using Microsoft.Xna.Framework;
using Razorwing.Framework.Graphics;
using Razorwing.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terramon.Players;
using Terramon.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Pokemon.Moves
{
    public class Absorb : DamageMove
    {
        public override string MoveName => "Absorb";
        public override string MoveDescription => "A nutrient-draining attack. The user's HP is restored by half the damage taken by the target.";
        public override int Damage => 20;
        public override int Accuracy => 100;
        public override int MaxPP => 25;
        public override int MaxBoostPP => 40;
        public override bool Special => true;
        public virtual bool MakesContact => false;
        public override Target Target => Target.Opponent;
        public override int Cooldown => 60 * 1; //Once per second
        public override PokemonType MoveType => PokemonType.Grass;

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

        public int endMoveTimer;
        public const string PROJID_KEY = "move.projID";

        private int spore1, spore2, spore3, spore4, spore5, spore6;

        private Vector2 a, b, c, d, e, f;

        private float damageDealt;
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

                a = new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18));
                spore1 = Projectile.NewProjectile(target.projectile.Hitbox.Center() + a, new Vector2(0, 0), ModContent.ProjectileType<AbsorbSpore>(), 0, 0);
                Main.projectile[spore1].maxPenetrate = 99;
                Main.projectile[spore1].penetrate = 99;
            }
            else if (AnimationFrame == 155)
            {
                b = new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18));
                spore2 = Projectile.NewProjectile(target.projectile.Hitbox.Center() + b, new Vector2(0, 0), ModContent.ProjectileType<AbsorbSpore>(), 0, 0);
                Main.projectile[spore2].maxPenetrate = 99;
                Main.projectile[spore2].penetrate = 99;
            }
            else if (AnimationFrame == 170)
            {
                c = new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18));
                spore3 = Projectile.NewProjectile(target.projectile.Hitbox.Center() + c, new Vector2(0, 0), ModContent.ProjectileType<AbsorbSpore>(), 0, 0);
                Main.projectile[spore3].maxPenetrate = 99;
                Main.projectile[spore3].penetrate = 99;
            }
            else if (AnimationFrame == 185)
            {
                TerramonMod.ZoomAnimator.ScreenPosX(mon.projectile.position.X + 12, 500, Easing.OutExpo);
                TerramonMod.ZoomAnimator.ScreenPosY(mon.projectile.position.Y, 500, Easing.OutExpo);
                d = new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18));
                spore4 = Projectile.NewProjectile(target.projectile.Hitbox.Center() + d, new Vector2(0, 0), ModContent.ProjectileType<AbsorbSpore>(), 0, 0);
                Main.projectile[spore4].maxPenetrate = 99;
                Main.projectile[spore4].penetrate = 99;
            }
            else if (AnimationFrame == 200)
            {
                e = new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18));
                spore5 = Projectile.NewProjectile(target.projectile.Hitbox.Center() + e, new Vector2(0, 0), ModContent.ProjectileType<AbsorbSpore>(), 0, 0);
                Main.projectile[spore5].maxPenetrate = 99;
                Main.projectile[spore5].penetrate = 99;
            }
            else if (AnimationFrame == 215)
            {
                f = new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18));
                spore6 = Projectile.NewProjectile(target.projectile.Hitbox.Center() + f, new Vector2(0, 0), ModContent.ProjectileType<AbsorbSpore>(), 0, 0);
                Main.projectile[spore6].maxPenetrate = 99;
                Main.projectile[spore6].penetrate = 99;
            }
            else if (AnimationFrame == 265)//At Last frame we destroy new proj
            {
                TerramonMod.ZoomAnimator.ScreenPosX(target.projectile.position.X + 12, 500, Easing.OutExpo);
                TerramonMod.ZoomAnimator.ScreenPosY(target.projectile.position.Y, 500, Easing.OutExpo);
                damageDealt = InflictDamage(mon, target, player, attacker, deffender, state, opponent);
                if (PostTextLoc.Args.Length >= 4)//If we can extract damage number
                    CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]);//Print combat text at attacked mon position
                Main.projectile[spore1].timeLeft = 0;
                Main.projectile[spore1].active = false;
                Main.projectile[spore2].timeLeft = 0;
                Main.projectile[spore2].active = false;
                Main.projectile[spore3].timeLeft = 0;
                Main.projectile[spore3].active = false;
                Main.projectile[spore4].timeLeft = 0;
                Main.projectile[spore4].active = false;
                Main.projectile[spore5].timeLeft = 0;
                Main.projectile[spore5].active = false;
                Main.projectile[spore6].timeLeft = 0;
                Main.projectile[spore6].active = false;
                BattleMode.queueEndMove = true;
            }

            else if (AnimationFrame > 140 && AnimationFrame < 265)
            {
                //Vector2 vel = (target.projectile.position + (target.projectile.Size / 2)) - (mon.projectile.position + (mon.projectile.Size / 2));
                //var l = vel.Length();
                //vel.Normalize();
                //Main.projectile[id].position = mon.projectile.position + (vel * (l * (AnimationFrame / 120)));
                if (AnimationFrame < 190)
                {
                    var pos = Main.projectile[spore1].position;
                    Main.projectile[spore1].position = Interpolation.ValueAt(AnimationFrame, target.projectile.Hitbox.Center() + a, mon.projectile.Hitbox.Center(), 140, 190,
                    Easing.Out);
                    AbsorbSpore ai = (AbsorbSpore)Main.projectile[spore1].modProjectile;
                    ai.vel = Main.projectile[spore1].position - pos;
                }
                if (AnimationFrame > 155 && AnimationFrame < 205)
                {
                    var pos = Main.projectile[spore2].position;
                    Main.projectile[spore2].position = Interpolation.ValueAt(AnimationFrame, target.projectile.Hitbox.Center() + b, mon.projectile.Hitbox.Center(), 155, 205,
                    Easing.Out);
                    AbsorbSpore bi = (AbsorbSpore)Main.projectile[spore2].modProjectile;
                    bi.vel = Main.projectile[spore2].position - pos;
                }
                if (AnimationFrame > 170 && AnimationFrame < 220)
                {
                    var pos = Main.projectile[spore3].position;
                    Main.projectile[spore3].position = Interpolation.ValueAt(AnimationFrame, target.projectile.Hitbox.Center() + c, mon.projectile.Hitbox.Center(), 170, 220,
                    Easing.Out);
                    AbsorbSpore ci = (AbsorbSpore)Main.projectile[spore3].modProjectile;
                    ci.vel = Main.projectile[spore3].position - pos;
                }
                if (AnimationFrame > 185 && AnimationFrame < 235)
                {
                    var pos = Main.projectile[spore4].position;
                    Main.projectile[spore4].position = Interpolation.ValueAt(AnimationFrame, target.projectile.Hitbox.Center() + d, mon.projectile.Hitbox.Center(), 185, 235,
                    Easing.Out);
                    AbsorbSpore di = (AbsorbSpore)Main.projectile[spore4].modProjectile;
                    di.vel = Main.projectile[spore4].position - pos;
                }
                if (AnimationFrame > 200 && AnimationFrame < 250)
                {
                    var pos = Main.projectile[spore5].position;
                    Main.projectile[spore5].position = Interpolation.ValueAt(AnimationFrame, target.projectile.Hitbox.Center() + e, mon.projectile.Hitbox.Center(), 200, 250,
                    Easing.Out);
                    AbsorbSpore ei = (AbsorbSpore)Main.projectile[spore5].modProjectile;
                    ei.vel = Main.projectile[spore5].position - pos;
                }
                if (AnimationFrame > 215 && AnimationFrame < 265)
                {
                    var pos = Main.projectile[spore6].position;
                    Main.projectile[spore6].position = Interpolation.ValueAt(AnimationFrame, target.projectile.Hitbox.Center() + f, mon.projectile.Hitbox.Center(), 215, 265,
                    Easing.Out);
                    AbsorbSpore fi = (AbsorbSpore)Main.projectile[spore6].modProjectile;
                    fi.vel = Main.projectile[spore6].position - pos;
                }
            }

            // This should be at the very bottom of AnimateTurn() in every move.
            if (BattleMode.moveEnd)
            {
                endMoveTimer++;
                if (endMoveTimer >= 50 && endMoveTimer < 190)
                {
                    if (opponent)
                    {
                        BattleMode.UI.splashText.SetText($"{deffender.PokemonName} had its energy drained!");
                    } 
                    else
                    {
                        if (state == BattleState.BattleWithWild)
                        {
                            BattleMode.UI.splashText.SetText($"The wild {deffender.PokemonName} had its energy drained!");
                        }
                        else
                        if (state == BattleState.BattleWithTrainer)
                        {
                            BattleMode.UI.splashText.SetText($"The foe's {deffender.PokemonName} had its energy drained!");
                        }
                    }
                    //TerramonMod.ZoomAnimator.ScreenPos(mon.projectile.position + new Vector2(12, 0), 500, Easing.OutExpo);
                    TerramonMod.ZoomAnimator.ScreenPosX(mon.projectile.position.X + 12, 500, Easing.OutExpo);
                    TerramonMod.ZoomAnimator.ScreenPosY(mon.projectile.position.Y, 500, Easing.OutExpo);
                    //BattleMode.animWindow = 0;
                }
                if (endMoveTimer == 190)
                {
                    BattleMode.UI.splashText.SetText("");
                    // If this attack deals 1 HP of damage, 1 HP will be restored to the user.
                    if ((int)damageDealt == 1)
                    {
                        CombatText.NewText(mon.projectile.Hitbox, CombatText.HealLife, SelfHeal(attacker, mon, 1));
                    }
                    else
                    {
                        CombatText.NewText(mon.projectile.Hitbox, CombatText.HealLife, SelfHeal(attacker, mon, (int)damageDealt / 2));
                    }
                }
                if (endMoveTimer >= 330)
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

    public class AbsorbSpore : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.alpha = 0;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 20000;
        }

        private int spawntimer;
        private int timer;

        internal Vector2 vel;

        public override void AI()
        {
            timer++;
            if (timer >= 8)
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 75, 0f, 0f, 0);
                    dust.velocity *= 0.2f;
                    dust.velocity = vel;
                    dust.noGravity = true;
                }
                timer = 0;
            }
        }
    }
}
