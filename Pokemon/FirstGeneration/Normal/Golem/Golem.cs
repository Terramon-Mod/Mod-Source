using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Golem
{
    public class Golem : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Rock, PokemonType.Ground };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 80; public override int PhysicalDamage => 120; public override int PhysicalDefence => 130; public override int SpecialDamage => 55; public override int SpecialDefence => 65; public override int Speed => 45;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}