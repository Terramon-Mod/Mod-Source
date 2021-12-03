using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Golbat
{
    public class GolbatNPC : ParentPokemonNPCFlyingBird
    {
        public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Golbat/Golbat";
        public override Type HomeClass()
        {
            return typeof(Golbat);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.width = 20;
            npc.height = 20;
            npc.scale = 1f;
        }

        public static bool PlayerIsInForest(Player player)
        {
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

        public static bool PlayerIsInEvils(Player player)
        {
            return player.ZoneCrimson
                || player.ZoneCorrupt;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.player;
            if (PlayerIsInForest(player) && spawnInfo.player.ZoneRockLayerHeight)
                return 0.03f;
            return 0f;
        }
    }
}
