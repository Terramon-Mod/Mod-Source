using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Kadabra
{
    public class Kadabra : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.LinkCable;

        public override Type EvolveTo => typeof(Alakazam.Alakazam);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Psychic };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 40; public override int PhysicalDamage => 35; public override int PhysicalDefence => 30; public override int SpecialDamage => 120; public override int SpecialDefence => 70; public override int Speed => 105;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}