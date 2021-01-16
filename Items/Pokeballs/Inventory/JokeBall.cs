using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Items.Pokeballs.Inventory
{
	public class JokeBall : ModItem
	{
		public override void SetStaticDefaults()
		{
	    	DisplayName.SetDefault("Flop Ball");
			//Tooltip.SetDefault("get stick-bugged lol");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.maxStack = 16;
		}
	}
}