﻿using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terrasweeper.Common.Configs;
using Terrasweeper.Common.PacketHandlers;

namespace Terrasweeper.Common.Systems
{
    public class MinesweeperWorldGenSystem : ModSystem
    {
        public static LocalizedText WorldgenMinesPassMessage { get; private set; }
        public static LocalizedText WorldgenHintTilesPassMessage { get; private set; }

        public override void SetStaticDefaults()
        {
            WorldgenMinesPassMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"Worldgen.{nameof(WorldgenMinesPassMessage)}"));
            WorldgenHintTilesPassMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"Worldgen.{nameof(WorldgenHintTilesPassMessage)}"));
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            tasks.Insert(tasks.Count - 1, new WorldgenMinesPass("Terrasweeper Add mines", 100f));
        }
    }

    public class WorldgenMinesPass : GenPass
    {
        public WorldgenMinesPass(string name, float loadWeight) : base(name, loadWeight) { }

        public override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = MinesweeperWorldGenSystem.WorldgenMinesPassMessage.Value;

            int minesPer100Tiles;
            if (Config.C.CustomMinePer100TilesValue)
            {
                minesPer100Tiles = (int)Config.C.MinesPer100Tile;
            }
            else
            {
                minesPer100Tiles = MakeMineRatio();
            }
            PlaceMines(minesPer100Tiles);

            progress.Message = MinesweeperWorldGenSystem.WorldgenHintTilesPassMessage.Value;
        }
        public static void PlaceMines(int minesPer100Tiles)
        {
            // 1) wipe all existing Minesweeper info --------------------------
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    ref var data = ref tile.Get<MinesweeperData>();

                    if (data.MineStatus == MineStatus.UnsolvedMine)
                    {
                        data.data = 0;
                        MinesweeperData.UpdateNumbersOfMines3x3(x, y);
                    }
                }
            }

            // 2) place mines with the new ratio ------------------------------
            int minesAdded = 0;

            for (int y = 0; y < Main.maxTilesY; y++)
            {
                int actuallMinerPer100Tiles = minesPer100Tiles;
                DepthZone depthZone = JJUtils.GetDepthZone(y);
                if (depthZone == DepthZone.Sky)
                {
                    actuallMinerPer100Tiles = (int)(actuallMinerPer100Tiles * 0.5f);
                }else if(depthZone == DepthZone.Overworld)
                {
                    actuallMinerPer100Tiles = (int)(actuallMinerPer100Tiles * 0.75f);
                }

                    for (int x = 0; x < Main.maxTilesX; x++)
                    {
                        Tile tile = Framing.GetTileSafely(x, y);
                        if (!JJUtils.IsTileSolidForMine(tile))
                            continue;

                        ref var data = ref tile.Get<MinesweeperData>();
                    
                        if (JJUtils.GenRandNumber(x, y, Main.ActiveWorldFileData.Seed) <= actuallMinerPer100Tiles && 
                        data.MineStatus == MineStatus.None)
                        {
                            data.MineStatus = MineStatus.UnsolvedMine;
                            minesAdded++;
                            MinesweeperData.UpdateNumbersOfMines3x3(x, y);
                        }
                    }
            }

            Log.Info($"[Minesweeper] Re-generated world mines: {minesAdded} added, ratio {minesPer100Tiles}/100.");
        }

        public static int MakeMineRatio()
        {
            int value = 12; // Baseline value, Normal or Journey, medium, no special seed
            if (Main.expertMode) value += 3; // 15 mines for Expert
            if (Main.masterMode) value += 2; // 17 mines for Master
            if (Main.noTrapsWorld) value += 7; // +7 for no traps
            if (Main.getGoodWorld)
            {
                if (Main.masterMode) value += 4; // a whopping im tired of counting for legendary <-- lmao
                else value += 3; // or a +3 on other difficulties
            }
            if (Main.zenithWorld) value += 3; // another +3 for zenith
            return value; // Value can go from 10 (Journey/Classic, Small, no special seed) to 48 (Master, Large, Zenith)
        }
    }
}
