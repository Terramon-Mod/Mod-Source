using System;
using static Terramon.Pokemon.ExpGroups;

namespace Terramon.Pokemon.FirstGeneration.Normal.Ponyta
{
    public class Ponyta : ParentPokemon
    {
        public override int EvolveCost => 35;

        public override Type EvolveTo => typeof(Rapidash.Rapidash);

        public override PokemonType[] PokemonTypes => new[] { PokemonType.Fire };

        public override ExpGroup ExpGroup => ExpGroup.MediumFast;public override int MaxHP => 50; public override int PhysicalDamage => 85; public override int PhysicalDefence => 55; public override int SpecialDamage => 65; public override int SpecialDefence => 65; public override int Speed => 90;

        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    }
}