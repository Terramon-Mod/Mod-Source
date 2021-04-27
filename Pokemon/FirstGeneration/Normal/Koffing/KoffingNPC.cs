using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Koffing
{
    public class KoffingNPC : ParentPokemonNPCFlying
    {
        public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Koffing/Koffing";
        public override Type HomeClass()
        {
            return typeof(Koffing);
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
            if (PlayerIsInEvils(player))
                return 0.05f;
            return 0f;
        }
    }
}
