using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Sandslash
{
    public class Sandslash : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ground };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 75; public override int PhysicalDamage => 100; public override int PhysicalDefence => 110; public override int SpecialDamage => 45; public override int SpecialDefence => 55; public override int Speed => 65;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}