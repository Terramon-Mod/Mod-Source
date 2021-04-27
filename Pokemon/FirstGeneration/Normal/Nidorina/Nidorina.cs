using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Nidorina
{
    public class Nidorina : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.MoonStone;

        public override Type EvolveTo => typeof(Nidoqueen.Nidoqueen);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 70; public override int PhysicalDamage => 62; public override int PhysicalDefence => 67; public override int SpecialDamage => 55; public override int SpecialDefence => 55; public override int Speed => 56;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}