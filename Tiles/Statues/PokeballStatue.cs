using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;

namespace Terramon.Tiles.Statues
{
    //Statue Tile

	public class PokeballStatue : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileShine[Type] = 1100;
			Main.tileSolid[Type] = false;
			Main.tileSolidTop[Type] = false;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            minPick = 0;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(53, 79, 17), Language.GetText("Statue"));
		}

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("PokeballStatueItem"));
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];

            if (tile.frameX > 18)
            {
                // We can support different light colors for different styles here: switch (tile.frameY / 54)
                r = 1f;
                g = 0.4f;
                b = 0.4f;
            }
        }

        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int topY = j - tile.frameY / 18 % 3;
            short frameAdjustment = (short)(tile.frameX > 0 ? -18*2 : 18*2);
            Main.tile[i, topY].frameX += frameAdjustment;
            Main.tile[i, topY + 1].frameX += frameAdjustment;
            Main.tile[i+1, topY].frameX += frameAdjustment;
            Main.tile[i+1, topY + 1].frameX += frameAdjustment;
            //Main.tile[i, topY + 2].frameX += frameAdjustment;
            Wiring.SkipWire(i, topY);
            Wiring.SkipWire(i, topY + 1);
            Wiring.SkipWire(i+1, topY);
            Wiring.SkipWire(i+1, topY + 1);
            //Wiring.SkipWire(i, topY + 2);
            NetMessage.SendTileSquare(-1, i, topY + 1, 2, TileChangeType.None);
            NetMessage.SendTileSquare(-1, i+1, topY + 1, 2, TileChangeType.None);

        }
    }
    
    //Statue Item

        public class PokeballStatueItem : ModItem
        {
        public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Pokeball Statue");
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
                item.createTile = mod.TileType("PokeballStatue");
            }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("PokeballItem"));
            recipe.AddIngredient(ItemID.StoneBlock, 50);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}