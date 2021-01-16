using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.ThirdGeneration.Normal.Snorunt
{
    public class Snorunt : ParentPokemon
    {
        public override int EvolveCost => 37;

        public override Type EvolveTo => typeof(Glalie.Glalie);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Ice };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}
