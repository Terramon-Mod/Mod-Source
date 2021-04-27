using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Pokemon
{
    public class ThrowBallAnimation : ModProjectile
    {
        public override string Texture => "Terramon/Items/Pokeballs/Inventory/PokeballItem";

        public int RunTimer = 0;

        private float visibility = 0f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Throw Ball Animation");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.alpha = 255;
            projectile.scale = 0.92f;
            projectile.timeLeft = 28;
            projectile.tileCollide = false;
            aiType = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D ballTexture = ModContent.GetTexture("Terramon/Items/Pokeballs/Inventory/PokeballItem");
            int frameHeight = 24;
            spriteBatch.Draw(ballTexture, projectile.position - Main.screenPosition + new Vector2(17, -16),
                new Rectangle(0, frameHeight * 0, ballTexture.Width, frameHeight), drawColor * visibility, projectile.rotation,
                new Vector2(ballTexture.Width / 2f, frameHeight / 2), projectile.scale, SpriteEffects.None, 0f);
            return true;
        }

        public override void AI()
        {
            RunTimer++;

            if (visibility < 1f) visibility += 0.065f;

            projectile.velocity.Y = 3;

            projectile.rotation += 0.23f;

            if (RunTimer == 1) // Set target
            {
                Main.PlaySound(ModContent.GetInstance<TerramonMod>()
                        .GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/throwballnew").WithVolume(0.55f));
            }
        }
        public override void Kill(int timeLeft)
        {

        }
    }
}