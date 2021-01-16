using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.SecondGeneration.Normal.Stantler
{
    public class StantlerNPC : ParentPokemonNPC
    { public override string Texture => "Terramon/Pokemon/SecondGeneration/Normal/Stantler/Stantler";
        public override Type HomeClass()
        {
            return typeof(Stantler);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.width = 20;
            npc.height = 20;
            npc.scale = 1f;
        }

public static bool PlayerIsInForest(Player player){
	return !player.ZoneJungle
		&& !player.ZoneDungeon
		&& !player.ZoneCorrupt
		&& !player.ZoneCrimson
		&& !player.ZoneHoly
		&& !player.ZoneSnow
		&& !player.ZoneUndergroundDesert
		&& !player.ZoneGlowshroom
		&& !player.ZoneMeteor
		&& !player.ZoneBeach
		&& !player.ZoneDesert
		&& player.ZoneOverworldHeight;
}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.player;
            if (PlayerIsInForest(player) && Main.dayTime)
                return 0.025f;
            return 0f;
        }
    }
}
