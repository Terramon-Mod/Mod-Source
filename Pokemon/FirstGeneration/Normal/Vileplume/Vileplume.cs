using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Vileplume
{
    public class Vileplume : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Grass, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 75; public override int PhysicalDamage => 80; public override int PhysicalDefence => 85; public override int SpecialDamage => 110; public override int SpecialDefence => 90; public override int Speed => 50;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}