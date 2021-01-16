using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Nidoranm
{
    public class Nidoranm : ParentPokemon
    {
        public override int EvolveCost => 11;

        public override Type EvolveTo => typeof(Nidorino.Nidorino);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Poison };

	public override ExpGroup ExpGroup => ExpGroup.MediumSlow;public override int MaxHP => 46; public override int PhysicalDamage => 57; public override int PhysicalDefence => 40; public override int SpecialDamage => 40; public override int SpecialDefence => 40; public override int Speed => 50;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}