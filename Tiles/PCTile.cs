using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Razorwing.Framework.Graphics;
using Terramon.UI;
using System.Collections.Generic;
using System.Linq;

namespace Terramon.Tiles
{
    //PC Tile

    public class PCTile : ModTile
    {
        public bool on = false;
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            minPick = 0;
            dustType = 117;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
            TileObjectData.newTile.StyleHorizontal = false;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(166, 41, 60), Language.GetText("PC"));
        }

        private readonly int animationFrameWidth = 56;

        public override bool HasSmartInteract()
        {
            return true;
        }
        public override bool NewRightClick(int i, int j)
        {
            HitWire(i, j);
            return true;
        }
        public override void HitWire(int i, int j)
        {
            int x = i - Main.tile[i, j].frameX / 18 % 2;
            int y = j - Main.tile[i, j].frameY / 18 % 3;
            if (Main.tile[x, y].frameY >= 56)
            {
                // Deactivate
                Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/pc_off").WithVolume(.2f));
                Main.tile[x, y].frameY -= 56;
                Main.tile[x + 1, y].frameY -= 56;
                Main.tile[x, y + 1].frameY -= 56;
                Main.tile[x + 1, y + 1].frameY -= 56;
            }
            else
            {
                // Activate
                Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/pc").WithVolume(.7f));
                Main.tile[x, y].frameY += 56;
                Main.tile[x + 1, y].frameY += 56;
                Main.tile[x, y + 1].frameY += 56;
                Main.tile[x + 1, y + 1].frameY += 56;
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("PCItem"));
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            int x = i - Main.tile[i, j].frameX / 18 % 2;
            int y = j - Main.tile[i, j].frameY / 18 % 3;
            if (Main.tile[x, y].frameY >= 56)
            {
                // We can support different light colors for different styles here: switch (tile.frameY / 54)
                r = 72 / 255f;
                g = 161 / 255f;
                b = 199 / 255f;
            }
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            // Tweak the frame drawn by x position so tiles next to each other are off-sync and look much more interesting.
            int uniqueAnimationFrame = Main.tileFrame[Type];
            uniqueAnimationFrame = uniqueAnimationFrame % 2;

            frameYOffset = uniqueAnimationFrame * animationFrameWidth;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = ModContent.ItemType<PCItem>();
        }
    }

    //PC Item

    public class PCItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PC");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.maxStack = 99;
            item.value = 40000;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = mod.TileType("PCTile");
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
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine nameLine = tooltips.FirstOrDefault(t => t.Name == "ItemName" && t.mod == "Terraria");

            foreach (TooltipLine line2 in tooltips)
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                    line2.overrideColor = new Color(190, 49, 49);
        }
    }
}