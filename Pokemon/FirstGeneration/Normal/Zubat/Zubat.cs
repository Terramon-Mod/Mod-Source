using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Zubat
{
    public class Zubat : ParentPokemonFlying
    {
        public override int EvolveCost => 17;

        public override Type EvolveTo => typeof(Golbat.Golbat);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Poison, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 40; public override int PhysicalDamage => 45; public override int PhysicalDefence => 35; public override int SpecialDamage => 30; public override int SpecialDefence => 40; public override int Speed => 55;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}