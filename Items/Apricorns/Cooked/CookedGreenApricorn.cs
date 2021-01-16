using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace Terramon.Items.Apricorns.Cooked
{
    public class CookedGreenApricorn : ModItem
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Cooked Green Apricorn");
            Tooltip.SetDefault("Combine these with a Stone Bowl to create Apricorn Stew."
                + "\nIt looks really tasty..");
        }
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.maxStack = 999;
            item.value = 500;
            item.rare = 0;
            // Set other item.X values here
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GreenApricorn"));
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine nameLine = tooltips.FirstOrDefault(t => t.Name == "ItemName" && t.mod == "Terraria");

            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = new Color(54, 190, 49);
                }
            }
        }
    }
}
