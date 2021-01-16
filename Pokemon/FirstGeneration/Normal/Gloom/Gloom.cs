using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Gloom
{
    public class Gloom : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.LeafStone;

        public override Type EvolveTo => typeof(Vileplume.Vileplume);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Grass, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 60; public override int PhysicalDamage => 65; public override int PhysicalDefence => 70; public override int SpecialDamage => 85; public override int SpecialDefence => 75; public override int Speed => 40;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}