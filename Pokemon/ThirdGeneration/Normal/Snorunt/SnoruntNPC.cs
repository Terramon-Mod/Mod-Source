using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.ThirdGeneration.Normal.Snorunt
{
    public class SnoruntNPC : ParentPokemonNPC
    {
        public override string Texture => "Terramon/Pokemon/ThirdGeneration/Normal/Snorunt/Snorunt";
        public override Type HomeClass()
        {
            return typeof(Snorunt);
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
                return 0.04f;
            return 0f;
        }
    }
}

