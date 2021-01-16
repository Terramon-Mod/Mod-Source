using System.Collections.Generic;
using Terramon.Items.Pokeballs.Inventory;
using Terramon.Pokemon;
using Terraria.ModLoader.IO;

namespace Terramon.Players
{
    public sealed partial class TerramonPlayer
    {
        private void InitializePokeballs()
        {
            ThrownPokeballs = new Dictionary<string, int>();
        }


        #region Thrown Pokeballs

        public void IncrementThrownPokeballs(BasePokeballItem pokeball)
        {
            IncrementThrownPokeballs(pokeball.UnlocalizedName);
        }

        public int IncrementThrownPokeballs(string unlocalizedName)
        {
            return ThrownPokeballs[unlocalizedName] = GetOrCreateThrownPokeballs(unlocalizedName) + 1;
        }

        public int GetThrownPokeballsCount(BasePokeballItem pokeball)
        {
            return GetThrownPokeballsCount(pokeball.UnlocalizedName);
        }

        public int GetThrownPokeballsCount(string unlocalizedName)
        {
            return GetOrCreateThrownPokeballs(unlocalizedName);
        }

        private int GetOrCreateThrownPokeballs(string unlocalizedName)
        {
            if (!ThrownPokeballs.ContainsKey(unlocalizedName))
                ThrownPokeballs.Add(unlocalizedName, 0);

            return ThrownPokeballs[unlocalizedName];
        }

        #endregion


        private void SavePokeballs(TagCompound tag)
        {
            TagCompound thrownPokeballs = new TagCompound();

            foreach (KeyValuePair<string, int> kvp in ThrownPokeballs)
                thrownPokeballs.Add(kvp.Key, kvp.Value);

            tag.Add(nameof(ThrownPokeballs), thrownPokeballs);
        }

        private void LoadPokeballs(TagCompound tag)
        {
            ThrownPokeballs = new Dictionary<string, int>();

            foreach (KeyValuePair<string, object> kvp in tag.GetCompound(nameof(ThrownPokeballs)))
                ThrownPokeballs.Add(kvp.Key, int.Parse(kvp.Value.ToString()));
        }

        private void FormatSlots()
        {
            PokemonData[] format = new PokemonData[6];
            int formatCounter = 0;
            format[0] = PartySlot1;
            format[1] = PartySlot2;
            format[2] = PartySlot3;
            format[3] = PartySlot4;
            format[4] = PartySlot5;
            format[5] = PartySlot6;
            foreach (PokemonData sl in format)
            {
                formatCounter++;
                if (sl != null)
                {
                    if (formatCounter == 1) PartySlot1 = sl;
                    if (formatCounter == 2) PartySlot2 = sl;
                    if (formatCounter == 3) PartySlot3 = sl;
                    if (formatCounter == 4) PartySlot4 = sl;
                    if (formatCounter == 5) PartySlot5 = sl;
                    if (formatCounter == 6) PartySlot6 = sl;
                }
            }
        }


        public Dictionary<string, int> ThrownPokeballs { get; private set; }
    }
}