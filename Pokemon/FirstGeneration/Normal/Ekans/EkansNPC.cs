using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Ekans
{
    public class EkansNPC : ParentPokemonNPC
    { public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Ekans/Ekans";
        public override Type HomeClass()
        {
            return typeof(Ekans);
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
            if (PlayerIsInEvils(player) && spawnInfo.player.ZoneOverworldHeight)
                return 0.06f;
            return 0f;
        }
    }
}
