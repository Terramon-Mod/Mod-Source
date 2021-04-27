using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Meowth
{
    public class Meowth : ParentPokemon
    {
        public override int EvolveCost => 23;

        public override Type EvolveTo => typeof(Persian.Persian);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 40; public override int PhysicalDamage => 45; public override int PhysicalDefence => 35; public override int SpecialDamage => 40; public override int SpecialDefence => 40; public override int Speed => 90;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}