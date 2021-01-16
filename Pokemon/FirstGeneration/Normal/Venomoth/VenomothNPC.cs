using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Venomoth
{
    public class VenomothNPC : ParentPokemonNPCFlyingBird
    { public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Venomoth/Venomoth";
        public override Type HomeClass()
        {
            return typeof(Venomoth);
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
                return 0.035f;
            return 0f;
        }
    }
}
