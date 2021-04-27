using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Machoke
{
    public class Machoke : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.LinkCable;

        public override Type EvolveTo => typeof(Machamp.Machamp);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fighting };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 80; public override int PhysicalDamage => 100; public override int PhysicalDefence => 70; public override int SpecialDamage => 50; public override int SpecialDefence => 60; public override int Speed => 45;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}
