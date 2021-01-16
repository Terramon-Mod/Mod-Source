using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Diglett
{
    public class Diglett : ParentPokemon
    {
        public override int EvolveCost => 21;

        public override Type EvolveTo => typeof(Dugtrio.Dugtrio);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ground };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 10; public override int PhysicalDamage => 55; public override int PhysicalDefence => 25; public override int SpecialDamage => 35; public override int SpecialDefence => 45; public override int Speed => 95;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}