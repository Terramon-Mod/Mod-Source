using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Onix
{
    public class Onix : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Rock, PokemonType.Ground };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 35; public override int PhysicalDamage => 45; public override int PhysicalDefence => 160; public override int SpecialDamage => 30; public override int SpecialDefence => 45; public override int Speed => 70;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}