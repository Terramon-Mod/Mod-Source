using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Krabby
{
    public class KrabbyNPC : ParentPokemonNPC
    { public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Krabby/Krabby";
        public override Type HomeClass()
        {
            return typeof(Krabby);
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
                return 0.05f;
            return 0f;
        }
    }
}
