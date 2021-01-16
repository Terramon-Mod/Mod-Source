using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Jynx
{
    public class Jynx : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ice, PokemonType.Psychic };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 65; public override int PhysicalDamage => 50; public override int PhysicalDefence => 35; public override int SpecialDamage => 115; public override int SpecialDefence => 95; public override int Speed => 95;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}