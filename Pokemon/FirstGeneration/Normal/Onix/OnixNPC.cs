using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Onix
{
    public class OnixNPC : ParentPokemonNPC
    { public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Onix/Onix";
        public override Type HomeClass()
        {
            return typeof(Onix);
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
            if (spawnInfo.player.ZoneDesert && Main.hardMode)
                return 0.03f;
            return 0f;
        }
    }
}
