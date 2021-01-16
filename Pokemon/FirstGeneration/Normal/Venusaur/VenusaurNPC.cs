using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Venusaur
{
    public class VenusaurNPC : NotCatchablePKMN
    { public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Venusaur/Venusaur";
        public override Type HomeClass()
        {
            return typeof(Venusaur);
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
            if (spawnInfo.player.ZoneJungle)
                return 0f;
            return 0f;
        }
    }
}

