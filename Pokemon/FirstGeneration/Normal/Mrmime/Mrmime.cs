using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Mrmime
{
    public class Mrmime : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Psychic, PokemonType.Fairy };

	public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 40; public override int PhysicalDamage => 45; public override int PhysicalDefence => 65; public override int SpecialDamage => 100; public override int SpecialDefence => 120; public override int Speed => 90;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}