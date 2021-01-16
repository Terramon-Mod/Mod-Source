using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Pokemon.FirstGeneration.Fishing
{
    public class HorseaFish : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Horsea");
            Tooltip.SetDefault("It's a Pokémon you caught while fishing."
                               + "\nSpawns a catchable Horsea when used.");
        }

        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 48;
            item.maxStack = 30;
            item.useTime = 16;
            item.useAnimation = 16;
            item.value = 0;
            item.rare = 0;
            item.consumable = true;
            item.useStyle = 1;
            item.UseSound = SoundID.Item2;
            item.noUseGraphic = true;
            // Set other item.X values here
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine nameLine = tooltips.FirstOrDefault(t => t.Name == "ItemName" && t.mod == "Terraria");

            foreach (TooltipLine line2 in tooltips)
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                    line2.overrideColor = new Color(46, 211, 255);
        }

        public override bool UseItem(Player player)
        {
            NPC.NewNPC((int) player.position.X, (int) player.position.Y, mod.NPCType("HorseaNPC"));
            return true;
        }
    }
}