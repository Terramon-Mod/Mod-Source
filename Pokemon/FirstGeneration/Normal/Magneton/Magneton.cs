using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Magneton
{
    public class Magneton : ParentPokemonGastly
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Electric, PokemonType.Steel };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 50; public override int PhysicalDamage => 60; public override int PhysicalDefence => 95; public override int SpecialDamage => 120; public override int SpecialDefence => 70; public override int Speed => 70;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}