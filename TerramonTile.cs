using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon
{
    public class TerramonTile : GlobalTile
    {
        public override bool Drop(int i, int j, int type)
        {
            var player = Main.player[Main.myPlayer];
            if (Main.netMode != 1 && !WorldGen.noTileActions && !WorldGen.gen)
                if (type == TileID.Trees && Main.tile[i, j + 1].type == TileID.Grass ||
                    type == TileID.Trees && Main.tile[i, j + 1].type == TileID.HallowedGrass)
                    switch (Main.rand.Next(7))
                    {
                        case 0:
                            Item.NewItem(i * 16, (j - 5) * 16, 32, 32, mod.ItemType("BlackApricorn"),
                                Main.rand.Next(1, 3));
                            break;
                        case 1:
                            Item.NewItem(i * 16, (j - 5) * 16, 32, 32, mod.ItemType("RedApricorn"),
                                Main.rand.Next(1, 3));
                            break;
                        case 2:
                            Item.NewItem(i * 16, (j - 5) * 16, 32, 32, mod.ItemType("BlueApricorn"),
                                Main.rand.Next(1, 3));
                            break;
                        case 3:
                            Item.NewItem(i * 16, (j - 5) * 16, 32, 32, mod.ItemType("YellowApricorn"),
                                Main.rand.Next(1, 3));
                            break;
                        case 4:
                            Item.NewItem(i * 16, (j - 5) * 16, 32, 32, mod.ItemType("WhiteApricorn"),
                                Main.rand.Next(1, 3));
                            break;
                        case 5:
                            Item.NewItem(i * 16, (j - 5) * 16, 32, 32, mod.ItemType("PinkApricorn"),
                                Main.rand.Next(1, 3));
                            break;
                        case 6:
                            Item.NewItem(i * 16, (j - 5) * 16, 32, 32, mod.ItemType("GreenApricorn"),
                                Main.rand.Next(1, 3));
                            break;
                    }

            return base.Drop(i, j, type);
        }
    }
}