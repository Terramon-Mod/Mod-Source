using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Paras
{
    public class Paras : ParentPokemon
    {
        public override int EvolveCost => 19;

        public override Type EvolveTo => typeof(Parasect.Parasect);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Bug, PokemonType.Grass };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast; public override int MaxHP => 35; public override int PhysicalDamage => 70; public override int PhysicalDefence => 55; public override int SpecialDamage => 45; public override int SpecialDefence => 55; public override int Speed => 25;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}