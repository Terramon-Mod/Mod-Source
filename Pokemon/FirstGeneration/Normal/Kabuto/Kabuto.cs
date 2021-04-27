using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Kabuto
{
    public class Kabuto : ParentPokemon
    {
        public override int EvolveCost => 35;

        public override Type EvolveTo => typeof(Kabutops.Kabutops);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Rock, PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 30; public override int PhysicalDamage => 80; public override int PhysicalDefence => 90; public override int SpecialDamage => 55; public override int SpecialDefence => 45; public override int Speed => 55;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}