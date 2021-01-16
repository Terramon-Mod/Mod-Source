using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terramon.Achievements;
using Terramon.Items.Pokeballs.Thrown;
using Terramon.Players;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Terramon.Items.Pokeballs.Inventory
{
    public class ZeroBallItem : BaseThrowablePokeballItem<ZeroBallProjectile>
    {
        public ZeroBallItem() : base(Constants.Pokeballs.UnlocalizedNames.POKE_BALL,
            new Dictionary<GameCulture, string>()
            {
                { GameCulture.English, "Zero Ball" },
                { GameCulture.French, "Zéro Ball" }
            },
            new Dictionary<GameCulture, string>()
            {
                { GameCulture.English, "A high-tech, upgraded Poké Ball.\nIt automatically targets the nearest Pokémon when thrown.\nWho needs good aim anyway?" },
                { GameCulture.French, "Une Poké Ball améliorée et de haute technologie.\nElle cible automatiquement le Pokémon le plus proche lorsqu'elle est lancée.\nQui a besoin d'une bonne visée de toute façon? " }
            },
            Item.sellPrice(gold: 4),
            ItemRarityID.White, Constants.Pokeballs.CatchRates.POKE_BALL, new Color(22, 100, 148))
        {
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ZeroBallCap"));
            recipe.AddIngredient(mod.ItemType("DarkButton"));
            recipe.AddIngredient(mod.ItemType("ZeroBallBase"));
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(mod.ItemType("BlackApricorn"), 4);
            recipe2.AddRecipeGroup("IronBar", 6);
            recipe2.AddIngredient(ItemID.Nanites, 12);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 50000;
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
