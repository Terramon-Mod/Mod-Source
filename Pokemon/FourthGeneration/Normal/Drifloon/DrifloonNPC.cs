using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FourthGeneration.Normal.Drifblim
{
    public class DrifloonNPC : ParentPokemonNPCFlying
    { public override string Texture => "Terramon/Pokemon/FourthGeneration/Normal/Drifloon/Drifloon";
        public override Type HomeClass()
        {
            return typeof(Drifloon.Drifloon);
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
            if (Main.halloween && spawnInfo.player.ZoneSkyHeight)
                return 0.03f;
            return 0f;
        }
    }
}

