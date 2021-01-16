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

namespace Terramon.Items.Food
{
    public class StoneBowl : ModItem
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Stone Bowl");
            Tooltip.SetDefault("A sturdy, rock-solid bowl.\nUsed to create different kinds of Apricorn Stew.");
        }
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 8;
            item.maxStack = 99;
            item.value = 10;
            item.rare = 0;
            // Set other item.X values here
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Bowl);
            recipe.AddIngredient(ItemID.StoneBlock, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
