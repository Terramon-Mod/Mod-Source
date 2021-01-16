using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Items.Pokeballs.Parts
{
    public class DarkButton : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Dark Button");
            Tooltip.SetDefault("Combine it with a base and a cap to create different kinds of Poké Balls.");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 12;
            item.maxStack = 999;
            item.value = 7000;
            item.rare = 0;
        }

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Button"));
            recipe.AddIngredient(mod.GetItem("BlueApricorn"));
            recipe.AddIngredient(mod.GetItem("BlackApricorn"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine nameLine = tooltips.FirstOrDefault(t => t.Name == "ItemName" && t.mod == "Terraria");

            foreach (TooltipLine line2 in tooltips)
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                    line2.overrideColor = new Color(125, 125, 125);
        }
    }
}