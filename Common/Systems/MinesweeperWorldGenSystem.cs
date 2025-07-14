using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terrasweeper.Common.Configs;

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
            tasks.Insert(tasks.Count-1, new WorldgenMinesPass("Terrasweeper Add mines", 100f));
        }
    }

    public class WorldgenMinesPass : GenPass
    {
        public WorldgenMinesPass(string name, float loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = MinesweeperWorldGenSystem.WorldgenMinesPassMessage.Value;

            // Main world generation code here
            if (Config.C.MinesEverywhere)
                PlaceMinesEverywhere();
            else
                PlaceMinesInSelectParts();

            // Place everything else
            progress.Message = MinesweeperWorldGenSystem.WorldgenHintTilesPassMessage.Value;
        }

        private void PlaceMinesEverywhere()
        {
            int minesAdded = 0; // testing code
            for (int j = 0; j < Main.maxTilesY; j++)
            {
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (!JJUtils.IsTileSolidForMine(tile))
                    {
                        continue;
                    }
                    ref var data = ref tile.Get<MinesweeperData>();
                    bool hasMine = WorldGen.genRand.Next(100) < Config.C.MineSpawnChance;
                    if (hasMine)
                    {
                        data.MineStatus = MineStatus.UnsolvedMine;
                        MinesweeperData.UpdateNumbersOfMines3x3(i, j);
                        minesAdded++; // testing code
                    }

                }
            }
            Log.Info("Total everywhere Mines added: " + minesAdded); // testing code
        }

        private void PlaceMinesInSelectParts()
        {
            int minesAdded = 0; // testing code
            int startX = Main.spawnTileX - 16;
            int endX = Main.spawnTileX + 16;
            int startY = Main.spawnTileY;
            int endY = startY + 100;

            for (int j = startY; j < endY && j < Main.maxTilesY; j++)
            {
                for (int i = startX; i <= endX && i < Main.maxTilesX; i++)
                {
                    if (i < 0) continue; // Prevent out-of-bounds
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (!JJUtils.IsTileSolidForMine(tile))
                    {
                        continue;
                    }
                    ref var data = ref tile.Get<MinesweeperData>();
                    bool hasMine = WorldGen.genRand.Next(100) < Config.C.MineSpawnChance;
                    if (hasMine)
                    {
                        data.MineStatus = MineStatus.UnsolvedMine;
                        MinesweeperData.UpdateNumbersOfMines3x3(i, j);
                        minesAdded++;
                    }
                }
            }
            Log.Info("Total select parts Mines added: " + minesAdded); // testing code
        }

        // sorry cotlim
        private int MakeMineRatio()
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
