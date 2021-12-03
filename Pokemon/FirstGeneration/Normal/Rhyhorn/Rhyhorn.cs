using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Rhyhorn
{
    public class Rhyhorn : ParentPokemon
    {
        public override int EvolveCost => 37;

        public override Type EvolveTo => typeof(Rhydon.Rhydon);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ground, PokemonType.Rock };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 80; public override int PhysicalDamage => 85; public override int PhysicalDefence => 95; public override int SpecialDamage => 30; public override int SpecialDefence => 30; public override int Speed => 25;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}