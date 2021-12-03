using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Pidgeot
{
    public class Pidgeot : ParentPokemonFlying
    {
#if DEBUG
        public override string[] DefaultMove => new[] { nameof(ShootMove), nameof(HealMove), null, null };
#endif

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Normal, PokemonType.Flying };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow; public override int MaxHP => 83; public override int PhysicalDamage => 80; public override int PhysicalDefence => 75; public override int SpecialDamage => 70; public override int SpecialDefence => 70; public override int Speed => 101;

        public override void SetDefaults()
        {
            base.SetDefaults();



        }
    }
}