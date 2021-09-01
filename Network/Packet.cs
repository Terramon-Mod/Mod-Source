using System.Diagnostics.Eventing.Reader;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Terramon.Network
{
    public abstract class Packet
    {
        public abstract string PacketName { get; }

        public ModPacket GetPacket(TerramonMod mod)
        {
            var packet = mod.GetPacket();
            packet.Write(PacketName);
            return packet;
        }

        public ModPacket GetPacket()
        {
            var packet = TerramonMod.Instance.GetPacket();
            packet.Write(PacketName);
            return packet;
        }

        public virtual void HandleFromClient(BinaryReader reader, int whoAmI)
        {
        }

        //We don't need whoAmI because server id all ways same and equal 256
        public virtual void HandleFromServer(BinaryReader reader)
        {
        }
    }
}