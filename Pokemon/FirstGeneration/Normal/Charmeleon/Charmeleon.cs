using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Charmeleon
{
    public class Charmeleon : ParentPokemon
    {
        public override int EvolveCost => 20;

        public override Type EvolveTo => typeof(Charizard.Charizard);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fire };

        public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 58; public override int PhysicalDamage => 64; public override int PhysicalDefence => 58; public override int SpecialDamage => 80; public override int SpecialDefence => 65; public override int Speed => 80;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}