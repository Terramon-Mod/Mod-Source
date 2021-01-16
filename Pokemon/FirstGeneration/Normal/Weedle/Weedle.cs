using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Weedle
{
    public class Weedle : ParentPokemon
    {
        public override int EvolveCost => 2;

        public override Type EvolveTo => typeof(Kakuna.Kakuna);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Bug, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 40; public override int PhysicalDamage => 35; public override int PhysicalDefence => 30; public override int SpecialDamage => 20; public override int SpecialDefence => 20; public override int Speed => 50;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}