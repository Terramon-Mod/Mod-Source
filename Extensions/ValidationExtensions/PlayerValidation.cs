using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terramon.Players;
using Terramon.Pokemon;
using Terraria;

namespace Terramon.Extensions.ValidationExtensions
{
    public static class PlayerValidation
    {
        public static Validator<TerramonPlayer> HasActivePetProjectile(this Validator<TerramonPlayer> validator)
        {
            var pl = validator.Value;

            if (pl.IsNull() || pl.ActivePet.IsNull() || // Null check
                pl.ActivePetId < 0 || pl.ActivePetId > Main.maxProjectiles || // OOR Exception check
                !Main.projectile[pl.ActivePetId].active || !(Main.projectile[pl.ActivePetId].modProjectile is ParentPokemon)) // Projectile and type check
                return validator.Invalidate();

            return validator;
        }

        public static Validator<TerramonPlayer> NotInBattle(this Validator<TerramonPlayer> validator) 
            => !validator.Value.Battle.IsNull() ? validator.Invalidate() : validator;

        public static Validator<TerramonPlayer> HasNotFaintedPokemons(this Validator<TerramonPlayer> validator)
        {
            var pl = validator.Value;

            if ((pl.PartySlot1.IsNull() || pl.PartySlot1.Fainted) &&// Each slot null and faint check
                (pl.PartySlot2.IsNull() || pl.PartySlot2.Fainted) &&
                (pl.PartySlot3.IsNull() || pl.PartySlot3.Fainted) &&
                (pl.PartySlot4.IsNull() || pl.PartySlot4.Fainted) &&
                (pl.PartySlot5.IsNull() || pl.PartySlot5.Fainted) &&
                (pl.PartySlot6.IsNull() || pl.PartySlot6.Fainted))
                return validator.Invalidate();

            return validator;
        }

        public static Validator<TerramonPlayer> IsLocalPlayer(this Validator<TerramonPlayer> validator)
        {
            var pl = validator.Value;

            if (pl.IsNull() || !pl.player.active || pl.player != Main.LocalPlayer)
                return validator.Invalidate();

            return validator;
        }
    }
}
