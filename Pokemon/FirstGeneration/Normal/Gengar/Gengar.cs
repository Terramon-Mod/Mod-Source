using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Gengar
{
    public class Gengar : ParentPokemon
    {

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ghost, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 60; public override int PhysicalDamage => 65; public override int PhysicalDefence => 60; public override int SpecialDamage => 130; public override int SpecialDefence => 75; public override int Speed => 110;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}