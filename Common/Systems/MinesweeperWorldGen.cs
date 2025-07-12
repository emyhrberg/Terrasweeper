using JulyJam.Common.Configs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace JulyJam.Common.Systems
{
    public class MinesweeperWorldGen : ModSystem
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
            tasks.Insert(tasks.Count, new WorldgenMinesPass("JulyJam Add mines", 100f));
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
            Main.NewText("mines: " + data.NumMines);
        }
    }

    public class WorldgenMinesPass : GenPass
    {
        public WorldgenMinesPass(string name, float loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = MinesweeperWorldGen.WorldgenMinesPassMessage.Value;

            //int spawnX = Main.spawnTileX - 5;
            //int spawnY = Main.spawnTileY;
            int numMines = ModContent.GetInstance<MinesWorldGenConfig>().numMines;
            Math.Clamp(numMines, 0, (Main.maxTilesX * Main.maxTilesY) / 2);

            // Place mines
            for(int i = 0; i < numMines; i++)
            {
                int x = Main.rand.Next(Main.maxTilesX);
                int y = Main.rand.Next(Main.maxTilesY);
                if (JJUtils.CanPlaceMine(Main.tile[x, y]))
                {
                    //WorldGen.PlaceTile(x, y, TileID.BorealWood, forced: true); // testing code
                    ref var data = ref Main.tile[x, y].Get<MinesweeperData>();
                    data.HasMine = true;
                }
                else continue;
            }

            // Place everything else
            progress.Message = MinesweeperWorldGen.WorldgenHintTilesPassMessage.Value;

            /*//test code, delete on release
            int[] hintTiles = [
                TileID.Stone,           // 0 mines
                TileID.CopperBrick,     // 1 mine
                TileID.TinBrick,        // 2 mines
                TileID.IronBrick,       // 3 mines
                TileID.LeadBrick,       // 4 mines
                TileID.SilverBrick,     // 5 mines
                TileID.TungstenBrick,   // 6 mines
                TileID.GoldBrick,       // 7 mines
                TileID.PlatinumBrick    // 8 mines
            ];*/

            for (int j = 0; j < Main.maxTilesY; j++)
            {
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    ref var data = ref Main.tile[i, j].Get<MinesweeperData>();
                    if (JJUtils.CanPlaceMine(Main.tile[i, j]) && !data.HasMine) // not a mine
                    {
                        int minesAround = 0;
                        for(int j2 = -1; j2 <= 1; j2++)
                        {
                            for(int i2 = -1; i2 <= 1; i2++)
                            {
                                if(WorldGen.InWorld(i + i2, j + j2))
                                {
                                    ref var surroundingData = ref Main.tile[i + i2, j + j2].Get<MinesweeperData>();
                                    if (surroundingData.HasMine)
                                    {
                                        minesAround += 1;
                                    }
                                }
                            }
                        }
                        //if(minesAround > 0) WorldGen.PlaceTile(i, j, hintTiles[minesAround], forced: true); // testing code
                        data.NumMines = minesAround;
                    }
                }

            }
        }
    }
}
