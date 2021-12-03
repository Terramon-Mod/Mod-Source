using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Poliwrath
{
    public class Poliwrath : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water, PokemonType.Fighting };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 90; public override int PhysicalDamage => 95; public override int PhysicalDefence => 95; public override int SpecialDamage => 70; public override int SpecialDefence => 90; public override int Speed => 70;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}