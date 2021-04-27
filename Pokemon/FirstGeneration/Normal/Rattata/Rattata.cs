using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Rattata
{
    public class Rattata : ParentPokemon
    {
        public override int EvolveCost => 15;

        public override Type EvolveTo => typeof(Raticate.Raticate);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 30; public override int PhysicalDamage => 56; public override int PhysicalDefence => 35; public override int SpecialDamage => 25; public override int SpecialDefence => 35; public override int Speed => 72;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}