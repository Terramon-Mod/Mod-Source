using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Golduck
{
    public class GolduckNPC : NotCatchablePKMN
    { public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Golduck/Golduck";
        public override Type HomeClass()
        {
            return typeof(Golduck);
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
            if (spawnInfo.player.ZoneBeach)
                return 0f;
            return 0f;
        }
    }
}
