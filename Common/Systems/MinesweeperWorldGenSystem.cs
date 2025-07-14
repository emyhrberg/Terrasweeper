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
            tasks.Insert(tasks.Count, new WorldgenMinesPass("Terrasweeper Add mines", 100f));
        }
    }

    public class WorldgenMinesPass : GenPass
    {
        public WorldgenMinesPass(string name, float loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = MinesweeperWorldGenSystem.WorldgenMinesPassMessage.Value;

            // int ratioOfMines = Conf.C.ratioOfMines;
            int ratioOfMines = MakeMineRatio();

            // Place mines
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
                    //bool hasMine = Main.rand.Next(100) < ratioOfMines;
                    bool hasMine = WorldGen.genRand.Next(100) < ratioOfMines;
                    if (hasMine)
                    {
                        data.MineStatus = MineStatus.UnsolvedMine;
                        MinesweeperData.UpdateNumbersOfMines3x3(i, j);
                        minesAdded++; // testing code
                    }

                }
            }

            // Place everything else
            progress.Message = MinesweeperWorldGenSystem.WorldgenHintTilesPassMessage.Value;
            Log.Info("Mines added: " + minesAdded); // testing code
        }

        private int MakeMineRatio()
        {
            int value = 12; // Baseline value, Normal or Journey, medium, no special seed
            if (Main.expertMode) value += 3; // 15 mines for Expert
            if (Main.masterMode) value += 2; // 17 mines for Master
            if (Main.noTrapsWorld) value += 7; // +7 for no traps
            if (Main.getGoodWorld)
            {
                if (Main.masterMode) value += 4; // a whopping im tired of counting for legendary
                else value += 3; // or a +3 on other difficulties
            }
            if (Main.zenithWorld) value += 3; // another +3 for zenith
            return value; // Value can go from 10 (Journey/Classic, Small, no special seed) to 48 (Master, Large, Zenith)
        }
    }
}
