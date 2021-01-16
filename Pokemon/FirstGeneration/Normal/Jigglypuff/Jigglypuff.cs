using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Jigglypuff
{
    public class Jigglypuff : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.MoonStone;

        public override Type EvolveTo => typeof(Wigglytuff.Wigglytuff);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal, PokemonType.Fairy };

        public override ExpGroup ExpGroup => ExpGroup.Fast;public override int MaxHP => 115; public override int PhysicalDamage => 45; public override int PhysicalDefence => 20; public override int SpecialDamage => 45; public override int SpecialDefence => 25; public override int Speed => 20;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}