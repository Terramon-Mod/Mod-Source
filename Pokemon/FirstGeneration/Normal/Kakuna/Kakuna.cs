using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Kakuna
{
    public class Kakuna : ParentPokemon
    {
        public override int EvolveCost => 3;

        public override Type EvolveTo => typeof(Beedrill.Beedrill);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Bug, PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 45; public override int PhysicalDamage => 25; public override int PhysicalDefence => 50; public override int SpecialDamage => 25; public override int SpecialDefence => 25; public override int Speed => 35;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}