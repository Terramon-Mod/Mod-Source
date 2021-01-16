using Terraria.ModLoader;

namespace Terramon.Items.Evolution
{
    public class RareCandy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rare Candy");
            Tooltip.SetDefault("A candy that is packed with energy."
                               + "\nThey can be used to level up [c/FFFF66:Pokémon.]"
                               + "\nThe Pokémon Trainer might be able to help you use them.");
        }

        public override void SetDefaults()
        {
            item.width = 21;
            item.height = 21;
            item.scale = 1f;
            item.maxStack = 999;
            item.value = 3300;
            item.rare = 9;
        }
    }
}