using System.IO;

namespace JulyJam.Common.PacketHandlers
{
    internal class ModNetHandler
    {
        // Here we define the packet types we will be using
        public const byte Minesweeper = 1;
        internal static MinesweeperPacketHandler minesweeperPacketHandler = new(Minesweeper);

        public static void HandlePacket(BinaryReader r, int fromWho)
        {
            // Here we read the packet type and call the appropriate handler
            switch (r.ReadByte())
            {
                case Minesweeper:
                    minesweeperPacketHandler.HandlePacket(r, fromWho);
                    break;
                default:
                    Log.Warn("Unknown packet type: " + r.ReadByte());
                    break;
            }
        }
    }
}