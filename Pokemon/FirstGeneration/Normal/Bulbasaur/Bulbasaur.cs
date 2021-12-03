using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Bulbasaur
{
    public class Bulbasaur : ParentPokemon
    {
        public override int EvolveCost => 11;

        public override Type EvolveTo => typeof(Ivysaur.Ivysaur);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Grass, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 45; public override int PhysicalDamage => 49; public override int PhysicalDefence => 49; public override int SpecialDamage => 65; public override int SpecialDefence => 65; public override int Speed => 45;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}