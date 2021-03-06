using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Victreebel
{
    public class Victreebel : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Grass, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 80; public override int PhysicalDamage => 105; public override int PhysicalDefence => 65; public override int SpecialDamage => 100; public override int SpecialDefence => 70; public override int Speed => 70;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}