using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Diglett
{
    public class DiglettNPC : ParentPokemonNPC
    { public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Diglett/Diglett";
        public override Type HomeClass()
        {
            return typeof(Diglett);
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
            if (spawnInfo.player.ZoneDesert)
                return 0.06f;
            return 0f;
        }
    }
}
