using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Pinsir
{
    public class Pinsir : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Bug };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 65; public override int PhysicalDamage => 125; public override int PhysicalDefence => 100; public override int SpecialDamage => 55; public override int SpecialDefence => 70; public override int Speed => 85;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}