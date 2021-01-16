using Terraria;
using Terraria.ID;

namespace Terramon.Items.MiscItems.GameBoys
{
    public class GameBoyDark : GameBoy
    {
        public const string TOOLTIP = "It's an 8-bit handheld console." +
                                      "\n[c/33ceff:Equip this to listen to music from Pokemon Fire Red!]" +
                                      "\n[c/FFFF66:Soundtrack: Lavender Town]";


        public GameBoyDark() : base("Dark", TOOLTIP, Item.sellPrice(gold: 6, silver: 6, copper: 6), ItemRarityID.Orange, "Lavender")
        {
        }
    }
}