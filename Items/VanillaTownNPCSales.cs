using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terramon.Items
{
    public class VanillaTownNPCSales : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Mechanic)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("LinkCable"));
                nextSlot++;
            }
        }
    }
}