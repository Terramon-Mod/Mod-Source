using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Kingler
{
    public class Kingler : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 55; public override int PhysicalDamage => 130; public override int PhysicalDefence => 115; public override int SpecialDamage => 50; public override int SpecialDefence => 50; public override int Speed => 75;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}