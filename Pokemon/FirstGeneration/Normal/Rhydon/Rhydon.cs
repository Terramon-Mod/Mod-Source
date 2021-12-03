using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Rhydon
{
    public class Rhydon : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ground, PokemonType.Rock };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 105; public override int PhysicalDamage => 130; public override int PhysicalDefence => 120; public override int SpecialDamage => 45; public override int SpecialDefence => 45; public override int Speed => 40;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}