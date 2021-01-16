using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Porygon
{
    public class Porygon : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 65; public override int PhysicalDamage => 60; public override int PhysicalDefence => 70; public override int SpecialDamage => 85; public override int SpecialDefence => 75; public override int Speed => 40;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}