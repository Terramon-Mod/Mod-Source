using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terramon.Players;
using Terramon.Pokemon.Moves;
using Terraria;
using Terraria.ModLoader;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Vulpix
{
    public class Vulpix : ParentPokemon
    {
        public override int EvolveCost => 1;

        public override EvolveItem EvolveItem => EvolveItem.FireStone;
        
        public override Type EvolveTo => typeof(Ninetales.Ninetales);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fire };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 38; public override int PhysicalDamage => 41; public override int PhysicalDefence => 40; public override int SpecialDamage => 50; public override int SpecialDefence => 65; public override int Speed => 65;

        public override void SetDefaults()
        {
            base.SetDefaults();

            
            
        }
    }
}
