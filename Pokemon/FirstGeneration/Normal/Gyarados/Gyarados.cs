using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Gyarados
{
    public class Gyarados : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 95; public override int PhysicalDamage => 125; public override int PhysicalDefence => 79; public override int SpecialDamage => 60; public override int SpecialDefence => 100; public override int Speed => 81;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}