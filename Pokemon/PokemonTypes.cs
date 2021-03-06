﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terramon.Pokemon
{
    public static class PokemonTypesExtencion
    {

        public static float GetResist(this PokemonType def, PokemonType atk) => TypeEffectivnes[def][atk];

        /// <summary>
        /// Type effectivnes table.
        /// First is defending type, second is attacking type
        /// </summary>
        public static Dictionary<PokemonType, Dictionary<PokemonType, float>> TypeEffectivnes = new Dictionary<PokemonType, Dictionary<PokemonType, float>>
        {
            [PokemonType.Normal] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 2f,
                [PokemonType.Flying] = 1f,
                [PokemonType.Poison] = 1f,
                [PokemonType.Ground] = 1f,
                [PokemonType.Rock] = 1f,
                [PokemonType.Bug] = 1f,
                [PokemonType.Ghost] = 0f,
                [PokemonType.Steel] = 1f,
                [PokemonType.Fire] = 1f,
                [PokemonType.Water] = 1f,
                [PokemonType.Grass] = 1f,
                [PokemonType.Electric] = 1f,
                [PokemonType.Psychic] = 1f,
                [PokemonType.Ice] = 1f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 1f,
                [PokemonType.Fairy] = 1f,
            },
            [PokemonType.Fighting] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 1f,
                [PokemonType.Flying] = 2f,
                [PokemonType.Poison] = 1f,
                [PokemonType.Ground] = 1f,
                [PokemonType.Rock] = 0.5f,
                [PokemonType.Bug] = 0.5f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 1f,
                [PokemonType.Fire] = 1f,
                [PokemonType.Water] = 1f,
                [PokemonType.Grass] = 1f,
                [PokemonType.Electric] = 1f,
                [PokemonType.Psychic] = 2f,
                [PokemonType.Ice] = 1f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 0.5f,
                [PokemonType.Fairy] = 2f,
            },
            [PokemonType.Flying] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 0.5f,
                [PokemonType.Flying] = 1f,
                [PokemonType.Poison] = 1f,
                [PokemonType.Ground] = 0f,
                [PokemonType.Rock] = 2f,
                [PokemonType.Bug] = 0.5f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 1f,
                [PokemonType.Fire] = 1f,
                [PokemonType.Water] = 1f,
                [PokemonType.Grass] = 0.5f,
                [PokemonType.Electric] = 2f,
                [PokemonType.Psychic] = 1f,
                [PokemonType.Ice] = 2f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 1f,
                [PokemonType.Fairy] = 1f,
            },
            [PokemonType.Poison] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 0.5f,
                [PokemonType.Flying] = 1f,
                [PokemonType.Poison] = 0.5f,
                [PokemonType.Ground] = 2f,
                [PokemonType.Rock] = 1f,
                [PokemonType.Bug] = 0.5f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 1f,
                [PokemonType.Fire] = 1f,
                [PokemonType.Water] = 1f,
                [PokemonType.Grass] = 0.5f,
                [PokemonType.Electric] = 1f,
                [PokemonType.Psychic] = 2f,
                [PokemonType.Ice] = 1f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 1f,
                [PokemonType.Fairy] = 0.5f,
            },
            [PokemonType.Ground] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 1f,
                [PokemonType.Flying] = 1f,
                [PokemonType.Poison] = 0.5f,
                [PokemonType.Ground] = 1f,
                [PokemonType.Rock] = 0.5f,
                [PokemonType.Bug] = 1f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 1f,
                [PokemonType.Fire] = 1f,
                [PokemonType.Water] = 2f,
                [PokemonType.Grass] = 2f,
                [PokemonType.Electric] = 0f,
                [PokemonType.Psychic] = 1f,
                [PokemonType.Ice] = 2f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 1f,
                [PokemonType.Fairy] = 1f,
            },
            [PokemonType.Rock] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 0.5f,
                [PokemonType.Fighting] = 2f,
                [PokemonType.Flying] = 0.5f,
                [PokemonType.Poison] = 0.5f,
                [PokemonType.Ground] = 2f,
                [PokemonType.Rock] = 1f,
                [PokemonType.Bug] = 1f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 2f,
                [PokemonType.Fire] = 0.5f,
                [PokemonType.Water] = 2f,
                [PokemonType.Grass] = 2f,
                [PokemonType.Electric] = 1f,
                [PokemonType.Psychic] = 1f,
                [PokemonType.Ice] = 1f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 1f,
                [PokemonType.Fairy] = 1f,
            },
            [PokemonType.Bug] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 0.5f,
                [PokemonType.Flying] = 2f,
                [PokemonType.Poison] = 1f,
                [PokemonType.Ground] = 0.5f,
                [PokemonType.Rock] = 2f,
                [PokemonType.Bug] = 1f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 1f,
                [PokemonType.Fire] = 2f,
                [PokemonType.Water] = 1f,
                [PokemonType.Grass] = 0.5f,
                [PokemonType.Electric] = 1f,
                [PokemonType.Psychic] = 1f,
                [PokemonType.Ice] = 1f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 1f,
                [PokemonType.Fairy] = 1f,
            },
            [PokemonType.Ghost] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 0f,
                [PokemonType.Fighting] = 0f,
                [PokemonType.Flying] = 1f,
                [PokemonType.Poison] = 0.5f,
                [PokemonType.Ground] = 1f,
                [PokemonType.Rock] = 1f,
                [PokemonType.Bug] = 0.5f,
                [PokemonType.Ghost] = 2f,
                [PokemonType.Steel] = 1f,
                [PokemonType.Fire] = 1f,
                [PokemonType.Water] = 1f,
                [PokemonType.Grass] = 1f,
                [PokemonType.Electric] = 1f,
                [PokemonType.Psychic] = 1f,
                [PokemonType.Ice] = 1f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 2f,
                [PokemonType.Fairy] = 1f,
            },
            [PokemonType.Steel] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 0.5f,
                [PokemonType.Fighting] = 2f,
                [PokemonType.Flying] = 0.5f,
                [PokemonType.Poison] = 0f,
                [PokemonType.Ground] = 2f,
                [PokemonType.Rock] = 0.5f,
                [PokemonType.Bug] = 0.5f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 0.5f,
                [PokemonType.Fire] = 2f,
                [PokemonType.Water] = 1f,
                [PokemonType.Grass] = 0.5f,
                [PokemonType.Electric] = 1f,
                [PokemonType.Psychic] = 0.5f,
                [PokemonType.Ice] = 0.5f,
                [PokemonType.Dragon] = 0.5f,
                [PokemonType.Dark] = 1f,
                [PokemonType.Fairy] = 0.5f,
            },
            [PokemonType.Fire] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 1f,
                [PokemonType.Flying] = 1f,
                [PokemonType.Poison] = 1f,
                [PokemonType.Ground] = 2f,
                [PokemonType.Rock] = 2f,
                [PokemonType.Bug] = 0.5f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 0.5f,
                [PokemonType.Fire] = 0.5f,
                [PokemonType.Water] = 2f,
                [PokemonType.Grass] = 0.5f,
                [PokemonType.Electric] = 1f,
                [PokemonType.Psychic] = 1f,
                [PokemonType.Ice] = 0.5f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 1f,
                [PokemonType.Fairy] = 1f,
            },
            [PokemonType.Water] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 1f,
                [PokemonType.Flying] = 1f,
                [PokemonType.Poison] = 1f,
                [PokemonType.Ground] = 1f,
                [PokemonType.Rock] = 1f,
                [PokemonType.Bug] = 1f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 0.5f,
                [PokemonType.Fire] = 0.5f,
                [PokemonType.Water] = 0.5f,
                [PokemonType.Grass] = 2f,
                [PokemonType.Electric] = 2f,
                [PokemonType.Psychic] = 1f,
                [PokemonType.Ice] = 0.5f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 1f,
                [PokemonType.Fairy] = 1f,
            },
            [PokemonType.Grass] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 1f,
                [PokemonType.Flying] = 2f,
                [PokemonType.Poison] = 2f,
                [PokemonType.Ground] = 0.5f,
                [PokemonType.Rock] = 1f,
                [PokemonType.Bug] = 2f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 1f,
                [PokemonType.Fire] = 2f,
                [PokemonType.Water] = 0.5f,
                [PokemonType.Grass] = 0.5f,
                [PokemonType.Electric] = 0.5f,
                [PokemonType.Psychic] = 1f,
                [PokemonType.Ice] = 2f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 1f,
                [PokemonType.Fairy] = 1f,
            },
            [PokemonType.Electric] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 1f,
                [PokemonType.Flying] = 0.5f,
                [PokemonType.Poison] = 1f,
                [PokemonType.Ground] = 2f,
                [PokemonType.Rock] = 1f,
                [PokemonType.Bug] = 1f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 0.5f,
                [PokemonType.Fire] = 1f,
                [PokemonType.Water] = 1f,
                [PokemonType.Grass] = 1f,
                [PokemonType.Electric] = 0.5f,
                [PokemonType.Psychic] = 1f,
                [PokemonType.Ice] = 1f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 1f,
                [PokemonType.Fairy] = 1f,
            },
            [PokemonType.Psychic] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 0.5f,
                [PokemonType.Flying] = 1f,
                [PokemonType.Poison] = 1f,
                [PokemonType.Ground] = 1f,
                [PokemonType.Rock] = 1f,
                [PokemonType.Bug] = 2f,
                [PokemonType.Ghost] = 2f,
                [PokemonType.Steel] = 1f,
                [PokemonType.Fire] = 1f,
                [PokemonType.Water] = 1f,
                [PokemonType.Grass] = 1f,
                [PokemonType.Electric] = 1f,
                [PokemonType.Psychic] = 0.5f,
                [PokemonType.Ice] = 1f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 2f,
                [PokemonType.Fairy] = 1f,
            },
            [PokemonType.Ice] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 2f,
                [PokemonType.Flying] = 1f,
                [PokemonType.Poison] = 1f,
                [PokemonType.Ground] = 1f,
                [PokemonType.Rock] = 2f,
                [PokemonType.Bug] = 1f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 2f,
                [PokemonType.Fire] = 2f,
                [PokemonType.Water] = 1f,
                [PokemonType.Grass] = 1f,
                [PokemonType.Electric] = 1f,
                [PokemonType.Psychic] = 1f,
                [PokemonType.Ice] = 0.5f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 1f,
                [PokemonType.Fairy] = 1f,
            },
            [PokemonType.Dragon] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 1f,
                [PokemonType.Flying] = 1f,
                [PokemonType.Poison] = 1f,
                [PokemonType.Ground] = 1f,
                [PokemonType.Rock] = 1f,
                [PokemonType.Bug] = 1f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 1f,
                [PokemonType.Fire] = 0.5f,
                [PokemonType.Water] = 0.5f,
                [PokemonType.Grass] = 0.5f,
                [PokemonType.Electric] = 0.5f,
                [PokemonType.Psychic] = 1f,
                [PokemonType.Ice] = 2f,
                [PokemonType.Dragon] = 2f,
                [PokemonType.Dark] = 1f,
                [PokemonType.Fairy] = 2f,
            },
            [PokemonType.Dark] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 2f,
                [PokemonType.Flying] = 1f,
                [PokemonType.Poison] = 1f,
                [PokemonType.Ground] = 1f,
                [PokemonType.Rock] = 1f,
                [PokemonType.Bug] = 2f,
                [PokemonType.Ghost] = 0.5f,
                [PokemonType.Steel] = 1f,
                [PokemonType.Fire] = 1f,
                [PokemonType.Water] = 1f,
                [PokemonType.Grass] = 1f,
                [PokemonType.Electric] = 1f,
                [PokemonType.Psychic] = 0f,
                [PokemonType.Ice] = 1f,
                [PokemonType.Dragon] = 1f,
                [PokemonType.Dark] = 0.5f,
                [PokemonType.Fairy] = 2f,
            },
            [PokemonType.Fairy] = new Dictionary<PokemonType, float>
            {
                [PokemonType.Normal] = 1f,
                [PokemonType.Fighting] = 0.5f,
                [PokemonType.Flying] = 1f,
                [PokemonType.Poison] = 2f,
                [PokemonType.Ground] = 1f,
                [PokemonType.Rock] = 1f,
                [PokemonType.Bug] = 0.5f,
                [PokemonType.Ghost] = 1f,
                [PokemonType.Steel] = 2f,
                [PokemonType.Fire] = 1f,
                [PokemonType.Water] = 1f,
                [PokemonType.Grass] = 1f,
                [PokemonType.Electric] = 1f,
                [PokemonType.Psychic] = 1f,
                [PokemonType.Ice] = 1f,
                [PokemonType.Dragon] = 0f,
                [PokemonType.Dark] = 0.5f,
                [PokemonType.Fairy] = 1f,
            },
        };
    }

    public enum PokemonType
    {
        Bug,
        Dark,
        Dragon,
        Electric,
        Fairy,
        Fighting,
        Fire,
        Flying,
        Ghost,
        Grass,
        Ground,
        Ice,
        Normal,
        Poison,
        Psychic,
        Rock,
        Steel,
        Water,
        Nuclear,
        Light,
        Machine,
        Sound
    }

}
