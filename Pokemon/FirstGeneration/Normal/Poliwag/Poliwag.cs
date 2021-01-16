using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Poliwag
{
    public class Poliwag : ParentPokemon
    {
        public override int EvolveCost => 20;

        public override Type EvolveTo => typeof(Poliwhirl.Poliwhirl);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 40; public override int PhysicalDamage => 50; public override int PhysicalDefence => 40; public override int SpecialDamage => 40; public override int SpecialDefence => 40; public override int Speed => 90;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}