using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Hitmonlee
{
    public class Hitmonlee : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fighting };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 50; public override int PhysicalDamage => 120; public override int PhysicalDefence => 53; public override int SpecialDamage => 35; public override int SpecialDefence => 110; public override int Speed => 87;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}