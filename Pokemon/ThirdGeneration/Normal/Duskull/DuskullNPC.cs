using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.ThirdGeneration.Normal.Duskull
{
    public class DuskullNPC : ParentPokemonNPCFlying
    { public override string Texture => "Terramon/Pokemon/ThirdGeneration/Normal/Duskull/Duskull";
        public override Type HomeClass()
        {
            return typeof(Duskull);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.width = 20;
            npc.height = 20;
            npc.scale = 1f;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.player;
            if (Main.halloween && !Main.dayTime)
                return 0.05f;
            return 0f;
        }
    }
}
