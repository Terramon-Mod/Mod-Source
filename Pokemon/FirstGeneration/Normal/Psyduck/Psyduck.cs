using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Psyduck
{
    public class Psyduck : ParentPokemon
    {
        public override int EvolveCost => 28;

        public override Type EvolveTo => typeof(Golduck.Golduck);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 50; public override int PhysicalDamage => 52; public override int PhysicalDefence => 48; public override int SpecialDamage => 65; public override int SpecialDefence => 50; public override int Speed => 55;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}