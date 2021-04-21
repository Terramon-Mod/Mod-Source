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
    public class MasterBallItem : BaseThrowablePokeballItem<MasterBallProjectile>
    {
        public MasterBallItem() : base(Constants.Pokeballs.UnlocalizedNames.MASTER_BALL,
            new Dictionary<GameCulture, string>
            {
                {GameCulture.English, "Master Ball"},
                {GameCulture.French, "Master Ball"}
            },
            new Dictionary<GameCulture, string>
            {
                {
                    GameCulture.English,
                    "The very best Ball with the ultimate performance.\nIt will catch any wild Pokémon without fail."
                },
                {
                    GameCulture.French,
                    "La meilleure balle avec une performance ultime.\nElle attrapera sans faute n'importe quel Pokémon sauvage."
                }
            },
            Item.sellPrice(), ItemRarityID.White, Constants.Pokeballs.CatchRates.MASTER_BALL, new Color(245, 83, 218))
        {
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("MasterBallCap"));
            recipe.AddIngredient(mod.ItemType("Button"));
            recipe.AddIngredient(mod.ItemType("PokeballBase"));
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.LunarBar, 7);
            recipe2.AddIngredient(mod.ItemType("RedApricorn"), 4);
            recipe2.AddIngredient(mod.ItemType("BlueApricorn"), 4);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        protected override void PostPokeballThrown(TerramonPlayer terramonPlayer, int thrownPokeballsCount)
        {
            /*compatibility.GrantAchievementLocal<UltraTossAchievement>(terramonPlayer.player);

            if (thrownPokeballsCount >= 25)
                compatibility.GrantAchievementLocal<ALotOfUltraTossesAchievement>(terramonPlayer.player);*/
        }

        public override bool CanBurnInLava()
        {
            return false;
        }
    }
}