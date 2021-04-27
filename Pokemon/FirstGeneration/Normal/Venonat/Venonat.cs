using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Venonat
{
    public class Venonat : ParentPokemon
    {
        public override int EvolveCost => 26;

        public override Type EvolveTo => typeof(Venomoth.Venomoth);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Bug, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 60; public override int PhysicalDamage => 55; public override int PhysicalDefence => 50; public override int SpecialDamage => 40; public override int SpecialDefence => 55; public override int Speed => 45;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}