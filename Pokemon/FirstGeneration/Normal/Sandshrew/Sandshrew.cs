using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Sandshrew
{
    public class Sandshrew : ParentPokemon
    {
        public override int EvolveCost => 17;

        public override Type EvolveTo => typeof(Sandslash.Sandslash);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ground };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 50; public override int PhysicalDamage => 75; public override int PhysicalDefence => 85; public override int SpecialDamage => 20; public override int SpecialDefence => 30; public override int Speed => 40;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}