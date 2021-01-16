using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Caterpie
{
    public class Caterpie : ParentPokemon
    {
        public override int EvolveCost => 2;

        public override Type EvolveTo => typeof(Metapod.Metapod);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Bug };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 45; public override int PhysicalDamage => 30; public override int PhysicalDefence => 35; public override int SpecialDamage => 20; public override int SpecialDefence => 20; public override int Speed => 45;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}