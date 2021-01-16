using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Bellsprout
{
    public class Bellsprout : ParentPokemon
    {
        public override int EvolveCost => 16;

        public override Type EvolveTo => typeof(Weepinbell.Weepinbell);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Grass, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 50; public override int PhysicalDamage => 75; public override int PhysicalDefence => 35; public override int SpecialDamage => 70; public override int SpecialDefence => 30; public override int Speed => 40;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}