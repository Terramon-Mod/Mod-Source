using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Charizard
{
    public class Charizard : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fire, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 78; public override int PhysicalDamage => 84; public override int PhysicalDefence => 78; public override int SpecialDamage => 109; public override int SpecialDefence => 85; public override int Speed => 100;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}
