using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Pokemon
{
	public class AngerOverlay : ModProjectile
	{
		public int RunTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anger Emote");
		}

		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			aiType = 0;
		}

		public override void AI()
		{
			RunTimer++;
			if (RunTimer == 2)
			{
				projectile.Kill();
				RunTimer = 0;
			}
		}
		public override void Kill(int timeLeft)
		{
			
		}
	}
}