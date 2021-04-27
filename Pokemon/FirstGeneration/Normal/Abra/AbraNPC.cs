using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Abra
{
    public class AbraNPC : ParentPokemonNPCFlying
    {
        public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Abra/Abra";
        public override Type HomeClass()
        {
            return typeof(Abra);
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
            if (spawnInfo.player.ZoneHoly && spawnInfo.player.ZoneOverworldHeight)
                return 0.05f;
            return 0f;
        }
    }
}
