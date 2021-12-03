using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Scyther
{
    public class Scyther : ParentPokemonFlying
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Bug, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 70; public override int PhysicalDamage => 110; public override int PhysicalDefence => 80; public override int SpecialDamage => 55; public override int SpecialDefence => 80; public override int Speed => 105;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}