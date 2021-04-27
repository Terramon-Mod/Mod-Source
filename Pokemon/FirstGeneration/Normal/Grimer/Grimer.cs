using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Grimer
{
    public class Grimer : ParentPokemon
    {
        public override int EvolveCost => 33;

        public override Type EvolveTo => typeof(Muk.Muk);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 80; public override int PhysicalDamage => 80; public override int PhysicalDefence => 50; public override int SpecialDamage => 40; public override int SpecialDefence => 50; public override int Speed => 25;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}