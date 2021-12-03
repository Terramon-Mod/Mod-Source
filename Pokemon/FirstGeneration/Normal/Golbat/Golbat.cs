using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Golbat
{
    public class Golbat : ParentPokemonFlying
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Poison, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 75; public override int PhysicalDamage => 80; public override int PhysicalDefence => 70; public override int SpecialDamage => 65; public override int SpecialDefence => 75; public override int Speed => 90;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}