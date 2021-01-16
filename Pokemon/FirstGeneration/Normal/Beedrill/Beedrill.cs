using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Beedrill
{
    public class Beedrill : ParentPokemonFlying
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Bug, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 65; public override int PhysicalDamage => 90; public override int PhysicalDefence => 40; public override int SpecialDamage => 45; public override int SpecialDefence => 80; public override int Speed => 75;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}