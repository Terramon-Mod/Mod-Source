using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Tentacool
{
    public class Tentacool : ParentPokemon
    {
        public override int EvolveCost => 25;

        public override Type EvolveTo => typeof(Tentacruel.Tentacruel);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 40; public override int PhysicalDamage => 40; public override int PhysicalDefence => 35; public override int SpecialDamage => 50; public override int SpecialDefence => 100; public override int Speed => 70;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}