using Terraria;
using Terraria.ID;

namespace Terramon.Items.MiscItems.GameBoys
{
    public class GameBoyGray : GameBoy
    {
        public const string TOOLTIP = "It's an 8-bit handheld console." +
                                      "\n[c/33ceff:Equip this to listen to music from Pokémon Fire Red!]" +
                                      "\n[c/FFFF66:Soundtrack: Title Screen]";


        public GameBoyGray() : base("Gray", TOOLTIP, Item.sellPrice(gold: 5), ItemRarityID.Orange, "TitleScreen")
        {
        }
    }
}