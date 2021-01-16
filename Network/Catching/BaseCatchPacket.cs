using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Linq;
using Terramon.Items.Pokeballs.Inventory;
using Terramon.Pokemon;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Terramon.Network.Catching
{
    public class BaseCatchPacket : Packet
    {
        public const string NAME = "net_basecat";
        public override string PacketName => NAME;

        public void Send(TerramonMod mod, int type, string name, Rectangle rect, int pokeType, bool shiny = false)
        {
            try
            {
                var packet = GetPacket(mod);
                packet.Write(type);
                packet.Write(name);
                packet.Write(shiny);
                packet.Write(rect.X);
                packet.Write(rect.Y);
                packet.Write(rect.Width);
                packet.Write(rect.Height);
                packet.Write(pokeType);
                packet.Write(false);//v3
                packet.Send(256);
            }
            catch (Exception e)
            {
                mod.Logger.ErrorFormat("Please report this stacktrace to Terramon devs:\n\n{0}\n\n{1}", e.Message,
                    e.StackTrace);
            }
        }

        public void Send(TerramonMod mod, string type, string name, Rectangle rect, int pokeType, PokemonData data)
        {
            try
            {
                var packet = GetPacket(mod);
                packet.Write(type);
                packet.Write(name);
                packet.Write(data.IsShiny);
                packet.Write(rect.X);
                packet.Write(rect.Y);
                packet.Write(rect.Width);
                packet.Write(rect.Height);
                packet.Write(pokeType);
                packet.Write(true);//v3
                packet.Write(data);
                packet.Send(256);
            }
            catch (Exception e)
            {
                mod.Logger.ErrorFormat("Please report this stacktrace to Terramon devs:\n\n{0}\n\n{1}", e.Message,
                    e.StackTrace);
            }
        }

        public void Send(TerramonMod mod, string type, string name, Rectangle rect, int pokeType, bool shiny = false)
        {
            try
            {
                var packet = GetPacket(mod);
                packet.Write(type);
                packet.Write(name);
                packet.Write(shiny);
                //packet.Write("v2");
                packet.Write(rect.X);
                packet.Write(rect.Y);
                packet.Write(rect.Width);
                packet.Write(rect.Height);
                packet.Write(pokeType);
                packet.Write(false);//v3
                packet.Send(256);
            }
            catch (Exception e)
            {
                mod.Logger.ErrorFormat("Please report this stacktrace to Terramon devs:\n\n{0}\n\n{1}", e.Message,
                    e.StackTrace);
            }
        }

        public override void HandleFromClient(BinaryReader r, int whoAmI)
        {
            try
            {
                if (!Main.player[whoAmI].active)
                    return;

                string type = r.ReadString();
                BaseCaughtClass.det_CapturedPokemon = type;
                BaseCaughtClass.det_PokemonName = r.ReadString();
                BaseCaughtClass.det_isShiny = r.ReadBoolean();
                //string t = r.ReadString();
                //if(t != "v2")
                //    PokeballCaught.det_SmallSpritePath = t;
                //else
                //{
                //    var mon = TerramonMod.GetPokemon(type);
                //    PokeballCaught.det_SmallSpritePath = mon.IconName;
                //}


                var rect = new Rectangle(r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32());
                var typeID = r.ReadInt32();
                if (r.ReadBoolean())
                {
                    BaseCaughtClass.det_Data = r.ReadPokeData();
                }


                int index = Item.NewItem(rect, typeID);

                if (index >= 400 || !(Main.item[index].modItem is PokeballCaught modItem))
                    return;
            }
            catch (Exception e)
            {
                TerramonMod.Instance.Logger.ErrorFormat("Please report this stacktrace to Terramon devs:\n\n{0}\n\n{1}",
                    e.Message, e.StackTrace);
            }
        }

    }
}