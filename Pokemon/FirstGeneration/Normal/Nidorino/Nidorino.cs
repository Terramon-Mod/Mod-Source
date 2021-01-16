using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Nidorino
{
    public class Nidorino : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.MoonStone;

        public override Type EvolveTo => typeof(Nidoking.Nidoking);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Poison };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 61; public override int PhysicalDamage => 72; public override int PhysicalDefence => 57; public override int SpecialDamage => 55; public override int SpecialDefence => 55; public override int Speed => 65;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}