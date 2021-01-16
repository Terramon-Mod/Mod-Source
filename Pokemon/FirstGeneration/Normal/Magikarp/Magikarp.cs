using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Magikarp
{
    public class Magikarp : ParentPokemon
    {
        public override int EvolveCost => 15;

        public override Type EvolveTo => typeof(Gyarados.Gyarados);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.Slow;public override int MaxHP => 20; public override int PhysicalDamage => 10; public override int PhysicalDefence => 55; public override int SpecialDamage => 15; public override int SpecialDefence => 20; public override int Speed => 80;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}