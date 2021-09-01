using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terramon.Players;
using Terramon.Pokemon;
using Terraria;

namespace Terramon.Extensions
{
    public static class PokemonExtension
    {
        
        public static ParentPokemon Pokemon(this Projectile proj) => proj.modProjectile as ParentPokemon;
        public static TerramonPlayer TPlayer(this Player pl) => pl.GetModPlayer<TerramonPlayer>();

        public static bool IsWild(this NPC npc) => npc.modNPC is ParentPokemonNPC;
    }
}
