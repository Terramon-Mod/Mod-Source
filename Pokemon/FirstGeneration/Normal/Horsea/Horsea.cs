using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Horsea
{
    public class Horsea : ParentPokemon
    {
        public override int EvolveCost => 27;

        public override Type EvolveTo => typeof(Seadra.Seadra);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 30; public override int PhysicalDamage => 40; public override int PhysicalDefence => 70; public override int SpecialDamage => 70; public override int SpecialDefence => 25; public override int Speed => 60;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}