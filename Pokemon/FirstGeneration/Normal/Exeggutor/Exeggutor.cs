using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Exeggutor
{
    public class Exeggutor : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Grass, PokemonType.Psychic };

        public override ExpGroup ExpGroup => ExpGroup.Slow;public override int MaxHP => 95; public override int PhysicalDamage => 95; public override int PhysicalDefence => 85; public override int SpecialDamage => 125; public override int SpecialDefence => 75; public override int Speed => 55;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}