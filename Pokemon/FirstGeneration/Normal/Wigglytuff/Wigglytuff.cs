using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Wigglytuff
{
    public class Wigglytuff : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal, PokemonType.Fairy };

        public override ExpGroup ExpGroup => ExpGroup.Fast; public override int MaxHP => 140; public override int PhysicalDamage => 70; public override int PhysicalDefence => 45; public override int SpecialDamage => 85; public override int SpecialDefence => 50; public override int Speed => 45;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}