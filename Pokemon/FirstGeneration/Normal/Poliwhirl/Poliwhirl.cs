using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Poliwhirl
{
    public class Poliwhirl : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.WaterStone;

        public override Type EvolveTo => typeof(Poliwrath.Poliwrath);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 65; public override int PhysicalDamage => 65; public override int PhysicalDefence => 65; public override int SpecialDamage => 50; public override int SpecialDefence => 50; public override int Speed => 90;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}