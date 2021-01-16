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
    public class Agility : DamageMove
    {
		public override string MoveName => "Agility";
		public override string MoveDescription => "The user relaxes and lightens its body to move faster. This sharply raises the Speed stat.";
		public override int Damage => 50;
		public override int Accuracy => 100;
		public override int MaxPP => 30;
		public override int MaxBoostPP => 48;
		public override bool MakesContact => false;
		public override bool Special => false;
		public override Target Target => Target.Opponent;
		public override int Cooldown => 60 * 1; 
		public override PokemonType MoveType => PokemonType.Psychic;

		public override int AutoUseWeight(ParentPokemon mon, Vector2 pos, TerramonPlayer player)
		{
			NPC target = GetNearestNPC(pos);
			if (target == null)
				return 0;
			return 30;
		}

		public int xposStart;
		public int xposTarget;
		int adder;
		Vector2 oldCenter;
		Vector2 someCenterPoint;
		float rotTimer = 0;

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

				mon.useAi = false;
				mon.projectile.tileCollide = false;

				oldCenter = mon.projectile.Center;

				someCenterPoint = mon.projectile.Center + new Vector2(0, -20);

				for (int i = 0; i < 10; i++)
				{
					Dust.NewDust(mon.projectile.Center, mon.projectile.width, mon.projectile.height,
						ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"));
				}

				MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
			}
			else if (AnimationFrame == 170)
			{
				for (int i = 0; i < 10; i++)
				{
					Dust.NewDust(oldCenter, mon.projectile.width, mon.projectile.height,
						ModContent.GetInstance<TerramonMod>().DustType("SmokeTransformDust"));
				}

				mon.projectile.Center = oldCenter;

				if (mon.projectile.spriteDirection == -1) adder = 10;
				else adder = -10;

				xposStart = (int)mon.projectile.position.X;
				xposTarget = (int)mon.projectile.position.X + adder;
			}
			else if (AnimationFrame == 235)
			{
				string s = ModifyStat(attacker, mon, GetStat.Speed, 2, state, !opponent).ToString();
				BattleMode.UI.splashText.SetText(s);
			}
			else if (AnimationFrame >= 385)
			{
				mon.useAi = true;
				mon.projectile.tileCollide = true;
				AnimationFrame = 0;
				someCenterPoint = Vector2.Zero;
				oldCenter = Vector2.Zero;
				rotTimer = 0;
				xposStart = 0;
				xposTarget = 0;
				adder = 0;
				AnimationFrame = 0;
				BattleMode.moveEnd = false;
				return false;
			}
			else if (AnimationFrame > 170 && AnimationFrame < 177)
			{
				mon.projectile.position.X = Interpolation.ValueAt(AnimationFrame, xposStart, xposTarget, 170, 176,
					Easing.None);
			}
			else if (AnimationFrame > 176 && AnimationFrame < 185)
			{
				mon.projectile.position.X = Interpolation.ValueAt(AnimationFrame, xposTarget, xposStart - adder, 176, 184,
					Easing.None);
			}
			else if (AnimationFrame > 184 && AnimationFrame < 191)
			{
				mon.projectile.position.X = Interpolation.ValueAt(AnimationFrame, xposStart - adder, xposStart, 184, 190,
					Easing.None);
			}

			if (AnimationFrame > 140 && AnimationFrame < 170)
			{
				rotTimer += MathHelper.Pi * 6f / 30;

				mon.projectile.Center = someCenterPoint + Vector2.One.RotatedBy(rotTimer) * 6.3f;
			}

			// IGNORE EVERYTHING BELOW WHEN MAKING YOUR OWN MOVES.
			if (AnimationFrame > 1810) return false;

			return true;
		}

	}
}

