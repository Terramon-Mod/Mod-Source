using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FourthGeneration.Normal.Drifblim
{
    public class DrifblimNPC : ParentPokemonNPCFlying
    { public override string Texture => "Terramon/Pokemon/FourthGeneration/Normal/Drifblim/Drifblim";
        public override Type HomeClass()
        {
            return typeof(Drifblim);
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
		&& !player.ZoneDesert;
}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.player;
            if (spawnInfo.player.ZoneSkyHeight)
                return 0f;
            return 0f;
        }
    }
}

