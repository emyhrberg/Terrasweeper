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

        // testing-related methods, delete on release
        public static bool JustPressed(Keys key)
        {
            return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
        }

        public override void PostUpdateWorld()
        {
            if (JustPressed(Keys.NumPad5))
            {
                TestMethod((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16);
            }
        }

        private void TestMethod(int x, int y)
        {
            ref var data = ref Main.tile[x, y].Get<MinesweeperData>();
            Main.NewText("data: " + data.data);
            Main.NewText("mines: " + data.TileNumber);
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
            Console.WriteLine("Mine ratio: " + ratioOfMines); // testing code

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
                        //WorldGen.PlaceTile(i, j, TileID.BoneBlock, forced: true); // testing code
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
            if (Main.masterMode) value += 5; // 17 mines for Master
            if (Main.noTrapsWorld) value += 7; // +7 for no traps
            if (Main.getGoodWorld)
            {
                if (Main.masterMode) value += 7; // a whopping +12 (5+7) for legendary
                else value += 5; // or a +5 on other difficulties
            }
            if (Main.zenithWorld) value += 3; // another +3 for zenith
            return value; // Value can go from 10 (Journey/Classic, Small, no special seed) to 48 (Master, Large, Zenith)
        }
    }
}
