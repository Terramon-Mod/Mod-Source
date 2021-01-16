using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Omanyte
{
    public class Omanyte : ParentPokemon
    {
        public override int EvolveCost => 35;

        public override Type EvolveTo => typeof(Omastar.Omastar);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Rock, PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 35; public override int PhysicalDamage => 40; public override int PhysicalDefence => 100; public override int SpecialDamage => 90; public override int SpecialDefence => 55; public override int Speed => 35;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}