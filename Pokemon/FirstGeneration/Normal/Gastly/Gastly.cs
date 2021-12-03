using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Gastly
{
    public class Gastly : ParentPokemonGastly
    {
        public override int EvolveCost => 20;

        public override Type EvolveTo => typeof(Haunter.Haunter);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ghost, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 30; public override int PhysicalDamage => 35; public override int PhysicalDefence => 30; public override int SpecialDamage => 100; public override int SpecialDefence => 35; public override int Speed => 80;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}