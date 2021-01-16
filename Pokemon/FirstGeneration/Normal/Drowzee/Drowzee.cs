using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Drowzee
{
    public class Drowzee : ParentPokemon
    {
        public override int EvolveCost => 21;

        public override Type EvolveTo => typeof(Hypno.Hypno);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Psychic };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 60; public override int PhysicalDamage => 48; public override int PhysicalDefence => 45; public override int SpecialDamage => 43; public override int SpecialDefence => 90; public override int Speed => 42;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}