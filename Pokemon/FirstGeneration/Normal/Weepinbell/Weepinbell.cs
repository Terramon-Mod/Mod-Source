using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Weepinbell
{
    public class Weepinbell : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.LeafStone;

        public override Type EvolveTo => typeof(Victreebel.Victreebel);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Grass, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 65; public override int PhysicalDamage => 90; public override int PhysicalDefence => 50; public override int SpecialDamage => 85; public override int SpecialDefence => 45; public override int Speed => 55;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}