using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Abra
{
    public class Abra : ParentPokemonGastly
    {
        public override int EvolveCost => 11;

        public override Type EvolveTo => typeof(Kadabra.Kadabra);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Psychic };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 25; public override int PhysicalDamage => 20; public override int PhysicalDefence => 15; public override int SpecialDamage => 105; public override int SpecialDefence => 55; public override int Speed => 90;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}