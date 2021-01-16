using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FourthGeneration.Normal.Honchkrow
{
    public class Honchkrow : ParentPokemon
    {
        public override PokemonType[] PokemonTypes => new[] { PokemonType.Dark, PokemonType.Flying };

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}