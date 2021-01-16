using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Clefable
{
    public class Clefable : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fairy };

        public override ExpGroup ExpGroup => ExpGroup.Fast;public override int MaxHP => 95; public override int PhysicalDamage => 70; public override int PhysicalDefence => 73; public override int SpecialDamage => 95; public override int SpecialDefence => 90; public override int Speed => 60;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}