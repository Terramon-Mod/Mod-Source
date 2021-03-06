using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Dugtrio
{
    public class Dugtrio : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ground };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 35; public override int PhysicalDamage => 100; public override int PhysicalDefence => 50; public override int SpecialDamage => 50; public override int SpecialDefence => 70; public override int Speed => 120;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}