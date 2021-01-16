using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Shellder
{
    public class Shellder : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.WaterStone;

        public override Type EvolveTo => typeof(Cloyster.Cloyster);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.Slow;public override int MaxHP => 30; public override int PhysicalDamage => 65; public override int PhysicalDefence => 100; public override int SpecialDamage => 45; public override int SpecialDefence => 25; public override int Speed => 40;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}