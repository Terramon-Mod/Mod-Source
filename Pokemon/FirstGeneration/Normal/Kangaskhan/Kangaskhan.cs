using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Kangaskhan
{
    public class Kangaskhan : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 105; public override int PhysicalDamage => 95; public override int PhysicalDefence => 80; public override int SpecialDamage => 40; public override int SpecialDefence => 80; public override int Speed => 90;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}