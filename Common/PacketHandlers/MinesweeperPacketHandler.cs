using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using JulyJam.Common.Systems;
using System.Collections.Generic;

namespace JulyJam.Common.PacketHandlers
{
    internal class MinesweeperPacketHandler : BasePacketHandler
    {
        public const byte SyncSingleTile = 0;
        public const byte SyncManyTiles = 1;

        public MinesweeperPacketHandler(byte handlerType) : base(handlerType) { }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case SyncSingleTile:
                    ReceiveSingleTile(reader, fromWho);
                    break;
                case SyncManyTiles:
                    ReceiveManyTiles(reader, fromWho);
                    break;
            }
        }

        // --- SINGLE TILE ---

        public void SendSingleTile(int x, int y, int toClient = -1, int ignoreClient = -1)
        {
            if(Main.netMode == NetmodeID.SinglePlayer)
                return;
            ModPacket packet = GetPacket(SyncSingleTile);
            packet.Write((short)x);
            packet.Write((short)y);
            packet.Write(Main.tile[x, y].Get<MinesweeperData>().data);
            packet.Send(toClient, ignoreClient);
        }

        private void ReceiveSingleTile(BinaryReader reader, int fromWho)
        {
            short x = reader.ReadInt16();
            short y = reader.ReadInt16();
            byte data = reader.ReadByte();

            Main.tile[x, y].Get<MinesweeperData>().data = data;

            if (Main.netMode == NetmodeID.Server)
                SendSingleTile(x, y, -1, fromWho);
        }

        // --- MANY TILES ---

        public void SendManyTiles(List<(int x, int y)> tiles, int toClient)
        {
            ModPacket packet = GetPacket(SyncManyTiles);
            packet.Write((ushort)tiles.Count);
            foreach (var (x, y) in tiles)
            {
                packet.Write((short)x);
                packet.Write((short)y);
                packet.Write(Main.tile[x, y].Get<MinesweeperData>().data);
            }
            packet.Send(toClient);
        }

        private void ReceiveManyTiles(BinaryReader reader, int fromWho)
        {
            ushort count = reader.ReadUInt16();
            for (int i = 0; i < count; i++)
            {
                short x = reader.ReadInt16();
                short y = reader.ReadInt16();
                byte data = reader.ReadByte();
                Main.tile[x, y].Get<MinesweeperData>().data = data;
            }
        }
    }
}
