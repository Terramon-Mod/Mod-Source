using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terramon.Pokemon.Utilities.ID;

namespace Terramon.Items.MiscItems.Medication
{
    public class Revive : ModItem
    {
        public ItemGroup itemGroup = ItemGroup.Revive;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Revive");
            Tooltip.SetDefault("A medicine that revives a fainted Pokémon."
                               + "\nIt restores half the Pokémon's maximum HP."
                               + "\nHold it and left click a Pokémon in your party to use.");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 26;
            item.maxStack = 99;
            item.value = 700;
            item.rare = 0;
            // Set other item.X values here
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine nameLine = tooltips.FirstOrDefault(t => t.Name == "ItemName" && t.mod == "Terraria");

            foreach (TooltipLine line2 in tooltips)
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                    line2.overrideColor = new Color(250, 210, 110);
        }

        public override bool CanBurnInLava()
        {
            return false;
        }
    }
}