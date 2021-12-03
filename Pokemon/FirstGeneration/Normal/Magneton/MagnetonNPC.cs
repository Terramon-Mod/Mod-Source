using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Magneton
{
    public class MagnetonNPC : ParentPokemonNPCFlying
    {
        public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Magneton/Magneton";
        public override Type HomeClass()
        {
            return typeof(Magneton);
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
            if (spawnInfo.player.ZoneGlowshroom)
                return 0.03f;
            return 0f;
        }
    }
}
