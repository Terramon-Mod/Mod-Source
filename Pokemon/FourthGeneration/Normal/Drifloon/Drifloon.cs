using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FourthGeneration.Normal.Drifloon
{
    public class Drifloon : ParentPokemonFlying
    {
        public override int EvolveCost => 23;

        public override Type EvolveTo => typeof(Drifblim.Drifblim);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ghost, PokemonType.Flying };

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}