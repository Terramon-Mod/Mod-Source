using Terraria;
using Terraria.ID;

namespace Terramon.Items.MiscItems.GameBoys
{
    public class GameBoyTeal : GameBoy
    {
        public const string TOOLTIP = "It's an 8-bit handheld console." +
                                      "\n[c/33ceff:Equip this to listen to music from Pokémon Fire Red!]" +
                                      "\n[c/FFFF66:Soundtrack: Pewter City]";


        public GameBoyTeal() : base("Teal", TOOLTIP, Item.sellPrice(gold: 5), ItemRarityID.Orange, "PewterCity")
        {
        }
    }
}