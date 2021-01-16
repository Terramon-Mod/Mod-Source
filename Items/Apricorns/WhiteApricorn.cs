using Terraria.ModLoader;

namespace Terramon.Items.Apricorns
{
    public class WhiteApricorn : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("White Apricorn");
            Tooltip.SetDefault("A special fruit seemingly related to berries."
                               + "\nCan be used to craft assorted Poké Balls.");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.maxStack = 999;
            item.value = 500;
            item.rare = 0;
            item.scale = 1.2f;
            // Set other item.X values here
        }
    }
}