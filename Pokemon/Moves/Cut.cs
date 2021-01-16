using Microsoft.Xna.Framework;
using Razorwing.Framework.Graphics;
using Terramon.Players;
using Terramon.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Pokemon.Moves
{
	public class Cut : DamageMove
	{
		public override string MoveName => "Cut";
		public override string MoveDescription => "The target is cut with a scythe or claw.";
		public override int Damage => 50;
		public override int Accuracy => 95;
		public override int MaxPP => 30;
		public override int MaxBoostPP => 48;
		public override bool MakesContact => true; 
		public override bool Special => false;
		public override Target Target => Target.Opponent;
		public override int Cooldown => 60 * 1; //Once per second
		public override PokemonType MoveType => PokemonType.Normal;

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

		int cutID;
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
			else if (AnimationFrame == 165)
			{
				MoveSound = Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/UI/BattleSFX/" + MoveName).WithVolume(.75f));
				cutID = Projectile.NewProjectile(target.projectile.Center, new Vector2(0, 0), ModContent.ProjectileType<CutProjectile>(), 0, 0);
				Main.projectile[cutID].maxPenetrate = 99;
				Main.projectile[cutID].penetrate = 99;
				Main.projectile[cutID].direction = mon.projectile.Center.X > target.projectile.Center.X ? -1 : 1;
				Main.projectile[cutID].spriteDirection = mon.projectile.Center.X > target.projectile.Center.X ? -1 : 1;
			}
			else if (AnimationFrame == 200)
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

	public class CutProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			if (++projectile.frameCounter >= 3)
			{
				projectile.frameCounter = 0;
				if (projectile.frame == Main.projFrames[projectile.type] - 1)
					projectile.Kill();
				projectile.frame = ++projectile.frame % Main.projFrames[projectile.type];
			}
		}
	}
}
