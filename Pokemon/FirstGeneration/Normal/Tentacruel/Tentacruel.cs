using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Tentacruel
{
    public class Tentacruel : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 80; public override int PhysicalDamage => 70; public override int PhysicalDefence => 65; public override int SpecialDamage => 80; public override int SpecialDefence => 120; public override int Speed => 100;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}