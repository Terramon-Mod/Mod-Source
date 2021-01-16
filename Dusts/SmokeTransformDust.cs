using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Dusts
{
    public class SmokeTransformDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            //dust.noGravity = false;
            dust.noLight = false;
            dust.scale *= 3f;

            dust.velocity *= 0.5f;
            dust.velocity.Y -= 0.5f;
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.1f;
            dust.scale -= 0.1f;
            if (dust.scale < 0.4f)
            {
                dust.active = false;
            }
            return false;
        }
    }
}