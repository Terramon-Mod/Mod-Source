using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Haunter
{
    public class Haunter : ParentPokemonGastly
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.LinkCable;

        public override Type EvolveTo => typeof(Gengar.Gengar);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ghost, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 45; public override int PhysicalDamage => 50; public override int PhysicalDefence => 45; public override int SpecialDamage => 115; public override int SpecialDefence => 55; public override int Speed => 95;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}