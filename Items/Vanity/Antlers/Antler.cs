using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace Terramon.Items.Vanity.Antlers
{
	[AutoloadEquip(EquipType.Head)]
	public class Antler : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Pokemon Antlers");
			Tooltip.SetDefault("No stantlers were harmed in the making of this product.");
		}

		public override void SetDefaults() {
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.vanity = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("Wood", 2);
			recipe.AddIngredient(ItemID.Topaz, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = true; 
		}
	}
}