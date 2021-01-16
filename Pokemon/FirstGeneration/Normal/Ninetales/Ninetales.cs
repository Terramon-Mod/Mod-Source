using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Ninetales
{
    public class Ninetales : ParentPokemon
    {

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fire };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 73; public override int PhysicalDamage => 76; public override int PhysicalDefence => 75; public override int SpecialDamage => 81; public override int SpecialDefence => 100; public override int Speed => 100;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}
