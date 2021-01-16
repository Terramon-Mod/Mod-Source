using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Cubone
{
    public class Cubone : ParentPokemon
    {
        public override int EvolveCost => 23;

        public override Type EvolveTo => typeof(Marowak.Marowak);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ground };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 50; public override int PhysicalDamage => 50; public override int PhysicalDefence => 95; public override int SpecialDamage => 40; public override int SpecialDefence => 50; public override int Speed => 35;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}