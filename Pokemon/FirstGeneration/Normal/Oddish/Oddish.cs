using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Oddish
{
    public class Oddish : ParentPokemon
    {
        public override int EvolveCost => 16;

        public override Type EvolveTo => typeof(Gloom.Gloom);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Grass, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 45; public override int PhysicalDamage => 50; public override int PhysicalDefence => 55; public override int SpecialDamage => 75; public override int SpecialDefence => 65; public override int Speed => 30;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}