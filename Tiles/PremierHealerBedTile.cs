using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terramon.Tiles.ShelfBlocks;
using Terramon.Players;
using Terraria.Enums;

namespace Terramon.Tiles
{
    //Premier Healer Tile

    public class PremierHealerBedTile : ModTile
    {
        public bool on = false;
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;

            minPick = 0;
            dustType = 117;
            //TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, 4, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.newTile.StyleHorizontal = false;
            TileObjectData.newTile.LavaDeath = true;
            // TE initialization
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TEPremierHealerBed>().Hook_AfterPlacement, -1, 0, false);
            disableSmartCursor = true;
            TileObjectData.addTile(Type);

            adjTiles = new int[] { TileID.Beds };

            bed = true;

            AddMapEntry(new Color(255, 255, 255), Language.GetText("Healer"));
        }

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
            var player = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
            int x = i - Main.tile[i, j].frameX / 18 % 4;
            int y = j - Main.tile[i, j].frameY / 18 % 2;
            Point16 pos = new Point16(x, y);
            if (TileEntity.ByPosition.ContainsKey(pos) && TileEntity.ByPosition[pos] is TEPremierHealerBed bed)
            {
                if (player.PartySlot1 != null)
                {
                    if (!bed.active) bed.active = true;
                }
                else
                {
                    Main.NewText("Your party is empty");
                    Main.PlaySound(ModContent.GetInstance<TerramonMod>().GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/denied").WithVolume(.15f));
                }
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 64, 32, mod.ItemType("PremierHealerBedItem"));
            ModContent.GetInstance<TEPremierHealerBed>().Kill(i, j);
            ModContent.GetInstance<TEPremierHealerBed>().OnKill();
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
            if (Framing.GetTileSafely(i, j).frameX == 0 && Framing.GetTileSafely(i, j).frameY == 0)
            {
                Main.specX[nextSpecialDrawIndex] = i;
                Main.specY[nextSpecialDrawIndex] = j;
                nextSpecialDrawIndex++;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.PostDraw(i, j, spriteBatch);

            int x = i - Main.tile[i, j].frameX / 18 % 4;
            int y = j - Main.tile[i, j].frameY / 18 % 2;
            Point16 pos = new Point16(x, y);

            var te = (TEPremierHealerBed)TileEntity.ByPosition[pos];
            if (te.drawFirstBall)
            {
                var player = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
                Vector2 world = new Point16(x, y).ToWorldCoordinates(0, 0);
                var _texture = ModContent.GetTexture($"Terramon/Tiles/ShelfBlocks/{pokeballTypeToShelf(player.PartySlot1.pokeballType)}Shelf");
                spriteBatch.Draw(position: world - Main.screenPosition + new Vector2(202, 194), texture: _texture, sourceRectangle: null, color: Lighting.GetColor(pos.X, pos.Y), rotation: 0f, origin: Vector2.Zero, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
            }
            if (te.drawSecondBall)
            {
                var player = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
                Vector2 world = new Point16(x, y).ToWorldCoordinates(0, 0);
                var _texture = ModContent.GetTexture($"Terramon/Tiles/ShelfBlocks/{pokeballTypeToShelf(player.PartySlot2.pokeballType)}Shelf");
                spriteBatch.Draw(position: world - Main.screenPosition + new Vector2(202, 194), texture: _texture, sourceRectangle: null, color: Lighting.GetColor(pos.X, pos.Y), rotation: 0f, origin: Vector2.Zero, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
            }
            if (te.drawThirdBall)
            {
                var player = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
                Vector2 world = new Point16(x, y).ToWorldCoordinates(0, 0);
                var _texture = ModContent.GetTexture($"Terramon/Tiles/ShelfBlocks/{pokeballTypeToShelf(player.PartySlot3.pokeballType)}Shelf");
                spriteBatch.Draw(position: world - Main.screenPosition + new Vector2(202 + 14, 194), texture: _texture, sourceRectangle: null, color: Lighting.GetColor(pos.X, pos.Y), rotation: 0f, origin: Vector2.Zero, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
            }
            if (te.drawFourthBall)
            {
                var player = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
                Vector2 world = new Point16(x, y).ToWorldCoordinates(0, 0);
                var _texture = ModContent.GetTexture($"Terramon/Tiles/ShelfBlocks/{pokeballTypeToShelf(player.PartySlot4.pokeballType)}Shelf");
                spriteBatch.Draw(position: world - Main.screenPosition + new Vector2(202 + 14, 194), texture: _texture, sourceRectangle: null, color: Lighting.GetColor(pos.X, pos.Y), rotation: 0f, origin: Vector2.Zero, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
            }
            if (te.drawFifthBall)
            {
                var player = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
                Vector2 world = new Point16(x, y).ToWorldCoordinates(0, 0);
                var _texture = ModContent.GetTexture($"Terramon/Tiles/ShelfBlocks/{pokeballTypeToShelf(player.PartySlot5.pokeballType)}Shelf");
                spriteBatch.Draw(position: world - Main.screenPosition + new Vector2(202 + 28, 194), texture: _texture, sourceRectangle: null, color: Lighting.GetColor(pos.X, pos.Y), rotation: 0f, origin: Vector2.Zero, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
            }
            if (te.drawSixthBall)
            {
                var player = Main.LocalPlayer.GetModPlayer<TerramonPlayer>();
                Vector2 world = new Point16(x, y).ToWorldCoordinates(0, 0);
                var _texture = ModContent.GetTexture($"Terramon/Tiles/ShelfBlocks/{pokeballTypeToShelf(player.PartySlot6.pokeballType)}Shelf");
                spriteBatch.Draw(position: world - Main.screenPosition + new Vector2(202 + 28, 194), texture: _texture, sourceRectangle: null, color: Lighting.GetColor(pos.X, pos.Y), rotation: 0f, origin: Vector2.Zero, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
            }
        }
        public virtual string pokeballTypeToShelf(byte a)
        {
            if (a == 1)
            {
                return "Pokeball";
            }
            else
            if (a == 2)
            {
                return "GreatBall";
            }
            else
            if (a == 3)
            {
                return "UltraBall";
            }
            else
            if (a == 4)
            {
                return "MasterBall";
            }
            else
            if (a == 5)
            {
                return "DuskBall";
            }
            else
            if (a == 6)
            {
                return "PremierBall";
            }
            else
            if (a == 7)
            {
                return "QuickBall";
            }
            else
            if (a == 8)
            {
                return "TimerBall";
            }
            else
            if (a == 9)
            {
                return "ZeroBall";
            }
            else
            if (a == 10)
            {
                return "PumpkinBall";
            }
            else
            {
                return "Pokeball";
            }
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            int x = i - Main.tile[i, j].frameX / 18 % 4;
            int y = j - Main.tile[i, j].frameY / 18 % 2;
            Point16 pos = new Point16(x, y);

            var te = (TEPremierHealerBed)TileEntity.ByPosition[pos];
            if (te.active)
            {
                // We can support different light colors for different styles here: switch (tile.frameY / 54)
                r = 255 / 300f;
                g = 255 / 300f;
                b = 255 / 300f;
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = ModContent.ItemType<PokeballShelf_Held>();
        }
    }

    public class PremierHealerBedItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Premier Healer");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 22;
            item.maxStack = 99;
            item.value = 40000;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = mod.TileType("PremierHealerBedTile");
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