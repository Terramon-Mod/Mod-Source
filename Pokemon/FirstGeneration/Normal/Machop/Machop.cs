using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Machop
{
    public class Machop : ParentPokemon
    {
        public override int EvolveCost => 23;

        public override Type EvolveTo => typeof(Machoke.Machoke);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fighting };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 70; public override int PhysicalDamage => 80; public override int PhysicalDefence => 50; public override int SpecialDamage => 35; public override int SpecialDefence => 35; public override int Speed => 35;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}