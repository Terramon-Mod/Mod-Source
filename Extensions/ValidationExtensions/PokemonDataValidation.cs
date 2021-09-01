using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terramon.Pokemon;

namespace Terramon.Extensions.ValidationExtensions
{
    public static class PokemonDataValidation
    {
        public static Validator<PokemonData> NotFainted(this Validator<PokemonData> validator)
        {
            var pd = validator.Value;

            if (pd.IsNull() || pd.Fainted || pd.HP == 0 || string.IsNullOrEmpty(pd.pokemon))
                return validator.Invalidate();

            return validator;
        }

        public static Validator<PokemonData> HasAvailableMoves(this Validator<PokemonData> validator)
        {
            var pd = validator.Value;

            if (pd.IsNull() || pd.Moves.IsNull() || pd.Moves.Length != 4 ) // Null and size check
                return validator.Invalidate();

            var moves = pd.Moves;
            var pp = pd.MovesPP;
            var result = false;

            for (int i = 0; i < 4; i++)// Query trough moves and its pp left 
            {
                if (moves[i] != null && pp[i] != 0)
                {
                    result = true;
                    break;
                }
            }

            return result ? validator : validator.Invalidate();
        }


    }
}
