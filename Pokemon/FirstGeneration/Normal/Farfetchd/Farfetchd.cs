using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Farfetchd
{
    public class Farfetchd : ParentPokemon
    {
        

        

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 52; public override int PhysicalDamage => 90; public override int PhysicalDefence => 55; public override int SpecialDamage => 58; public override int SpecialDefence => 62; public override int Speed => 60;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}