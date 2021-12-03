using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Alakazam
{
    public class Alakazam : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Psychic };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 55; public override int PhysicalDamage => 50; public override int PhysicalDefence => 45; public override int SpecialDamage => 135; public override int SpecialDefence => 95; public override int Speed => 120;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}