using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Slowpoke
{
    public class Slowpoke : ParentPokemon
    {
        public override int EvolveCost => 32;

        public override Type EvolveTo => typeof(Slowbro.Slowbro);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water, PokemonType.Psychic };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 90; public override int PhysicalDamage => 65; public override int PhysicalDefence => 65; public override int SpecialDamage => 40; public override int SpecialDefence => 40; public override int Speed => 15;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}