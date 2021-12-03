using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Snorlax
{
    public class Snorlax : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 160; public override int PhysicalDamage => 110; public override int PhysicalDefence => 65; public override int SpecialDamage => 65; public override int SpecialDefence => 110; public override int Speed => 30;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}