using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Doduo
{
    public class Doduo : ParentPokemon
    {
        public override int EvolveCost => 26;

        public override Type EvolveTo => typeof(Dodrio.Dodrio);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 35; public override int PhysicalDamage => 85; public override int PhysicalDefence => 45; public override int SpecialDamage => 35; public override int SpecialDefence => 35; public override int Speed => 75;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}