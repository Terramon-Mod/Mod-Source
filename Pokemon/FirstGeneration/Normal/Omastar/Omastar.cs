using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Omastar
{
    public class Omastar : ParentPokemon
    {




        public override PokemonType[] PokemonTypes => new[] { PokemonType.Rock, PokemonType.Water };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 70; public override int PhysicalDamage => 60; public override int PhysicalDefence => 125; public override int SpecialDamage => 115; public override int SpecialDefence => 70; public override int Speed => 55;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}