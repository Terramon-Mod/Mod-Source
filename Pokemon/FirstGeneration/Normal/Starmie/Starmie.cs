using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Starmie
{
    public class Starmie : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water, PokemonType.Psychic };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 60; public override int PhysicalDamage => 75; public override int PhysicalDefence => 85; public override int SpecialDamage => 100; public override int SpecialDefence => 85; public override int Speed => 115;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}