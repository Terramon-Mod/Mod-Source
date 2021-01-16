using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Slowbro
{
    public class Slowbro : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water, PokemonType.Psychic };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 95; public override int PhysicalDamage => 75; public override int PhysicalDefence => 110; public override int SpecialDamage => 100; public override int SpecialDefence => 80; public override int Speed => 30;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}