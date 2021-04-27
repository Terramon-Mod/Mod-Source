using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Squirtle
{
    public class Squirtle : ParentPokemon
    {
        public override int EvolveCost => 11;

        public override Type EvolveTo => typeof(Wartortle.Wartortle);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 44; public override int PhysicalDamage => 48; public override int PhysicalDefence => 65; public override int SpecialDamage => 50; public override int SpecialDefence => 64; public override int Speed => 43;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}