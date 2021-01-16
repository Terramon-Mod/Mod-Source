using System;
using Microsoft.Xna.Framework;
using Terramon.Items.Pokeballs.Inventory;
using Terramon.Items.Pokeballs.Parts;
using Terramon.Pokemon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Items.Pokeballs.Thrown
{
    public class ZeroBallProjectile : BasePokeballProjectile
    {
        public Color zeroballparticles = new Color(22, 100, 148);
        public override void AI()
        {
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                if 
                    (
                        target.modNPC is ParentPokemonNPC parentNPC
                    )
                {
                    float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
                    float shootToY = target.position.Y - projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                    //If the distance between the live targeted npc and the projectile is less than 480 pixels
                    if (distance < 280f && target.active)
                    {
                        //Divide the factor, 3f, which is the desired velocity
                        distance = 3f / distance;

                        //Multiply the distance by a multiplier if you wish the projectile to have go faster
                        shootToX *= distance * 5;
                        shootToY *= distance * 5;

                        //Set the velocities to the shoot values
                        projectile.velocity.X = shootToX;
                        projectile.velocity.Y = shootToY;
                    }
                }
            }

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
                
                projectile.tileCollide = false;

                projectile.alpha = 255;

                projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
                projectile.width = 250;
                projectile.height = 250;
                projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
                projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            }
            else
            {

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
                    if ((double)projectile.velocity.X > -0.01 && (double)projectile.velocity.X < 0.01)
                    {
                        projectile.velocity.X = 0f;
                        projectile.netUpdate = true;
                    }
                }
                projectile.velocity.Y = projectile.velocity.Y + 0.2f;
            }

            projectile.rotation += projectile.velocity.X * 0.025f;
            return;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server)
                Main.PlaySound(SoundID.Item10, projectile.position);
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 60, 0, 0, 0, zeroballparticles);
            Vector2 usePos = projectile.position;

            Vector2 rotVector =
                (projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2();
            usePos += rotVector * 16f;

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(usePos, projectile.width, projectile.height, 60, 0, 0, 0, zeroballparticles);
                dust.position = (dust.position + projectile.Center) / 2f;
                dust.velocity += rotVector * 2f;
                dust.velocity *= 0.5f;
                dust.noGravity = true;
                usePos -= rotVector * 8f;
            }
        }
    }
}