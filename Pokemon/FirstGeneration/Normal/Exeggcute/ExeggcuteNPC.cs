using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Exeggcute
{
    public class ExeggcuteNPC : ParentPokemonNPC
    {
        public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Exeggcute/Exeggcute";
        public override Type HomeClass()
        {
            return typeof(Exeggcute);
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
            if (spawnInfo.player.ZoneJungle && spawnInfo.player.ZoneOverworldHeight)
                return 0.035f;
            return 0f;
        }
    }
}
