using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terramon.Pokemon.Utilities.ID;

namespace Terramon.Items.MiscItems.Medication
{
    public class IceHeal : ModItem
    {
        public ItemGroup itemGroup = ItemGroup.StatusConditionHealing;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Ice Heal");
            Tooltip.SetDefault("A spray-type medicine for treating freezing."
                               + "\nIt can be used to thaw out a single Pokémon that has been frozen solid."
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
                    line2.overrideColor = new Color(232, 118, 166);
        }

        public override bool CanBurnInLava()
        {
            return false;
        }
    }
}