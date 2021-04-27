using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Muk
{
    public class Muk : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 105; public override int PhysicalDamage => 105; public override int PhysicalDefence => 75; public override int SpecialDamage => 65; public override int SpecialDefence => 100; public override int Speed => 50;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}