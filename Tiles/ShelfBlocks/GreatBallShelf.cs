using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;

namespace Terramon.Tiles.ShelfBlocks
{
    //Pokeball Tile

	public class GreatBallShelf : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileShine[Type] = 1100;
			Main.tileSolid[Type] = false;
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
            minPick = 0;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(120, 145, 245), Language.GetText("Great Ball"));
		}

		public override bool Drop(int i, int j)
		{
			Tile t = Main.tile[i, j];
			int style = t.frameX / 18;
			if (style == 0) 
			{
				Item.NewItem(i * 16, j * 16, 16, 16, mod.ItemType("GreatBallShelf_Held"));
			}
			return base.Drop(i, j);
		}
	}
    
    //Pokeball Item

        public class GreatBallShelf_Held : ModItem
        {
        public override string Texture => mod.Name + "/Tiles/ShelfBlocks/GreatBallShelf";
        public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Great Ball Prop");
            }

            public override void SetDefaults()
            {
                item.width = 12;
                item.height = 12;
                item.maxStack = 99;
                item.value = 1000;
                item.useTurn = true;
                item.autoReuse = true;
                item.useAnimation = 15;
                item.useTime = 10;
                item.useStyle = 1;
                item.consumable = true;
                item.createTile = mod.TileType("GreatBallShelf");
            }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("BlueApricorn"));
            recipe.AddIngredient(mod.ItemType("RedApricorn"));
            recipe.AddIngredient(ItemID.IronBar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}