using Terraria;
using Terraria.ModLoader;
using System.Diagnostics;
using static Terraria.ModLoader.ModContent;
using Terramon.Items.MiscItems.GameBoys;

namespace Terramon.Items.MiscItems.GameBoys
{
    public abstract class GameBoy : TerramonItem
    {
        /// <summary></summary>
        /// <param name="color"></param>
        /// <param name="tooltip"></param>
        /// <param name="value"></param>
        /// <param name="rarity"></param>
        /// <param name="musicPath"></param>
        protected GameBoy(string color, string tooltip, int value, int rarity, string musicPath) : base(
            $"Game Boy ({color})", "Sell this item and download Terramon Expansion to buy a new one.", 22, 32, Item.sellPrice(gold: 5), 0, rarity)
        {

        }

        public override string Texture => mod.Name + "/Items/MiscItems/GameBoys/GameBoy";


        public override void SetDefaults()
        {
            base.SetDefaults();

            item.maxStack = 1;

            item.useTime = 34;
            item.useAnimation = 34;
            item.useStyle = 4;

            item.accessory = false;
            item.autoReuse = false;
            item.consumable = true;
        }
    }
}