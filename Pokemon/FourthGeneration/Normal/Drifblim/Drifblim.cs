using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FourthGeneration.Normal.Drifblim
{
    public class Drifblim : ParentPokemonFlying
    {
        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ghost, PokemonType.Flying };

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}