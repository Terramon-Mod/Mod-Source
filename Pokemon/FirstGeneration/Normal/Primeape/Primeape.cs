using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Primeape
{
    public class Primeape : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fighting };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 65; public override int PhysicalDamage => 105; public override int PhysicalDefence => 60; public override int SpecialDamage => 60; public override int SpecialDefence => 70; public override int Speed => 95;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}