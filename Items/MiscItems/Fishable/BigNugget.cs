using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Terramon.Items.MiscItems.Fishable
{
    public class BigNugget : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Big Nugget");
            Tooltip.SetDefault("A big nugget of pure gold that gives off a lustrous gleam."
                               + "\nIt can be sold at a high price to NPCs.");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.maxStack = 99;
            item.rare = 0;
            item.value = 1000000;
            // Set other item.X values here
        }

        public override bool CanBurnInLava()
        {
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine nameLine = tooltips.FirstOrDefault(t => t.Name == "ItemName" && t.mod == "Terraria");

            foreach (TooltipLine line2 in tooltips)
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                    line2.overrideColor = new Color(255, 235, 84);
        }
    }
}