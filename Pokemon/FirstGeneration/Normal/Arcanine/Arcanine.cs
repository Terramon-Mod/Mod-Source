using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Arcanine
{
    public class Arcanine : ParentPokemon
    {
        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fire };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 90; public override int PhysicalDamage => 110; public override int PhysicalDefence => 80; public override int SpecialDamage => 100; public override int SpecialDefence => 80; public override int Speed => 95;

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }
}