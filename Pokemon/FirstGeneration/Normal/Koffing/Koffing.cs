using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Koffing
{
    public class Koffing : ParentPokemonGastly
    {
        public override int EvolveCost => 30;

        public override Type EvolveTo => typeof(Weezing.Weezing);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 40; public override int PhysicalDamage => 65; public override int PhysicalDefence => 95; public override int SpecialDamage => 60; public override int SpecialDefence => 45; public override int Speed => 35;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}