using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Goldeen
{
    public class Goldeen : ParentPokemon
    {
        public override int EvolveCost => 28;

        public override Type EvolveTo => typeof(Seaking.Seaking);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 45; public override int PhysicalDamage => 67; public override int PhysicalDefence => 60; public override int SpecialDamage => 35; public override int SpecialDefence => 50; public override int Speed => 63;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}