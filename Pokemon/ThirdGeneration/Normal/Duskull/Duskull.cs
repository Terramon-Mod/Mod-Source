using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.ThirdGeneration.Normal.Duskull
{
    public class Duskull : ParentPokemonGastly
    {
        public override int EvolveCost => 32;

        public override Type EvolveTo => typeof(Dusclops.Dusclops);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ghost };

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}