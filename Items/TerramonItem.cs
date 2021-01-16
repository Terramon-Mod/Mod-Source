using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
//using WebmilioCommons.Items.Standard;

namespace Terramon.Items
{
    public abstract class TerramonItem : ModItem
    {

        protected Dictionary<GameCulture, string> displayNames;
        protected Dictionary<GameCulture, string> tooltips;
        protected int rarity = ItemRarityID.White;
        protected int maxStack = 999;
        protected int value = 0;
        protected int width, height, defense;

        protected TerramonItem(string displayName, string tooltip, int width, int height, int value = 0, int defense = 0, int rarity = ItemRarityID.White, int maxStack = 999) : this(new Dictionary<GameCulture, string>() { [GameCulture.English] = displayName },
            new Dictionary<GameCulture, string>() { [GameCulture.English] = tooltip }, width, height, value, defense, rarity, maxStack)
        {
            
        }

        public override void SetStaticDefaults()
        {
            //var lang = Language.ActiveCulture;

            DisplayName.SetDefault(displayNames.ContainsKey(GameCulture.English) ? displayNames[GameCulture.English] : displayNames.FirstOrDefault().Value);
            foreach (var it in displayNames)
            {
                DisplayName.AddTranslation(it.Key, it.Value);
            }

            Tooltip.SetDefault(tooltips.ContainsKey(GameCulture.English) ? tooltips[GameCulture.English] : tooltips.FirstOrDefault().Value);
            foreach (var it in tooltips)
            {
                Tooltip.AddTranslation(it.Key, it.Value);
            }

        }

        protected TerramonItem(Dictionary<GameCulture, string> displayNames, Dictionary<GameCulture, string> tooltips, int width, int height, int value = 0, int defense = 0, int rarity = ItemRarityID.White, int maxStack = 999)
        {
            this.displayNames = displayNames;
            this.tooltips = tooltips;
            this.width = width;
            this.height = height;
            this.value = value;
            this.rarity = rarity;
            this.defense = defense;
            this.maxStack = maxStack;
        }

        public override void SetDefaults()
        {
            item.width = width;
            item.height = height;
            item.value = value;

            item.rare = rarity;
            item.defense = defense;
            item.maxStack = maxStack;
        }
    }
}