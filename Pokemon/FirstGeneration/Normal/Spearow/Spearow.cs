using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Spearow
{
    public class Spearow : ParentPokemon
    {
        public override int EvolveCost => 15;

        public override Type EvolveTo => typeof(Fearow.Fearow);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 40; public override int PhysicalDamage => 60; public override int PhysicalDefence => 30; public override int SpecialDamage => 31; public override int SpecialDefence => 31; public override int Speed => 70;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}