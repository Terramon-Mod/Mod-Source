using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Aerodactyl
{
    public class Aerodactyl : ParentPokemonFlying
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Rock, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.Slow;public override int MaxHP => 80; public override int PhysicalDamage => 105; public override int PhysicalDefence => 65; public override int SpecialDamage => 60; public override int SpecialDefence => 75; public override int Speed => 130;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}