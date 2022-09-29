using Microsoft.Xna.Framework;
using Razorwing.RPC;
using Razorwing.RPC.Attributes;
using System;
using Terramon.Pokemon;
using Terraria;
using Terraria.ID;

namespace Terramon.Network
{
    public class NetWatcher
    {
        [RPCCallable]
        public static void WildToProjTransform(NPC npc, int pid)
        {
            if (npc.modNPC is ParentPokemonNPC pk)
            {
                //ParentPokemon.det_Wild = true;
                //var id = Projectile.NewProjectile(npc.position, Vector2.Zero,
                //    TerramonMod.Instance.ProjectileType(pk.HomeClass().Name), 0, 0);
                npc.active = false;
                //if (id != pid)
                //{
                //    Main.projectile[id].active = false;//Invalid sync
                //    //TODO: Make a workaround for this
                //    throw new InvalidOperationException("In output get wrong projectile ID. Consider mod as desynced");
                //}

            }
        }

        [RPCCallable]
        public static int RequestWildToProjTransform(Player sender, NPC npc)
        {
            if (npc.modNPC is ParentPokemonNPC pk)
            {
                ParentPokemon.det_Wild = true;
                var id = Projectile.NewProjectile(npc.position, Vector2.Zero,
                    TerramonMod.Instance.ProjectileType(pk.HomeClass().Name), 0, 0, sender.whoAmI);
                npc.active = false;
                if(Main.netMode != NetmodeID.SinglePlayer)
                    dev.Null.RPC(WildToProjTransform, npc, id);
                return id;
            }
            return -1;
        }

        //public static void SpawnStarter(string pokemon, Rectangle rect)
        //{
        //    BaseCaughtClass.writeDetour(nameof(Pokemon.FirstGeneration.Normal.Squirtle), "Squirtle",
        //        "Terramon/Minisprites/Regular/miniSquirtle");
        //    Item.NewItem(rect, ModContent.ItemType<PokeballCaught>());
        //}
        //public static void SpawnCatch(int whoAmI, string type, bool isShiny, 
        //    int typeID, Rectangle rect, Identity data)
        //{
        //    try
        //    {
        //        if (!Main.player[whoAmI].active)
        //            return;

        //        //string type = r.ReadString();
        //        BaseCaughtClass.det_CapturedPokemon = type;
        //        BaseCaughtClass.det_PokemonName = type;
        //        BaseCaughtClass.det_isShiny = isShiny;
        //        //string t = r.ReadString();
        //        //if(t != "v2")
        //        //    PokeballCaught.det_SmallSpritePath = t;
        //        //else
        //        //{
        //        //    var mon = TerramonMod.GetPokemon(type);
        //        //    PokeballCaught.det_SmallSpritePath = mon.IconName;
        //        //}


        //        //var rect = new Rectangle(r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32());
        //        //var typeID = r.ReadInt32();
        //        //if (r.ReadBoolean())
        //        //{
        //        BaseCaughtClass.det_Data = new PokemonData(data);
        //        //}


        //        int index = Item.NewItem(rect, typeID);

        //        if (index >= 400 || !(Main.item[index].modItem is PokeballCaught modItem))
        //            return;
        //    }
        //    catch (Exception e)
        //    {
        //        TerramonMod.Instance.Logger.ErrorFormat("Please report this stacktrace to Terramon devs:\n\n{0}\n\n{1}",
        //            e.Message, e.StackTrace);
        //    }
        //}

    }
}
