using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terramon.Items.Pokeballs.Thrown;
using Terramon.Players;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terramon.Items.Pokeballs.Inventory
{
    public abstract class BaseThrowablePokeballItem<T> : BasePokeballItem where T : BasePokeballProjectile
    {
        protected BaseThrowablePokeballItem(string unlocalizedName, Dictionary<GameCulture, string> displayNames,
            Dictionary<GameCulture, string> tooltips, int value, int rarity, float catchRate,
            Color? nameColorOverride = null) :
            base(unlocalizedName, displayNames, tooltips, value, rarity, catchRate, nameColorOverride)
        {
        }


        public override void SetDefaults()
        {
            base.SetDefaults();

            item.shoot = ModContent.ProjectileType<T>();
            item.shootSpeed = 10f;

            item.ranged = true;
            item.autoReuse = false;
            item.consumable = true;
            item.noUseGraphic = true;
        }


        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY,
            ref int type, ref int damage, ref float knockBack)
        {
            TerramonPlayer terramonPlayer = TerramonPlayer.Get(player);

            OnPokeballThrown(terramonPlayer);
            PostPokeballThrown(terramonPlayer, terramonPlayer.GetThrownPokeballsCount(this));

            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        protected virtual void OnPokeballThrown(TerramonPlayer terramonPlayer)
        {
            terramonPlayer.IncrementThrownPokeballs(this);
        }

        protected virtual void PostPokeballThrown(TerramonPlayer terramonPlayer, int thrownPokeballsCount)
        {
        }
    }
}