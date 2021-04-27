using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Golduck
{
    public class Golduck : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 80; public override int PhysicalDamage => 82; public override int PhysicalDefence => 78; public override int SpecialDamage => 95; public override int SpecialDefence => 80; public override int Speed => 85;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}