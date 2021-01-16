using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terramon.Items.Pokeballs.Thrown;
using Terramon.Players;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terramon.Items.Pokeballs.Inventory
{
    public class UltraBallItem : BaseThrowablePokeballItem<UltraBallProjectile>
    {
        public UltraBallItem() : base(Constants.Pokeballs.UnlocalizedNames.ULTRA_BALL,
            new Dictionary<GameCulture, string>
            {
                {GameCulture.English, "Ultra Ball"},
                {GameCulture.French, "Hyper Ball"}
            },
            new Dictionary<GameCulture, string>
            {
                {
                    GameCulture.English,
                    "It's an ultra-performance Ball.\nProvides a higher Pokémon catch rate than a Great Ball."
                },
                {
                    GameCulture.French,
                    "C'est une balle ultra-performante.\nFournit un taux de capture de Pokémon supérieur à celui d'une Super Ball."
                }
            },
            Item.sellPrice(gold: 7, silver: 75), ItemRarityID.White, Constants.Pokeballs.CatchRates.ULTRA_BALL,
            new Color(245, 218, 83))
        {
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("UltraBallCap"));
            recipe.AddIngredient(mod.ItemType("Button"));
            recipe.AddIngredient(mod.ItemType("PokeballBase"));
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(mod.ItemType("BlackApricorn"), 3);
            recipe2.AddIngredient(mod.ItemType("YellowApricorn"), 1);
            recipe2.AddRecipeGroup("IronBar", 6);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 80000;
        }

        protected override void PostPokeballThrown(TerramonPlayer terramonPlayer, int thrownPokeballsCount)
        {
            /*compatibility.GrantAchievementLocal<UltraTossAchievement>(terramonPlayer.player);

            if (thrownPokeballsCount >= 25)
                compatibility.GrantAchievementLocal<ALotOfUltraTossesAchievement>(terramonPlayer.player);*/
        }
    }
}