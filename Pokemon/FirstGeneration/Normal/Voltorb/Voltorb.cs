using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Voltorb
{
    public class Voltorb : ParentPokemon
    {
        public override int EvolveCost => 25;

        public override Type EvolveTo => typeof(Electrode.Electrode);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Electric };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 40; public override int PhysicalDamage => 30; public override int PhysicalDefence => 50; public override int SpecialDamage => 55; public override int SpecialDefence => 55; public override int Speed => 100;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}