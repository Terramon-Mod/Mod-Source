using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Fearow
{
    public class Fearow : ParentPokemonFlying
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 65; public override int PhysicalDamage => 90; public override int PhysicalDefence => 65; public override int SpecialDamage => 61; public override int SpecialDefence => 61; public override int Speed => 100;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}