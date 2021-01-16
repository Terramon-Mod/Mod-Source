using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Exeggutor
{
    public class ExeggutorNPC : ParentPokemonNPC
    { public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Exeggutor/Exeggutor";
        public override Type HomeClass()
        {
            return typeof(Exeggutor);
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
            if (spawnInfo.player.ZoneJungle && spawnInfo.player.ZoneOverworldHeight && Main.hardMode)
                return 0.02f;
            return 0f;
        }
    }
}
