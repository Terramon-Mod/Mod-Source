using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Magnemite
{
    public class Magnemite : ParentPokemonGastly
    {
        public override int EvolveCost => 25;

        public override Type EvolveTo => typeof(Magneton.Magneton);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Electric, PokemonType.Steel };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 25; public override int PhysicalDamage => 35; public override int PhysicalDefence => 70; public override int SpecialDamage => 95; public override int SpecialDefence => 55; public override int Speed => 45;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}