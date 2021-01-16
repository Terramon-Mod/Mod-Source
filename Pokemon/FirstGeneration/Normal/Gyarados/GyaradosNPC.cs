using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Gyarados
{
    public class GyaradosNPC : ParentPokemonNPC
    {
        public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Gyarados/Gyarados";
        public override Type HomeClass()
        {
            return typeof(Gyarados);
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
            if (spawnInfo.player.ZoneBeach && spawnInfo.water && Main.hardMode)
                return 0.0125f;
            return 0f;
        }
    }
}
