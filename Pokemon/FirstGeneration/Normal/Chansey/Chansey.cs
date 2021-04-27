using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Chansey
{
    public class Chansey : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal };

        public override ExpGroup ExpGroup => ExpGroup.Fast; public override int MaxHP => 250; public override int PhysicalDamage => 5; public override int PhysicalDefence => 5; public override int SpecialDamage => 35; public override int SpecialDefence => 105; public override int Speed => 50;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}