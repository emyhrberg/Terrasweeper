using JulyJam.Common.PacketHandlers;
using System.IO;
using Terraria.ModLoader;

namespace JulyJam
{
    public class JulyJam : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModNetHandler.HandlePacket(reader, whoAmI);
        }
    }
}
