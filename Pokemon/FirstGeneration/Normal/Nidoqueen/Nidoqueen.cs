using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Nidoqueen
{
    public class Nidoqueen : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Poison, PokemonType.Ground };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 90; public override int PhysicalDamage => 92; public override int PhysicalDefence => 87; public override int SpecialDamage => 75; public override int SpecialDefence => 85; public override int Speed => 76;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}