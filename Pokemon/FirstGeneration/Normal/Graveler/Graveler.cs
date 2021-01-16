using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Graveler
{
    public class Graveler : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.LinkCable;

        public override Type EvolveTo => typeof(Golem.Golem);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Rock, PokemonType.Ground };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 55; public override int PhysicalDamage => 95; public override int PhysicalDefence => 115; public override int SpecialDamage => 45; public override int SpecialDefence => 45; public override int Speed => 35;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}