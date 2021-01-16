using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terramon.Items.Pokeballs.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terramon.Items.Pokeballs.Inventory
{
    public class DuskBallItem : BaseThrowablePokeballItem<DuskBallProjectile>
    {
        public DuskBallItem() : base(Constants.Pokeballs.UnlocalizedNames.DUSK_BALL,
            new Dictionary<GameCulture, string>
            {
                {GameCulture.English, "Dusk Ball"},
                {GameCulture.French, "Sombre Ball"}
            },
            new Dictionary<GameCulture, string>
            {
                {
                    GameCulture.English,
                    "A somewhat different Poké Ball.\nIt makes it easier to catch wild Pokémon at night."
                },
                {
                    GameCulture.French,
                    "Un Poké Ball quelque peu différent.\nIl est plus facile d'attraper les Pokémon pendant la nuit."
                }
            },
            Item.sellPrice(gold: 2, silver: 20), ItemRarityID.White, Constants.Pokeballs.CatchRates.DUSK_BALL,
            new Color(130, 224, 99))
        {
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("DuskBallCap"));
            recipe.AddIngredient(mod.ItemType("Button"));
            recipe.AddIngredient(mod.ItemType("DuskBallBase"));
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(mod.ItemType("BlackApricorn"), 2);
            recipe2.AddIngredient(mod.ItemType("GreenApricorn"), 2);
            recipe2.AddIngredient(ItemID.SoulofNight);
            recipe2.AddRecipeGroup("IronBar", 6);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.value = 60000;
        }
    }
}