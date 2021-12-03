using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using Terramon.Pokemon.FourthGeneration.Normal.Honchkrow;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.SecondGeneration.Normal.Stantler
{
    public class Stantler : ParentPokemon
    {

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal };

        public override ExpGroup ExpGroup => ExpGroup.Slow;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}
