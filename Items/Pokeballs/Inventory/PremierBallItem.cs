using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terramon.Items.Pokeballs.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terramon.Items.Pokeballs.Inventory
{
    public class PremierBallItem : BaseThrowablePokeballItem<PremierBallProjectile>
    {
        public PremierBallItem() : base(Constants.Pokeballs.UnlocalizedNames.PREMIER_BALL,
            new Dictionary<GameCulture, string>
            {
                {GameCulture.English, "Premier Ball"},
                {GameCulture.French, "Honor Ball"}
            },
            new Dictionary<GameCulture, string>
            {
                {
                    GameCulture.English,
                    "A rare Ball made in commemoration of some event.\nObtained after buying 10 regular Poké Balls at once."
                },
                {
                    GameCulture.French,
                    "Un Ball rare fait en commémoration d'un événement.\nObtenu après l'achat de 10 Poké Balls réguliers à la fois."
                }
            },
            Item.sellPrice(), ItemRarityID.White, Constants.Pokeballs.CatchRates.PREMIER_BALL)
        {
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("PremierBallCap"));
            recipe.AddIngredient(mod.ItemType("Button"));
            recipe.AddIngredient(mod.ItemType("PokeballBase"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine nameLine = tooltips.FirstOrDefault(t => t.Name == "ItemName" && t.mod == "Terraria");

            base.ModifyTooltips(tooltips);

            tooltips.RemoveAll(l => l.Name == "Damage");
            tooltips.RemoveAll(l => l.Name == "CritChance");
            tooltips.RemoveAll(l => l.Name == "Speed");
            tooltips.RemoveAll(l => l.Name == "Knockback");

            if (NameColorOverride != null)
                tooltips.Find(t => t.Name == "ItemName").overrideColor = NameColorOverride;

            foreach (TooltipLine line2 in tooltips)
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                    line2.overrideColor = new Color(186, 207, 222);
        }
    }
}