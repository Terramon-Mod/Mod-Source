using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Blastoise
{
    public class Blastoise : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 79; public override int PhysicalDamage => 83; public override int PhysicalDefence => 100; public override int SpecialDamage => 85; public override int SpecialDefence => 105; public override int Speed => 78;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}