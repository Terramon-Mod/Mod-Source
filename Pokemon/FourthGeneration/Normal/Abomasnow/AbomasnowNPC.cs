using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FourthGeneration.Normal.Abomasnow
{
    public class AbomasnowNPC : ParentPokemonNPC
    {
        public override string Texture => "Terramon/Pokemon/FourthGeneration/Normal/Abomasnow/Abomasnow";
        public override Type HomeClass()
        {
            return typeof(Abomasnow);
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
            if (spawnInfo.player.ZoneSnow && spawnInfo.player.ZoneRockLayerHeight)
                return 0.015f;
            return 0f;
        }
    }
}

