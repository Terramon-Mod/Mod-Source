using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Venomoth
{
    public class Venomoth : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Bug, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 70; public override int PhysicalDamage => 65; public override int PhysicalDefence => 60; public override int SpecialDamage => 90; public override int SpecialDefence => 75; public override int Speed => 90;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}