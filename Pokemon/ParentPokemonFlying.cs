using Terraria;
using Terraria.ID;

namespace Terramon.Pokemon
{
    public abstract class ParentPokemonFlying : ParentPokemon
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabyHornet);
            aiType = ProjectileID.BabyHornet;
            flying = true;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.zephyrfish = false; // Relic from aiType
            return true;
        }
    }
}