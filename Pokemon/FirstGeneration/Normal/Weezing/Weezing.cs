using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Weezing
{
    public class Weezing : ParentPokemonGastly
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 65; public override int PhysicalDamage => 90; public override int PhysicalDefence => 120; public override int SpecialDamage => 85; public override int SpecialDefence => 70; public override int Speed => 60;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}