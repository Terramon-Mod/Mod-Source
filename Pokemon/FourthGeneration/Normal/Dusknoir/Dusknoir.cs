using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FourthGeneration.Normal.Dusknoir
{
    public class Dusknoir : ParentPokemon
    {

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ghost };

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}