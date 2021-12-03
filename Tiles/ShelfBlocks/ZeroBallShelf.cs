using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;

namespace Terramon.Tiles.ShelfBlocks
{
    //Pokeball Tile

    public class ZeroBallShelf : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileShine[Type] = 1100;
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            minPick = 0;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(8, 60, 84), Language.GetText("Zero Ball"));
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.frameX == 0)
            {
                // We can support different light colors for different styles here: switch (tile.frameY / 54)
                r = 0f;
                g = 0.30f;
                b = 0.66f;
            }
        }

        public override bool Drop(int i, int j)
        {
            Tile t = Main.tile[i, j];
            int style = t.frameX / 18;
            if (style == 0)
            {
                Item.NewItem(i * 16, j * 16, 16, 16, mod.ItemType("ZeroBallShelf_Held"));
            }
            return base.Drop(i, j);
        }
    }

    //Pokeball Item

    public class ZeroBallShelf_Held : ModItem
    {
        public override string Texture => mod.Name + "/Tiles/ShelfBlocks/ZeroBallShelf";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zero Ball Prop");
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
            item.createTile = mod.TileType("ZeroBallShelf");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("BlackApricorn"), 2);
            recipe.AddIngredient(ItemID.Nanites);
            recipe.AddIngredient(ItemID.IronBar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}