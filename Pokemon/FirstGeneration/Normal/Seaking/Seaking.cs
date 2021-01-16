using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Seaking
{
    public class Seaking : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 80; public override int PhysicalDamage => 92; public override int PhysicalDefence => 65; public override int SpecialDamage => 65; public override int SpecialDefence => 80; public override int Speed => 68;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}