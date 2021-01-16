using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FourthGeneration.Normal.Abomasnow
{
    public class Abomasnow : ParentPokemon
    {

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Grass, PokemonType.Ice };

        public override ExpGroup ExpGroup => ExpGroup.Slow;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}
