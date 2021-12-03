using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Dragonair
{
    public class Dragonair : ParentPokemon
    {
        public override int EvolveCost => 15;

        public override Type EvolveTo => typeof(Dragonite.Dragonite);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Dragon };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 61; public override int PhysicalDamage => 84; public override int PhysicalDefence => 65; public override int SpecialDamage => 70; public override int SpecialDefence => 70; public override int Speed => 70;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}