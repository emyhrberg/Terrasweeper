using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace JulyJam.Common.Systems
{
    public class MinesweeperWorldGen : ModSystem
    {
        public static LocalizedText WorldgenMinesPassMessage { get; private set; }

        public override void SetStaticDefaults()
        {
            WorldgenMinesPassMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"Worldgen.{nameof(WorldgenMinesPassMessage)}"));
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            tasks.Insert(tasks.Count, new WorldgenMinesPass("JulyJam Add mines", 100f));
        }
    }

    public class WorldgenMinesPass : GenPass
    {
        public WorldgenMinesPass(string name, float loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = MinesweeperWorldGen.WorldgenMinesPassMessage.Value;

            int spawnX = Main.spawnTileX - 5;
            int spawnY = Main.spawnTileY;
            int numMines = 10;

            // Place mines
            for(int i = 0; i < numMines; i++)
            {
                int x = Main.rand.Next(11);
                int y = Main.rand.Next(11);
                if (Main.tile[spawnX + x, spawnY + y].type != TileID.BorealWood)
                {
                    WorldGen.PlaceTile(spawnX + x, spawnY + y, TileID.BorealWood, forced: true);
                }
                else continue;
            }

            // Place everything else
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
            ];
            for (int j = 0; j < 11; j++)
            {
                for (int i = 0; i < 11; i++)
                {
                    if (Main.tile[spawnX + i, spawnY + j].type != TileID.BorealWood) // not a mine
                    {
                        int minesAround = 0;
                        for(int j2 = -1; j2 <= 1; j2++)
                        {
                            for(int i2 = -1; i2 <= 1; i2++)
                            {
                                // add InWorld check later
                                if (Main.tile[spawnX + i + i2, spawnY + j + j2].type == TileID.BorealWood)
                                {
                                    minesAround += 1;
                                }
                            }
                        }
                        WorldGen.PlaceTile(spawnX + i, spawnY + j, hintTiles[minesAround], forced: true);
                    }
                }

            }
        }
    }
}
