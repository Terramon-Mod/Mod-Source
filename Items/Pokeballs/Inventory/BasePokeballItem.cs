using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terramon.Items.Pokeballs.Inventory
{
    public abstract class BasePokeballItem : TerramonItem
    {
        protected BasePokeballItem(string unlocalizedName, Dictionary<GameCulture, string> displayNames,
            Dictionary<GameCulture, string> tooltips, int value, int rarity, float catchRate,
            Color? nameColorOverride = null) :
            base(displayNames, tooltips, 24, 24, value, 0, rarity)
        {
            UnlocalizedName = unlocalizedName;

            CatchRate = catchRate;

            NameColorOverride = nameColorOverride;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.useTime = 16;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useAnimation = item.useTime;
            item.UseSound = SoundID.Item1;
            item.damage = 1;

            item.scale = 1f;
            item.maxStack = 99;
        }


        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            tooltips.RemoveAll(l => l.Name == "Damage");
            tooltips.RemoveAll(l => l.Name == "CritChance");
            tooltips.RemoveAll(l => l.Name == "Speed");
            tooltips.RemoveAll(l => l.Name == "Knockback");

            if (NameColorOverride != null)
                tooltips.Find(t => t.Name == "ItemName").overrideColor = NameColorOverride;
        }


        public string UnlocalizedName { get; }

        public float CatchRate { get; }

        public Color? NameColorOverride { get; }
    }
}