using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Dusts
{
    public class AcidBubbleDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 18, 18);
            dust.scale = 0f;
            dust.noLight = true;
        }

        public override bool Update(Dust dust)
        {
            dust.velocity.X = 0f;
            dust.velocity.Y = -1.3f;
            dust.position += dust.velocity;
            if (dust.scale < 1f) dust.scale += 0.02f;
            if (dust.scale > 1f) dust.active = false;
            return false;
        }
    }
}