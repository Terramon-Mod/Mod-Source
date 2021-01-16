using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Nidoking
{
    public class Nidoking : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Poison, PokemonType.Ground };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 81; public override int PhysicalDamage => 102; public override int PhysicalDefence => 77; public override int SpecialDamage => 85; public override int SpecialDefence => 75; public override int Speed => 85;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}