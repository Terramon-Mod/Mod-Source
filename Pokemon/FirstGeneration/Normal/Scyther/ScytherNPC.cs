using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Scyther
{
    public class ScytherNPC : ParentPokemonNPCFlyingBird
    {
        public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Scyther/Scyther";
        public override Type HomeClass()
        {
            return typeof(Scyther);
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
            if (spawnInfo.player.ZoneSkyHeight)
                return 0.03f;
            return 0f;
        }
    }
}
