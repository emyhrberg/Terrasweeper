using System.IO;
using Terraria.ModLoader;
using Terrasweeper.Common.PacketHandlers;

namespace Terrasweeper
{
    public class Terrasweeper : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModNetHandler.HandlePacket(reader, whoAmI);
        }
    }
}
