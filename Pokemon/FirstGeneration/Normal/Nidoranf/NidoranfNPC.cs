using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Nidoranf
{
    public class NidoranfNPC : ParentPokemonNPC
    {
        public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Nidoranf/Nidoranf";
        public override Type HomeClass()
        {
            return typeof(Nidoranf);
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
            if (PlayerIsInForest(player))
                return 0.055f;
            return 0f;
        }
    }
}
