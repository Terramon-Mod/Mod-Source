using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Marowak
{
    public class Marowak : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ground };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 60; public override int PhysicalDamage => 80; public override int PhysicalDefence => 110; public override int SpecialDamage => 50; public override int SpecialDefence => 80; public override int Speed => 45;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}