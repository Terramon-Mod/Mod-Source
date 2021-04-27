using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Cloyster
{
    public class Cloyster : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water, PokemonType.Ice };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 50; public override int PhysicalDamage => 95; public override int PhysicalDefence => 180; public override int SpecialDamage => 85; public override int SpecialDefence => 45; public override int Speed => 70;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}