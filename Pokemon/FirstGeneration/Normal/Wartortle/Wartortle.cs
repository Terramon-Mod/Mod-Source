using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Wartortle
{
    public class Wartortle : ParentPokemon
    {
        public override int EvolveCost => 20;

        public override Type EvolveTo => typeof(Blastoise.Blastoise);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 59; public override int PhysicalDamage => 63; public override int PhysicalDefence => 80; public override int SpecialDamage => 65; public override int SpecialDefence => 80; public override int Speed => 58;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}