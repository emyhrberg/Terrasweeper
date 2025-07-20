using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Common.PacketHandlers;

namespace Terrasweeper.Common.Systems
{
    /// <summary> Helper that can raise or lower the mine density without regenerating the whole board. </summary>
    public static class MinesweeperWorldHelper
    {
        /// <param name="minesPer100Tiles">
        ///     Slider value from the config (e.g. 12 → “12 mines per 100 tiles” = 12 % of adjustable cells).
        /// </param>
        public static void AdjustMineDensity(float minesPer100Tiles)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;                           // safety: run only on server / single-player host

            List<Point16> addCandidates = new();
            List<Point16> removeCandidates = new();

            // ── 1. Walk the board once, classifying cells ────────────────────────────
            for (short x = 0; x < Main.maxTilesX; x++)
            {
                for (short y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    ref var data = ref tile.Get<MinesweeperData>();

                    bool cleared = data.HasFlag ||
                                     data.MineStatus is MineStatus.Solved or MineStatus.Failed;

                    if (cleared)      // never modify player-cleared tiles
                        continue;

                    if (data.MineStatus == MineStatus.UnsolvedMine)
                        removeCandidates.Add(new Point16(x, y));     // present mine, could be removed
                    else
                        addCandidates.Add(new Point16(x, y));        // empty, could receive a mine
                }
            }

            int adjustableCells = addCandidates.Count + removeCandidates.Count;
            int currentMines = removeCandidates.Count;
            int targetMines = (int)Math.Round(adjustableCells * (minesPer100Tiles / 100f));

            // ── 2a. Remove mines if the ratio went down ─────────────────────────────
            if (targetMines < currentMines)
            {
                int toRemove = currentMines - targetMines;
                Shuffle(removeCandidates);

                for (int i = 0; i < toRemove && i < removeCandidates.Count; i++)
                    Demine(removeCandidates[i]);
            }
            // ── 2b. Add mines if the ratio went up ──────────────────────────────────
            else if (targetMines > currentMines)
            {
                int toAdd = targetMines - currentMines;
                Shuffle(addCandidates);

                for (int i = 0; i < toAdd && i < addCandidates.Count; i++)
                    ArmMine(addCandidates[i]);
            }

            ModContent.GetInstance<Terrasweeper>().Logger.Info(
                $"[Minesweeper] Adjusted mines: {currentMines} → {targetMines} " +
                $"(among {adjustableCells} adjustable cells).");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        private static void Demine(Point16 p)
        {
            ref var data = ref Main.tile[p.X, p.Y].Get<MinesweeperData>();
            data.ClearMineFlagData();                                // sets MineStatus = None
            MinesweeperData.UpdateNumbersOfMines3x3(p.X, p.Y);       // patch neighbour hints
            ModNetHandler.minesweeperPacketHandler.SendSingleTile(p.X, p.Y);
        }

        private static void ArmMine(Point16 p)
        {
            ref var data = ref Main.tile[p.X, p.Y].Get<MinesweeperData>();
            data.MineStatus = MineStatus.UnsolvedMine;
            MinesweeperData.UpdateNumbersOfMines3x3(p.X, p.Y);
            ModNetHandler.minesweeperPacketHandler.SendSingleTile(p.X, p.Y);
        }

        /// <summary> In-place Fisher–Yates shuffle that uses Terraria’s RNG. </summary>
        private static void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Main.rand.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
