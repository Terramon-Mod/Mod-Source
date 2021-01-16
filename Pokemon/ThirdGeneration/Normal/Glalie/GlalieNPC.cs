using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.ThirdGeneration.Normal.Glalie
{
    public class GlalieNPC : ParentPokemonNPC
    {
        public override string Texture => "Terramon/Pokemon/ThirdGeneration/Normal/Glalie/Glalie";
        public override Type HomeClass()
        {
            return typeof(Glalie);
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
            if (spawnInfo.player.ZoneSnow)
                return 0.015f;
            return 0f;
        }
    }
}

