using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Dodrio
{
    public class Dodrio : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 60; public override int PhysicalDamage => 110; public override int PhysicalDefence => 70; public override int SpecialDamage => 60; public override int SpecialDefence => 60; public override int Speed => 110;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}