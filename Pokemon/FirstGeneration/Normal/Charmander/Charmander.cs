using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Charmander
{
    public class Charmander : ParentPokemon
    {
        public override int EvolveCost => 11;

        public override Type EvolveTo => typeof(Charmeleon.Charmeleon);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fire };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 39; public override int PhysicalDamage => 52; public override int PhysicalDefence => 43; public override int SpecialDamage => 60; public override int SpecialDefence => 50; public override int Speed => 65;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}