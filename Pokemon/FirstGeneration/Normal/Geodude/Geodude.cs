using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Geodude
{
    public class Geodude : ParentPokemon
    {
        public override int EvolveCost => 20;

        public override Type EvolveTo => typeof(Graveler.Graveler);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Rock, PokemonType.Ground };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 40; public override int PhysicalDamage => 80; public override int PhysicalDefence => 100; public override int SpecialDamage => 30; public override int SpecialDefence => 30; public override int Speed => 20;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}