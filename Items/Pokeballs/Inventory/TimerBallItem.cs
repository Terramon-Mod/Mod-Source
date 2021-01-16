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
    public sealed class TimerBallItem : BaseThrowablePokeballItem<TimerBallProjectile>
    {
        public TimerBallItem() : base(Constants.Pokeballs.UnlocalizedNames.QUICK_BALL,
            new Dictionary<GameCulture, string>
            {
                {GameCulture.English, "Timer Ball"},
                {GameCulture.French, "Chrono Ball"}
            },
            new Dictionary<GameCulture, string>
            {
                {
                    GameCulture.English,
                    "A somewhat different Poké Ball." +
                    "\nIt becomes progressively better the more Balls used on a wild Pokémon."
                },
                {
                    GameCulture.French,
                    "Un Poké Ball un peu différent." +
                    "\nIl devient progressivement meilleur au fur et à mesure que l'on utilise des balles sur un Pokémon sauvage."
                }
            },
            Item.sellPrice(gold: 6, silver: 25), ItemRarityID.White, Constants.Pokeballs.CatchRates.QUICK_BALL)
        {
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TimerBallCap"));
            recipe.AddIngredient(mod.ItemType("Button"));
            recipe.AddIngredient(mod.ItemType("PokeballBase"));
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(mod.ItemType("RedApricorn"), 2);
            recipe2.AddIngredient(mod.ItemType("BlackApricorn"), 2);
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
                    line2.overrideColor = new Color(255, 163, 71);
        }
    }
}