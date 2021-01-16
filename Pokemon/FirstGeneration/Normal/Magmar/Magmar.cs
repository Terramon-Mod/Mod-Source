using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Magmar
{
    public class Magmar : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fire };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 65; public override int PhysicalDamage => 95; public override int PhysicalDefence => 57; public override int SpecialDamage => 100; public override int SpecialDefence => 85; public override int Speed => 93;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}