using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Growlithe
{
    public class Growlithe : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.FireStone;

        public override Type EvolveTo => typeof(Arcanine.Arcanine);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fire };

        public override ExpGroup ExpGroup => ExpGroup.Slow; public override int MaxHP => 55; public override int PhysicalDamage => 70; public override int PhysicalDefence => 45; public override int SpecialDamage => 70; public override int SpecialDefence => 50; public override int Speed => 60;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}