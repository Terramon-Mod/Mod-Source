using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Terramon.Items.MiscItems.LinkCables
{
    public class LinkCable : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Link Cable");
            Tooltip.SetDefault("It enables certain kind of Pokémon to evolve."
                               + "\nGive it to the Pokémon when it's ready to evolve."
                               + "\nThis item serves as a replacement for the trading mechanic in single player.");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 26;
            item.scale = 1f;
            item.maxStack = 99;
            item.value = 100000;
            item.rare = 0;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine nameLine = tooltips.FirstOrDefault(t => t.Name == "ItemName" && t.mod == "Terraria");

            foreach (TooltipLine line2 in tooltips)
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                    line2.overrideColor = new Color(60, 171, 70);
        }

        public virtual bool CanBurnInLava()
        {
            return false;
        }
    }
}