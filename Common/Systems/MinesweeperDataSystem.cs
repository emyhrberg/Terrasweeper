using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace JulyJam.Common.Systems
{
    public struct MinesweeperData : ITileData
    {
        // n - Number of mines in the 3x3 area around this tile
        // m - Whether this tile is a mine
        // f - Whether this tile is flagged by the player
        // nnnnnmf0
        public byte data;
        public bool HasMine { get => TileDataPacking.GetBit(data, 2); 
            set => data = (byte)TileDataPacking.SetBit(value, data, 2); }
        public bool HasFlag
        {
            get => TileDataPacking.GetBit(data, 1);
            set => data = (byte)TileDataPacking.SetBit(value, data, 1);
        }
        public int NumMines
        {
            get => (int)data >> 3;
            set => data = (byte)(value << 3);
        }

        public void Clear()
        {
            data = 0; // clear all bits
        }
    }
    class MinesweeperDataSystem : ModSystem
    {
        public override void SaveWorldData(TagCompound tag)
        {
            using MemoryStream data = new(Main.maxTilesX);
            // 'fastest' compression level is likely good enough
            using (DeflateStream ds = new(data, CompressionLevel.Fastest, leaveOpen: true))
            using (BinaryWriter writer = new(ds, Encoding.UTF8))
            {
                writer.Write((byte)0); // version just in case 
                                       // if MyTileData is updated, update this 'version' number 
                                       // and add handling logic in LoadWorldData for backwards compat
                writer.Write(checked((ushort)Main.maxTilesX));
                writer.Write(checked((ushort)Main.maxTilesY));
                if (Unsafe.SizeOf<MinesweeperData>() != 1)
                    throw new Exception("This method only works for ITileDatas whose size is 1");
                writer.Write(MemoryMarshal.Cast<MinesweeperData, byte>(Main.tile.GetData<MinesweeperData>()));
            }
            tag["MinesweeperData"] = data.ToArray();
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.TryGet("MinesweeperData", out byte[] data))
            {
                using (BinaryReader reader = new(new DeflateStream(new MemoryStream(data), CompressionMode.Decompress), Encoding.UTF8))
                {
                    byte version = reader.ReadByte();
                    if (version == 0)
                    {
                        int width = reader.ReadUInt16();
                        int height = reader.ReadUInt16();
                        if (width != Main.maxTilesX || height != Main.maxTilesY)
                        {
                            // the world was somehow resized
                            // up to you what to do here 
                            throw new NotImplementedException("World size was changed");
                        }
                        else
                        {
                            if (Unsafe.SizeOf<MinesweeperData>() != 1)
                                throw new Exception("This method only works for ITileDatas whose size is 1");
                            var worldData = MemoryMarshal.Cast<MinesweeperData, byte>(Main.tile.GetData<MinesweeperData>());
                            reader.BaseStream.ReadExactly(worldData);
                        }
                    }
                    // add more else-ifs for newer versions of the data
                    else
                    {
                        throw new Exception("Unknown world data saved version");
                    }
                }
            }
            else
            {
                Log.Warn("No MinesweeperData found in world data. This is normal for new worlds or worlds that haven't been played yet.");
            }
        }
    }
}
