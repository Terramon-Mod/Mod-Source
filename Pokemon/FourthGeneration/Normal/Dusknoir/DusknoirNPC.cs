using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FourthGeneration.Normal.Dusknoir
{
    public class DusknoirNPC : ParentPokemonNPC
    {
        public override string Texture => "Terramon/Pokemon/FourthGeneration/Normal/Dusknoir/Dusknoir";
        public override Type HomeClass()
        {
            return typeof(Dusknoir);
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
            if (!Main.dayTime)
                return 0f;
            return 0f;
        }
    }
}
