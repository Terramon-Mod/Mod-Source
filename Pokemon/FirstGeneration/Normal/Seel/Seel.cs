using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Seel
{
    public class Seel : ParentPokemon
    {
        public override int EvolveCost => 29;

        public override Type EvolveTo => typeof(Dewgong.Dewgong);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 65; public override int PhysicalDamage => 45; public override int PhysicalDefence => 55; public override int SpecialDamage => 45; public override int SpecialDefence => 70; public override int Speed => 45;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}