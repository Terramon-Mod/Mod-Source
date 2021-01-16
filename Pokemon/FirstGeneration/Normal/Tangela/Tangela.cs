using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Tangela
{
    public class Tangela : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Grass };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 65; public override int PhysicalDamage => 55; public override int PhysicalDefence => 115; public override int SpecialDamage => 100; public override int SpecialDefence => 40; public override int Speed => 60;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}