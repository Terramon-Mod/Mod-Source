using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Dragonite
{
    public class Dragonite : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Dragon, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 91; public override int PhysicalDamage => 134; public override int PhysicalDefence => 95; public override int SpecialDamage => 100; public override int SpecialDefence => 100; public override int Speed => 80;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}