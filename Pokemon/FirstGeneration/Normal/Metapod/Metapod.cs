using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Metapod
{
    public class Metapod : ParentPokemon
    {
        public override int EvolveCost => 3;

        public override Type EvolveTo => typeof(Butterfree.Butterfree);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Bug };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 50; public override int PhysicalDamage => 20; public override int PhysicalDefence => 55; public override int SpecialDamage => 25; public override int SpecialDefence => 25; public override int Speed => 30;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}