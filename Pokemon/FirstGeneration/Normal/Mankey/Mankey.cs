using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Mankey
{
    public class Mankey : ParentPokemon
    {
        public override int EvolveCost => 23;

        public override Type EvolveTo => typeof(Primeape.Primeape);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fighting };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 40; public override int PhysicalDamage => 80; public override int PhysicalDefence => 35; public override int SpecialDamage => 35; public override int SpecialDefence => 45; public override int Speed => 70;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}