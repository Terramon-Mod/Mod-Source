using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Items.Pokeballs.Thrown
{
    public class UltraBallProjectile : BasePokeballProjectile
    {
        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                if (Main.netMode != NetmodeID.Server)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/throwball").WithVolume(.7f));
                projectile.ai[0] = 1;
                projectile.ai[1] = 1;

                projectile.netUpdate = true;
            }

            if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 3)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width,
                    projectile.height, 31);
                projectile.tileCollide = false;

                projectile.alpha = 255;

                projectile.position.X = projectile.position.X + projectile.width / 2;
                projectile.position.Y = projectile.position.Y + projectile.height / 2;
                projectile.width = 250;
                projectile.height = 250;
                projectile.position.X = projectile.position.X - projectile.width / 2;
                projectile.position.Y = projectile.position.Y - projectile.height / 2;
            }
            else
            {
                if (Main.rand.Next(3) == 0)
                {
                }
            }

            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 5f)
            {
                projectile.ai[0] = 10f;

                if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f)
                {
                    projectile.velocity.X = projectile.velocity.X * 0.97f;

                    {
                        projectile.velocity.X = projectile.velocity.X * 0.99f;
                    }
                    if (projectile.velocity.X > -0.01 && projectile.velocity.X < 0.01)
                    {
                        projectile.velocity.X = 0f;
                        projectile.netUpdate = true;
                    }
                }

                projectile.velocity.Y = projectile.velocity.Y + 0.2f;
            }

            projectile.rotation += projectile.velocity.X * 0.07f;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server)
                Main.PlaySound(SoundID.Item10, projectile.position);
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 64);
            Vector2 usePos = projectile.position;

            Vector2 rotVector =
                (projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2();
            usePos += rotVector * 16f;

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(usePos, projectile.width, projectile.height, 64);
                dust.position = (dust.position + projectile.Center) / 2f;
                dust.velocity += rotVector * 2f;
                dust.velocity *= 0.5f;
                dust.noGravity = true;
                usePos -= rotVector * 8f;
            }
        }
    }
}