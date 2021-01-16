using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Staryu
{
    public class Staryu : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.WaterStone;

        public override Type EvolveTo => typeof(Starmie.Starmie);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.Slow;public override int MaxHP => 30; public override int PhysicalDamage => 45; public override int PhysicalDefence => 55; public override int SpecialDamage => 70; public override int SpecialDefence => 55; public override int Speed => 85;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}