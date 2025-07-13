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
            Main.NewText("mines: " + data.TileNumber);
        }
    }

    public class WorldgenMinesPass : GenPass
    {
        public WorldgenMinesPass(string name, float loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = MinesweeperWorldGen.WorldgenMinesPassMessage.Value;

            int ratioOfMines = Conf.C.ratioOfMines;

            // Place mines
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
                    bool hasMine = Main.rand.Next(100) < ratioOfMines;
                    if (hasMine)
                    {
                        data.MineStatus = MineStatus.UnsolvedMine;
                        MinesweeperData.UpdateNumbersOfMines3x3(i, j);
                    }
                    
                }
            }

            // Place everything else
            progress.Message = MinesweeperWorldGen.WorldgenHintTilesPassMessage.Value;
        }
    }
}
