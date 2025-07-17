using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Common.BuilderToggles;
using Terrasweeper.Common.PacketHandlers;
using Terrasweeper.Common.Systems;

namespace Terrasweeper.Common.Globals
{
    internal class PicaxeFlagingGlobalItem : GlobalItem
    {
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (item.pick <= 0 || ModContent.GetInstance<NumbersTransparencyBuilderToggle>().CurrentState == 2)
            {
                return false;
            }
            int i = Player.tileTargetX;
            int j = Player.tileTargetY;

            if (!JJUtils.WithinPlacementRange(player, i, j))
            {
                return false;
            }

            if (JJUtils.IsTileSolidForMine(i, j))
            {
                JJUtils.ToggleFlagState(i, j);
                ItemID.Sets.ItemsThatAllowRepeatedRightClick[item.type] = false;
            }
            else // Mining in 3x3 if amount of flags / revealed mines == number on tile
            {
                Tile tile = Framing.GetTileSafely(i, j);
                if (JJUtils.IsTileSolidForNumbers(tile))
                {
                    return false;
                }
                ref var data = ref tile.Get<MinesweeperData>();
                if (data.HasOrAtLeastHadMine)
                {
                    return false;
                }


                int countOfRevealedMinesAndFlaggedTilesArroundNumberedTile = 0;
                for (int si = i - 1; si <= i + 1; si++)
                {
                    for (int sj = j - 1; sj <= j + 1; sj++)
                    {
                        Tile tileSafely = Framing.GetTileSafely(si, sj);
                        ref var subData = ref tileSafely.Get<MinesweeperData>();

                        if (
                            (subData.HasOrAtLeastHadMine && subData.MineStatus != MineStatus.UnsolvedMine) || 
                            (subData.HasFlag && JJUtils.IsTileSolidForMine(tileSafely))
                            )
                        {
                            countOfRevealedMinesAndFlaggedTilesArroundNumberedTile++;
                        }
                    }
                }
                if (data.TileNumber != countOfRevealedMinesAndFlaggedTilesArroundNumberedTile)
                {
                    return false;
                }
                ItemID.Sets.ItemsThatAllowRepeatedRightClick[item.type] = true;
                bool minedSomething = false;
                for (int si = i - 1; si <= i + 1; si++)
                {
                    for (int sj = j - 1; sj <= j + 1; sj++)
                    {
                        Tile tileSafely = Framing.GetTileSafely(si, sj);
                        Main.LocalPlayer.PickTile(si, sj, item.pick);
                        minedSomething = true;
                    }
                }
                if (minedSomething)
                {
                    ItemID.Sets.ItemsThatAllowRepeatedRightClick[item.type] = true;
                }
                return minedSomething;
            }



            return false;
        }

    }
}
