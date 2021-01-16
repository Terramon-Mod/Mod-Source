using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Items.Pokeballs.Parts
{
    public class ZeroBallBase : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Zero Ball Base");
            Tooltip.SetDefault("Combine it with a dark button and cap to create a Zero Ball.");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.maxStack = 999;
            item.value = 8000;
            item.rare = 0;
        }

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("PokeballBase"));
            recipe.AddIngredient(mod.ItemType("BlueApricorn"));
            recipe.AddIngredient(mod.ItemType("BlackApricorn"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/


        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine nameLine = tooltips.FirstOrDefault(t => t.Name == "ItemName" && t.mod == "Terraria");

            foreach (TooltipLine line2 in tooltips)
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                    line2.overrideColor = new Color(22, 100, 148);
        }
    }
}
 
 