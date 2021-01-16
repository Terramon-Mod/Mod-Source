using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Flareon
{
    public class Flareon : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fire };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 65; public override int PhysicalDamage => 130; public override int PhysicalDefence => 60; public override int SpecialDamage => 95; public override int SpecialDefence => 110; public override int Speed => 65;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}