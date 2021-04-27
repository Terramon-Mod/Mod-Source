using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Pikachu
{
    public class Pikachu : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.ThunderStone;

        public override Type EvolveTo => typeof(Raichu.Raichu);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Electric };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 35; public override int PhysicalDamage => 55; public override int PhysicalDefence => 40; public override int SpecialDamage => 50; public override int SpecialDefence => 50; public override int Speed => 90;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}