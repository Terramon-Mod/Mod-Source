using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Parasect
{
    public class Parasect : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Bug, PokemonType.Grass };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 60; public override int PhysicalDamage => 95; public override int PhysicalDefence => 80; public override int SpecialDamage => 60; public override int SpecialDefence => 80; public override int Speed => 30;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}