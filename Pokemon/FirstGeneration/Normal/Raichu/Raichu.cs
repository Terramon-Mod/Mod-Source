using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Raichu
{
    public class Raichu : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Electric };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 60; public override int PhysicalDamage => 90; public override int PhysicalDefence => 55; public override int SpecialDamage => 90; public override int SpecialDefence => 80; public override int Speed => 110;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}