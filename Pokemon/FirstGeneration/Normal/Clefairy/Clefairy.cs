using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Clefairy
{
    public class Clefairy : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.MoonStone;

        public override Type EvolveTo => typeof(Clefable.Clefable);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fairy };

        public override ExpGroup ExpGroup => ExpGroup.Fast; public override int MaxHP => 70; public override int PhysicalDamage => 45; public override int PhysicalDefence => 48; public override int SpecialDamage => 60; public override int SpecialDefence => 65; public override int Speed => 35;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}