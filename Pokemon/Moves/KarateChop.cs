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
    public class KarateChop : DamageMove
    {
		public override string MoveName => "Karate Chop";
		public override string MoveDescription => "The target is attacked with a sharp chop. Critical hits land more easily.";
		public override int Damage => 50;
		public override int Accuracy => 100;
		public override int MaxPP => 25;
		public override int MaxBoostPP => 40;
		public override bool MakesContact => true;
		public override bool Special => false;
		public override Target Target => Target.Opponent;
		public override int Cooldown => 60 * 1; 
		public override PokemonType MoveType => PokemonType.Fighting;
		public override bool HighCritRatio => true; // Boosts crit stage by 1 at damage calculation

		public override int AutoUseWeight(ParentPokemon mon, Vector2 pos, TerramonPlayer player)
		{
			NPC target = GetNearestNPC(pos);
			if (target == null)
				return 0;
			return 30;
		}

		private int kchop;

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
			}
			else if (AnimationFrame == 154) MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
			else if (AnimationFrame == 155)
			{
				kchop = Projectile.NewProjectile(target.projectile.Center + new Vector2(0, -120), new Vector2(0, 0), ModContent.ProjectileType<KarateChopHand>(), 0, 0);
				Main.projectile[kchop].velocity.Y = 6;
				if (mon.projectile.spriteDirection == 1) Main.projectile[kchop].spriteDirection = -1;
				else Main.projectile[kchop].spriteDirection = 1;
			}
			else if (AnimationFrame == 170)
			{
				Vector2 stored = Main.projectile[kchop].Center + new Vector2(-20, 0);
				Main.projectile[kchop].timeLeft = 0;
				Main.projectile[kchop].active = false;

				for (int i = 0; i < 22; i++)
				{
					Dust.NewDust(stored, Main.projectile[kchop].width, Main.projectile[kchop].height,
						ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"), 0, 0, 0, Color.DarkGray);
				}

				for (int i = 0; i < 18; i++)
				{
					Dust.NewDust(stored, Main.projectile[kchop].width, Main.projectile[kchop].height,
						53, 0, 0, 0, Color.DarkGray);
				}
			}
			else if (AnimationFrame == 200)
			{
				Main.projectile[kchop].timeLeft = 0;
				Main.projectile[kchop].active = false;
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

	public class KarateChopHand : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.timeLeft = 20000;
			projectile.scale = 1f;
		}

		internal Vector2 vel;

		public override void AI()
		{
			if (projectile.alpha != 0) projectile.alpha -= 26; // Fade in
		}
	}
}

