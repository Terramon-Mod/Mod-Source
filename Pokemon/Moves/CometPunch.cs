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
    public class CometPunch : DamageMove
    {
        public override string MoveName => "Comet Punch";
        public override string MoveDescription => "The target is hit with a flurry of punches that strike two to five times in a row.";
        public override int Damage => 18;
        public override int Accuracy => 85;
        public override int MaxPP => 15;
        public override int MaxBoostPP => 24;
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

        private int kchop;
        private int hits = 0;
        private int hitCountTimer = 0;
        private bool displayHitCountAndEnd = false;

        public override bool AnimateTurn(ParentPokemon mon, ParentPokemon target, TerramonPlayer player, PokemonData attacker,
            PokemonData deffender, BattleState state, bool opponent)
        {
            // Finished all hits
            if (displayHitCountAndEnd)
            {
                hitCountTimer++;
                if (hitCountTimer == 55)
                {
                    BattleMode.UI.splashText.SetText($"Hit {hits} time(s)!");
                    BattleMode.queueEndMove = true;
                }
            }

            if (!displayHitCountAndEnd)
            {
                if (AnimationFrame == 1) //At initial frame we pan camera to attacker
                {
                    TerramonMod.ZoomAnimator.ScreenPosX(mon.projectile.position.X + 12, 500, Easing.OutExpo);
                    TerramonMod.ZoomAnimator.ScreenPosY(mon.projectile.position.Y, 500, Easing.OutExpo);

                    // Decide total amounts to hit
                    hits = GetHits();
                }
                else if (AnimationFrame == 140)
                {
                    BattleMode.UI.splashText.SetText("");

                    TerramonMod.ZoomAnimator.ScreenPosX(target.projectile.position.X + 12, 500, Easing.OutExpo);
                    TerramonMod.ZoomAnimator.ScreenPosY(target.projectile.position.Y, 500, Easing.OutExpo);
                }
                else if (AnimationFrame == 154) MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                else if (AnimationFrame == 155) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 160) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 165) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 170) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 175) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 210)
                {
                    InflictDamage(mon, target, player, attacker, deffender, state, opponent);
                    if (PostTextLoc.Args.Length >= 4) //If we can extract damage number
                        CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]); //Print combat text at attacked mon position
                }

                // SECOND HIT, GUARENTEED

                //if (AnimationFrame == 269) MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                else if (AnimationFrame == 270) MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                else if (AnimationFrame == 275) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 280) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 285) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 290) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 295) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 330)
                {
                    InflictDamage(mon, target, player, attacker, deffender, state, opponent);
                    if (PostTextLoc.Args.Length >= 4) //If we can extract damage number
                        CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]); //Print combat text at attacked mon position
                    if (hits == 2 || deffender.HP == 0)
                    {
                        displayHitCountAndEnd = true;
                    }
                }

                // THIRD HIT, NOT GUARENTEED

                //if (AnimationFrame == 389) MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                else if (AnimationFrame == 390) MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                else if (AnimationFrame == 395) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 400) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 405) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 410) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 415) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 450)
                {
                    InflictDamage(mon, target, player, attacker, deffender, state, opponent);
                    if (PostTextLoc.Args.Length >= 4) //If we can extract damage number
                        CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]); //Print combat text at attacked mon position
                    if (hits == 3 || deffender.HP == 0)
                    {
                        displayHitCountAndEnd = true;
                    }
                }

                // FOURTH HIT, NOT GUARENTEED

                //if (AnimationFrame == 509) MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                else if (AnimationFrame == 510) MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                else if (AnimationFrame == 515) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 520) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 525) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 530) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 535) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 570)
                {
                    InflictDamage(mon, target, player, attacker, deffender, state, opponent);
                    if (PostTextLoc.Args.Length >= 4) //If we can extract damage number
                        CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]); //Print combat text at attacked mon position
                    if (hits == 4 || deffender.HP == 0)
                    {
                        displayHitCountAndEnd = true;
                    }
                }

                // FIFTH HIT, NOT GUARENTEED

                //if (AnimationFrame == 629) MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                else if (AnimationFrame == 630) MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                else if (AnimationFrame == 635) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 640) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 645) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 650) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 655) Projectile.NewProjectile(target.projectile.Center + new Vector2(Main.rand.Next(-18, 18), Main.rand.Next(-18, 18)), Vector2.Zero, ModContent.ProjectileType<CometPunchFist>(), 0, 0);
                else if (AnimationFrame == 690)
                {
                    InflictDamage(mon, target, player, attacker, deffender, state, opponent);
                    if (PostTextLoc.Args.Length >= 4) //If we can extract damage number
                        CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]); //Print combat text at attacked mon position
                    if (hits == 5 || deffender.HP == 0)
                    {
                        displayHitCountAndEnd = true;
                    }
                }
            }

            // This should be at the very bottom of AnimateTurn() in every move.
            if (BattleMode.moveEnd)
            {
                AnimationFrame = 0;
                hits = 0;
                hitCountTimer = 0;
                displayHitCountAndEnd = false;
                BattleMode.moveEnd = false;
                return false;
            }

            // IGNORE EVERYTHING BELOW WHEN MAKING YOUR OWN MOVES.
            if (AnimationFrame > 1810) return false;

            return true;
        }

        public int GetHits()
        {
            float rnd = _mrand.NextFloat();
            if (rnd <= 0.375f)
            {
                return 2;
            }
            else if (rnd > 0.375f && rnd <= 0.75f)
            {
                return 3;
            }
            else if (rnd > 0.75f && rnd <= 0.875f)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }

    }

    public class CometPunchFist : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 50;
            projectile.friendly = true;
            projectile.alpha = 0;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 6000;
            projectile.scale = 0.7f;
        }

        public int live;

        public override void AI()
        {
            live++;

            if (live > 8) projectile.alpha += 45;
        }
    }
}

