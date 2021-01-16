using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Pidgeotto
{
    public class Pidgeotto : ParentPokemonFlying
    {
        public override int EvolveCost => 18;

        public override Type EvolveTo => typeof(Pidgeot.Pidgeot);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 63; public override int PhysicalDamage => 60; public override int PhysicalDefence => 55; public override int SpecialDamage => 50; public override int SpecialDefence => 50; public override int Speed => 71;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}