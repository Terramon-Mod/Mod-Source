using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terramon.Pokemon;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Terramon.Network
{
    public static class PacketExtensions
    {
        public static void Write(this ModPacket packet, TagCompound tag)
        {
            packet.Write(tag.Count);
            foreach (var it in tag)
            {
                switch (it.Value)
                {
                    case bool b:
                        packet.Write((byte)0);
                        packet.Write(it.Key);
                        packet.Write(b);
                        break;
                    case int i:
                        packet.Write((byte)1);
                        packet.Write(it.Key);
                        packet.Write(i);
                        break;
                    case float f:
                        packet.Write((byte)2);
                        packet.Write(it.Key);
                        packet.Write(f);
                        break;
                    case double d:
                        packet.Write((byte)3);
                        packet.Write(it.Key);
                        packet.Write(d);
                        break;
                    case string s:
                        packet.Write((byte)4);
                        packet.Write(it.Key);
                        packet.Write(s);
                        break;
                    case byte bt:
                        packet.Write((byte)5);
                        packet.Write(it.Key);
                        packet.Write(bt);
                        break;
                    case TagCompound ctag:
                        packet.Write((byte)6);
                        packet.Write(it.Key);
                        packet.Write(ctag);
                        break;
                }
            }
        }

        public static TagCompound ReadTag(this BinaryReader r)
        {
            var c = r.ReadInt32();
            var tag = new TagCompound();
            for (int i = 0; i < c; i++)
            {
                var t = r.ReadByte();
                switch (t)
                {
                    case 0://bool
                        tag.Add(r.ReadString(), r.ReadBoolean());
                        break;
                    case 1://int
                        tag.Add(r.ReadString(), r.ReadInt32());
                        break;
                    case 2://float
                        tag.Add(r.ReadString(), r.ReadSingle());
                        break;
                    case 3://double
                        tag.Add(r.ReadString(), r.ReadDouble());
                        break;
                    case 4://string
                        tag.Add(r.ReadString(), r.ReadString());
                        break;
                    case 5://byte
                        tag.Add(r.ReadString(), r.ReadByte());
                        break;
                    case 6://TagCompound
                        tag.Add(r.ReadString(), r.ReadTag());
                        break;
                }
            }

            return tag;
        }

        public static void Write(this ModPacket packet, PokemonData tag) => Write(packet, tag.GetCompound());
        public static PokemonData ReadPokeData(this BinaryReader r) => new PokemonData(ReadTag(r));

    }
}
