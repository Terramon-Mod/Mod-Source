using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Ivysaur
{
    public class Ivysaur : ParentPokemon
    {
        public override int EvolveCost => 16;

        public override Type EvolveTo => typeof(Venusaur.Venusaur);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Grass, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 60; public override int PhysicalDamage => 62; public override int PhysicalDefence => 63; public override int SpecialDamage => 80; public override int SpecialDefence => 80; public override int Speed => 60;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}