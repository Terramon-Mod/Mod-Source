using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Venusaur
{
    public class Venusaur : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Grass, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 80; public override int PhysicalDamage => 82; public override int PhysicalDefence => 83; public override int SpecialDamage => 100; public override int SpecialDefence => 100; public override int Speed => 80;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}