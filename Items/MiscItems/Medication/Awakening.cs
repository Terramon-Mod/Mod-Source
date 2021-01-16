using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terramon.Pokemon.Utilities.ID;

namespace Terramon.Items.MiscItems.Medication
{
    public class Awakening : ModItem
    {
        public ItemGroup itemGroup = ItemGroup.StatusConditionHealing;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Awakening");
            Tooltip.SetDefault("A spray-type medicine to wake the sleeping."
                               + "\nIt can be used to rouse a single Pokémon from the clutches of sleep."
                               + "\nHold it and left click a Pokémon in your party to use.");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 34;
            item.maxStack = 99;
            item.value = 225;
            item.rare = 0;
            // Set other item.X values here
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine nameLine = tooltips.FirstOrDefault(t => t.Name == "ItemName" && t.mod == "Terraria");

            foreach (TooltipLine line2 in tooltips)
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                    line2.overrideColor = new Color(105, 169, 207);
        }

        public override bool CanBurnInLava()
        {
            return false;
        }
    }
}