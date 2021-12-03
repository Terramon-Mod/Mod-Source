using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Exeggcute
{
    public class Exeggcute : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.LeafStone;

        public override Type EvolveTo => typeof(Exeggutor.Exeggutor);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Grass, PokemonType.Psychic };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 60; public override int PhysicalDamage => 40; public override int PhysicalDefence => 80; public override int SpecialDamage => 60; public override int SpecialDefence => 45; public override int Speed => 40;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}