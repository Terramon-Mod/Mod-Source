using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Normal.Magmar
{
    public class MagmarNPC : ParentPokemonNPC
    { public override string Texture => "Terramon/Pokemon/FirstGeneration/Normal/Magmar/Magmar";
        public override Type HomeClass()
        {
            return typeof(Magmar);
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
            if (spawnInfo.player.ZoneUnderworldHeight)
                return 0.03f;
            return 0f;
        }
    }
}
