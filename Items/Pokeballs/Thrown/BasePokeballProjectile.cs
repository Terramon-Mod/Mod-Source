using System;
using Microsoft.Xna.Framework;
using Terramon.Items.Pokeballs.Parts;
using Terramon.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Items.Pokeballs.Thrown
{
    public abstract class BasePokeballProjectile : TerramonProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            projectile.damage = 1;
            projectile.width = 17;
            projectile.height = 17;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
            projectile.light = 1f;
            projectile.scale = 1f;
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.ai[0] == 2)
                return true;

            if (projectile.soundDelay == 0)
                if (Main.netMode != NetmodeID.Server)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/ballbounce")
                        .WithVolume(.7f));
            projectile.soundDelay = 10;


            if (projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f)
                projectile.velocity.X = oldVelocity.X * -0.4f;
            if (projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f)
                projectile.velocity.Y = oldVelocity.Y * -0.4f;

            if (projectile.type == ModContent.ProjectileType<PokeballProjectile>())
                if (Main.rand.Next(12) == 0)
                {
                    projectile.timeLeft = 0;
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<PokeballCap>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<Button>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<PokeballBase>());
                }

            if (projectile.type == ModContent.ProjectileType<GreatBallProjectile>())
                if (Main.rand.Next(12) == 0)
                {
                    projectile.timeLeft = 0;
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<GreatBallCap>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<Button>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<PokeballBase>());
                }

            if (projectile.type == ModContent.ProjectileType<UltraBallProjectile>())
                if (Main.rand.Next(12) == 0)
                {
                    projectile.timeLeft = 0;
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<UltraBallCap>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<Button>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<PokeballBase>());
                }

            if (projectile.type == ModContent.ProjectileType<MasterBallProjectile>())
                if (Main.rand.Next(12) == 0)
                {
                    projectile.timeLeft = 0;
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<MasterBallCap>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<Button>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<PokeballBase>());
                }

            if (projectile.type == ModContent.ProjectileType<DuskBallProjectile>())
                if (Main.rand.Next(12) == 0)
                {
                    projectile.timeLeft = 0;
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<DuskBallCap>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<Button>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<DuskBallBase>());
                }

            if (projectile.type == ModContent.ProjectileType<PremierBallProjectile>())
                if (Main.rand.Next(12) == 0)
                {
                    projectile.timeLeft = 0;
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<PremierBallCap>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<Button>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<PokeballBase>());
                }

            if (projectile.type == ModContent.ProjectileType<QuickBallProjectile>())
                if (Main.rand.Next(12) == 0)
                {
                    projectile.timeLeft = 0;
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<QuickBallCap>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<Button>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<QuickBallBase>());
                }

            if (projectile.type == ModContent.ProjectileType<TimerBallProjectile>())
                if (Main.rand.Next(12) == 0)
                {
                    projectile.timeLeft = 0;
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<TimerBallCap>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<Button>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<PokeballBase>());
                }

            if (projectile.type == ModContent.ProjectileType<PumpkinBallProjectile>())
                if (Main.rand.Next(12) == 0)
                {
                    projectile.timeLeft = 0;
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<PumpkinBallCap>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<DarkButton>());
                    Item.NewItem(projectile.getRect(), ModContent.ItemType<PokeballBase>());
                }

            return false;
        }
    }
}