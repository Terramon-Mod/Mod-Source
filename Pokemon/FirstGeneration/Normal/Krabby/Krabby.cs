using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Krabby
{
    public class Krabby : ParentPokemon
    {
        public override int EvolveCost => 23;

        public override Type EvolveTo => typeof(Kingler.Kingler);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 30; public override int PhysicalDamage => 105; public override int PhysicalDefence => 90; public override int SpecialDamage => 25; public override int SpecialDefence => 25; public override int Speed => 50;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}