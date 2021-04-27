using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Hypno
{
    public class Hypno : ParentPokemon
    {

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Psychic };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 85; public override int PhysicalDamage => 73; public override int PhysicalDefence => 70; public override int SpecialDamage => 73; public override int SpecialDefence => 115; public override int Speed => 67;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}