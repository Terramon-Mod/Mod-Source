using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Ditto
{
    public class Ditto : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 48; public override int PhysicalDamage => 48; public override int PhysicalDefence => 48; public override int SpecialDamage => 48; public override int SpecialDefence => 48; public override int Speed => 48;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}