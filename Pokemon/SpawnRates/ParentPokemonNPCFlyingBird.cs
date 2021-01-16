using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Pokemon
{
    public abstract class ParentPokemonNPCFlyingBird : ParentPokemonNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(PokeName());
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.defense = 0;
            npc.lifeMax = 1;
            npc.knockBackResist = 0.5f;
            npc.noGravity = true;

            npc.value = 0f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = mod.GetLegacySoundSlot(SoundType.NPCHit, "Sounds/NPCHit/capturepokemon");

            npc.aiStyle = 65;
            aiType = NPCID.Firefly;
        }

        private const int Flying1 = 0;
        private const int Flying2 = 1;
        public int AITimer;

        public override void AI()
        {
            npc.scale = 1f;

            npc.spriteDirection = npc.velocity.X > 0 ? -1 : (npc.velocity.X < 0 ? 1 : npc.spriteDirection);
            frameCounter++;
            if (frameCounter > 30)
            {
                frame += 1;
                frameCounter = 0;
                if (frame >= Main.npcFrameCount[npc.type])
                {
                    frame = 0;
                }
            }
        }
    }
}