using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Machamp
{
    public class Machamp : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fighting };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 90; public override int PhysicalDamage => 130; public override int PhysicalDefence => 80; public override int SpecialDamage => 65; public override int SpecialDefence => 85; public override int Speed => 55;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}