using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Kabutops
{
    public class Kabutops : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Rock, PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 60; public override int PhysicalDamage => 115; public override int PhysicalDefence => 105; public override int SpecialDamage => 65; public override int SpecialDefence => 70; public override int Speed => 80;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}