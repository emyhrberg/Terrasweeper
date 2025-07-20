using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terrasweeper.Common.PacketHandlers;

namespace Terrasweeper.Common.Systems
{
    public struct MinesweeperData : ITileData
    {
        // n - Number of mines in the 3x3 area around this tile
        // m - Enum of Mine status
        // f - Whether this tile is flagged by the player
        // nnnnmmf0
        public byte data;
        public bool HasFlag
        {
            get => TileDataPacking.GetBit(data, 1);
            set => data = (byte)TileDataPacking.SetBit(value, data, 1);
        }

        public MineStatus MineStatus
        {
            get => (MineStatus)TileDataPacking.Unpack(data, 2, 2);
            set => data = (byte)TileDataPacking.Pack((byte)value, data, 2, 2);
        }
        public byte TileNumber // from 0 to 9
        {
            get => (byte)TileDataPacking.Unpack(data, 4, 4);
            set => data = (byte)TileDataPacking.Pack(value, data, 4, 4);
        }
        public bool HasOrAtLeastHadMine => MineStatus != MineStatus.None;
        public void ClearMineFlagData()
        {
            MineStatus = MineStatus.None;
            HasFlag = false;
        }

        public void UpdateNumbersOfMines(int x, int y)
        {
            // update the number of mines around this tile
            int mineCount = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Tile neighborTile = Framing.GetTileSafely(x + i, y + j);
                    ref var data = ref neighborTile.Get<MinesweeperData>();
                    if (data.HasOrAtLeastHadMine)
                    {
                        mineCount++;
                    }
                }
            }
            if (mineCount > 9)
            {
                mineCount = 9;
                Log.Warn($"Mine count exceeded 9 at ({x}, {y}). Clamping to 9.");
            }
            TileNumber = (byte)mineCount;
        }

        public static void UpdateNumbersOfMines3x3(int x, int y)
        {
            // update the number of mines around this tile
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Tile neighborTile = Framing.GetTileSafely(x + i, y + j);
                    ref var data = ref neighborTile.Get<MinesweeperData>();
                    data.UpdateNumbersOfMines(x + i, y + j);
                    ModNetHandler.minesweeperPacketHandler.SendSingleTile(x + i, y + j);
                }
            }
        }
    }

    public enum MineStatus : byte
    {
        None = 0, // no mine
        UnsolvedMine = 1, // not yet solved by the player
        Solved = 2, // solved by the player
        Failed = 3 // failed by the player
    }
    class MinesweeperDataSystem : ModSystem
    {
        public override void SaveWorldData(TagCompound tag)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
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
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
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
