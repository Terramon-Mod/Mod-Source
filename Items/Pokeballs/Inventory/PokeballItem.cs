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
    public class PokeballItem : BaseThrowablePokeballItem<PokeballProjectile>
    {
        public PokeballItem() : base(Constants.Pokeballs.UnlocalizedNames.POKE_BALL,
            new Dictionary<GameCulture, string>
            {
                {GameCulture.English, "Poke Ball"}
            },
            new Dictionary<GameCulture, string>
            {
                {
                    GameCulture.English,
                    "A device for catching wild Pokémon.\nIt is thrown like a ball at the target.\nIt is designed as a capsule system."
                },
                {
                    GameCulture.French,
                    "Un appareil pour attraper les Pokémon.\nIl est lancé comme une balle sur la cible.\nIl est utiliser comme un système de capsule."
                }
            },
            Item.sellPrice(silver: 65),
            ItemRarityID.White, Constants.Pokeballs.CatchRates.POKE_BALL, new Color(255, 87, 87))
        {
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("PokeballCap"));
            recipe.AddIngredient(mod.ItemType("Button"));
            recipe.AddIngredient(mod.ItemType("PokeballBase"));
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(mod.ItemType("RedApricorn"), 4);
            recipe2.AddRecipeGroup("IronBar", 6);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 30000;
            item.scale = 0.7f;
        }

        protected override void PostPokeballThrown(TerramonPlayer terramonPlayer, int thrownPokeballsCount)
        {
            /*compatibility.GrantAchievementLocal<FirstTossAchievement>(terramonPlayer.player);
            
            if (thrownPokeballsCount >= 25)
                compatibility.GrantAchievementLocal<ALotOfTossesAchievement>(terramonPlayer.player);*/
        }
    }
}