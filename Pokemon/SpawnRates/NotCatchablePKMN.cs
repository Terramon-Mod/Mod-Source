using Terraria.ModLoader;

namespace Terramon.Pokemon
{
    public abstract class NotCatchablePKMN : ParentPokemonNPC
    {
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }
    }
}