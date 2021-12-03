using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using Terramon.Pokemon.FourthGeneration.Normal.Dusknoir;

namespace Terramon.Pokemon.ThirdGeneration.Normal.Dusclops
{
    public class Dusclops : ParentPokemon
    {
        public override int EvolveCost => 40;

        public override Type EvolveTo => typeof(Dusknoir);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ghost };

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}