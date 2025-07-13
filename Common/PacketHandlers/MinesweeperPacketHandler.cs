using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Common.Systems;

namespace Terrasweeper.Common.PacketHandlers
{
    internal class MinesweeperPacketHandler : BasePacketHandler
    {
        public const byte SyncSingleTile = 0;
        public const byte ResyncRequest = 1;
        public const byte RegionCompressedData = 2;

        private readonly Dictionary<int, ChunkBuffer> chunkBuffers = new();

        public MinesweeperPacketHandler(byte handlerType) : base(handlerType) { }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case SyncSingleTile:
                    ReceiveSingleTile(reader, fromWho);
                    break;
                case ResyncRequest:
                    ReceiveResyncRequest(reader, fromWho);
                    break;
                case RegionCompressedData:
                    ReceiveRegionCompressed(reader, fromWho);
                    break;
            }
        }


        // --- SINGLE TILE ---

        public void SendSingleTile(int x, int y, int toClient = -1, int ignoreClient = -1)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
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
            {
                SendSingleTile(x, y, -1, fromWho);
            }

        }

        // Only MP client
        public void SendResyncRequest(int x, int y, int width, int height)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                return;
            }
            ModPacket packet = GetPacket(ResyncRequest);
            packet.Write((ushort)x);
            packet.Write((ushort)y);
            packet.Write((ushort)width);
            packet.Write((ushort)height);
            packet.Send();
        }

        // Only Server
        private void ReceiveResyncRequest(BinaryReader reader, int fromWho)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                return;
            }
            int xStart = reader.ReadUInt16();
            int yStart = reader.ReadUInt16();
            int width = reader.ReadUInt16();
            int height = reader.ReadUInt16();

            SendCompressedRegion(xStart, yStart, width, height, fromWho);
        }

        // Only Server
        private void SendCompressedRegion(int xStart, int yStart, int width, int height, int toClient)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                return;
            }
            using MemoryStream stream = new();
            using (DeflateStream deflate = new(stream, CompressionLevel.Fastest, leaveOpen: true))
            using (BinaryWriter writer = new(deflate, Encoding.UTF8))
            {
                writer.Write((byte)1); // version
                writer.Write((ushort)xStart);
                writer.Write((ushort)yStart);
                writer.Write((ushort)width);
                writer.Write((ushort)height);

                for (int y = yStart; y < yStart + height; y++)
                    for (int x = xStart; x < xStart + width; x++)
                    {
                        writer.Write(Main.tile[x, y].Get<MinesweeperData>().data);
                    }
            }

            byte[] compressed = stream.ToArray();
            ModPacket packet = GetPacket(RegionCompressedData);
            packet.Write((ushort)compressed.Length);
            packet.Write(compressed);
            packet.Send(toClient);
        }

        // Only MP client
        private void ReceiveRegionCompressed(BinaryReader reader, int fromWho)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                return;
            }
            int length = reader.ReadUInt16();
            byte[] compressed = reader.ReadBytes(length);

            using MemoryStream stream = new(compressed);
            using DeflateStream deflate = new(stream, CompressionMode.Decompress);
            using BinaryReader r = new(deflate, Encoding.UTF8);

            byte version = r.ReadByte();
            if (version != 1)
            {
                Log.Warn("Unknown region version");
                return;
            }

            int xStart = r.ReadUInt16();
            int yStart = r.ReadUInt16();
            int width = r.ReadUInt16();
            int height = r.ReadUInt16();

            for (int y = yStart; y < yStart + height; y++)
                for (int x = xStart; x < xStart + width; x++)
                {
                    byte value = r.ReadByte();
                    Main.tile[x, y].Get<MinesweeperData>().data = value;
                }

            Log.Info($"Received and applied MinesweeperData for region {xStart},{yStart} {width}x{height}");
        }


    }
    public class ChunkBuffer
    {
        public int ExpectedChunks;
        public byte[][] Chunks;
        public int ReceivedCount;

        public ChunkBuffer(int expectedChunks)
        {
            ExpectedChunks = expectedChunks;
            Chunks = new byte[expectedChunks][];
        }

        public void AddChunk(int index, byte[] data)
        {
            if (Chunks[index] == null)
            {
                Chunks[index] = data;
                ReceivedCount++;
            }
        }

        public bool IsComplete => ReceivedCount >= ExpectedChunks;

        public byte[] Combine()
        {
            using MemoryStream ms = new();
            for (int i = 0; i < ExpectedChunks; i++)
                ms.Write(Chunks[i], 0, Chunks[i].Length);
            return ms.ToArray();
        }
    }


}
