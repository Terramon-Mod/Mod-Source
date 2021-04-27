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
    public class DoubleSlap : DamageMove
    {
        public override string MoveName => "Double Slap";
        public override string MoveDescription => "The target is slapped repeatedly, back and forth, two to five times in a row.";
        public override int Damage => 15;
        public override int Accuracy => 85;
        public override int MaxPP => 10;
        public override int MaxBoostPP => 16;
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
                else if (AnimationFrame == 155)
                {
                    kchop = Projectile.NewProjectile(target.projectile.Center + new Vector2(-90, 0), new Vector2(12, 0), ModContent.ProjectileType<Hand>(), 0, 0);
                }
                else if (AnimationFrame == 170)
                {
                    Main.projectile[kchop].velocity = new Vector2(-12, 0);
                    Vector2 stored = target.projectile.position;

                    for (int i = 0; i < 16; i++)
                    {
                        Dust.NewDust(stored, target.projectile.width, target.projectile.height,
                            ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"), 0, 0, 0, Color.White);
                    }
                }
                else if (AnimationFrame == 188)
                {
                    Vector2 stored = target.projectile.position;

                    for (int i = 0; i < 16; i++)
                    {
                        Dust.NewDust(stored, target.projectile.width, target.projectile.height,
                            ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"), 0, 0, 0, Color.White);
                    }
                }
                else if (AnimationFrame == 190)
                {
                    Main.projectile[kchop].timeLeft = 0;
                    Main.projectile[kchop].active = false;
                    InflictDamage(mon, target, player, attacker, deffender, state, opponent);
                    if (PostTextLoc.Args.Length >= 4) //If we can extract damage number
                        CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]); //Print combat text at attacked mon position
                                                                                                                           //BattleMode.queueEndMove = true;
                }

                // SECOND HIT, GUARENTEED

                if (AnimationFrame == 250)
                {
                    MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                    kchop = Projectile.NewProjectile(target.projectile.Center + new Vector2(-90, 0), new Vector2(12, 0), ModContent.ProjectileType<Hand>(), 0, 0);
                }
                else if (AnimationFrame == 265)
                {
                    Main.projectile[kchop].velocity = new Vector2(-12, 0);
                    Vector2 stored = target.projectile.position;

                    for (int i = 0; i < 16; i++)
                    {
                        Dust.NewDust(stored, target.projectile.width, target.projectile.height,
                            ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"), 0, 0, 0, Color.White);
                    }
                }
                else if (AnimationFrame == 283)
                {
                    Vector2 stored = target.projectile.position;

                    for (int i = 0; i < 16; i++)
                    {
                        Dust.NewDust(stored, target.projectile.width, target.projectile.height,
                            ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"), 0, 0, 0, Color.White);
                    }
                }
                else if (AnimationFrame == 285)
                {
                    Main.projectile[kchop].timeLeft = 0;
                    Main.projectile[kchop].active = false;
                    InflictDamage(mon, target, player, attacker, deffender, state, opponent);
                    if (PostTextLoc.Args.Length >= 4) //If we can extract damage number
                        CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]); //Print combat text at attacked mon position
                    if (hits == 2 || deffender.HP == 0)
                    {
                        displayHitCountAndEnd = true;
                    }
                }

                // THIRD HIT, NOT GUARENTEED

                if (AnimationFrame == 345)
                {
                    MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                    kchop = Projectile.NewProjectile(target.projectile.Center + new Vector2(-90, 0), new Vector2(12, 0), ModContent.ProjectileType<Hand>(), 0, 0);
                }
                else if (AnimationFrame == 360)
                {
                    Main.projectile[kchop].velocity = new Vector2(-12, 0);
                    Vector2 stored = target.projectile.position;

                    for (int i = 0; i < 16; i++)
                    {
                        Dust.NewDust(stored, target.projectile.width, target.projectile.height,
                            ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"), 0, 0, 0, Color.White);
                    }
                }
                else if (AnimationFrame == 378)
                {
                    Vector2 stored = target.projectile.position;

                    for (int i = 0; i < 16; i++)
                    {
                        Dust.NewDust(stored, target.projectile.width, target.projectile.height,
                            ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"), 0, 0, 0, Color.White);
                    }
                }
                else if (AnimationFrame == 380)
                {
                    Main.projectile[kchop].timeLeft = 0;
                    Main.projectile[kchop].active = false;
                    InflictDamage(mon, target, player, attacker, deffender, state, opponent);
                    if (PostTextLoc.Args.Length >= 4) //If we can extract damage number
                        CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]); //Print combat text at attacked mon position
                    if (hits == 3 || deffender.HP == 0)
                    {
                        displayHitCountAndEnd = true;
                    }
                }

                // FOURTH HIT, NOT GUARENTEED

                if (AnimationFrame == 440)
                {
                    MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                    kchop = Projectile.NewProjectile(target.projectile.Center + new Vector2(-90, 0), new Vector2(12, 0), ModContent.ProjectileType<Hand>(), 0, 0);
                }
                else if (AnimationFrame == 455)
                {
                    Main.projectile[kchop].velocity = new Vector2(-12, 0);
                    Vector2 stored = target.projectile.position;

                    for (int i = 0; i < 16; i++)
                    {
                        Dust.NewDust(stored, target.projectile.width, target.projectile.height,
                            ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"), 0, 0, 0, Color.White);
                    }
                }
                else if (AnimationFrame == 473)
                {
                    Vector2 stored = target.projectile.position;

                    for (int i = 0; i < 16; i++)
                    {
                        Dust.NewDust(stored, target.projectile.width, target.projectile.height,
                            ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"), 0, 0, 0, Color.White);
                    }
                }
                else if (AnimationFrame == 475)
                {
                    Main.projectile[kchop].timeLeft = 0;
                    Main.projectile[kchop].active = false;
                    InflictDamage(mon, target, player, attacker, deffender, state, opponent);
                    if (PostTextLoc.Args.Length >= 4) //If we can extract damage number
                        CombatText.NewText(target.projectile.Hitbox, CombatText.DamagedHostile, (int)PostTextLoc.Args[3]); //Print combat text at attacked mon position
                    if (hits == 4 || deffender.HP == 0)
                    {
                        displayHitCountAndEnd = true;
                    }
                }

                // FIFTH HIT, NOT GUARENTEED

                if (AnimationFrame == 535)
                {
                    MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
                    kchop = Projectile.NewProjectile(target.projectile.Center + new Vector2(-90, 0), new Vector2(12, 0), ModContent.ProjectileType<Hand>(), 0, 0);
                }
                else if (AnimationFrame == 550)
                {
                    Main.projectile[kchop].velocity = new Vector2(-12, 0);
                    Vector2 stored = target.projectile.position;

                    for (int i = 0; i < 16; i++)
                    {
                        Dust.NewDust(stored, target.projectile.width, target.projectile.height,
                            ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"), 0, 0, 0, Color.White);
                    }
                }
                else if (AnimationFrame == 568)
                {
                    Vector2 stored = target.projectile.position;

                    for (int i = 0; i < 16; i++)
                    {
                        Dust.NewDust(stored, target.projectile.width, target.projectile.height,
                            ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"), 0, 0, 0, Color.White);
                    }
                }
                else if (AnimationFrame == 570)
                {
                    Main.projectile[kchop].timeLeft = 0;
                    Main.projectile[kchop].active = false;
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

    public class Hand : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 48;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 6000;
            projectile.scale = 1f;
        }

        public int live;

        public override void AI()
        {
            live++;

            if (projectile.alpha != 0 && live < 8) projectile.alpha -= 46; // Fade in

            if (live >= 21) projectile.alpha += 33;
        }
    }
}

