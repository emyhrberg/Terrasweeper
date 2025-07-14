using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terrasweeper.Common.Systems;
using Terrasweeper.Content.Items;
using Terrasweeper.Content.Projectiles;

namespace Terrasweeper.Common.Globals;

public class MinesweeperGlobalTile : GlobalTile
{
    public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (fail || effectOnly) return; // tile survived or just an effect

        Tile tile = Main.tile[i, j];
        if (!JJUtils.IsTileSolidForMine(tile))
        {
            return;
        }
        ref var data = ref tile.Get<MinesweeperData>();
        bool unsolvedMine = data.MineStatus == MineStatus.UnsolvedMine;

        // Fail Mine
        if (unsolvedMine && !data.HasFlag)
        {
            data.ClearMineFlagData();
            data.MineStatus = MineStatus.Failed;
            JJUtils.Explode(i, j);
            MinesweeperData.UpdateNumbersOfMines3x3(i, j);
        }
        // Fail Flag
        else if (data.HasFlag && !data.HasOrAtLeastHadMine)
        {
            data.ClearMineFlagData();
            JJUtils.Explode(i, j);
            MinesweeperData.UpdateNumbersOfMines3x3(i, j);
        }
        // Solve Mine
        else if (unsolvedMine && data.HasFlag)
        {
            data.ClearMineFlagData();
            data.MineStatus = MineStatus.Solved;
            int item = Item.NewItem(
                new EntitySource_TileInteraction(Main.LocalPlayer, i, j),
                new Vector2(i * 16f, j * 16f),
                ModContent.ItemType<MinedMine>(),
                1, false);
            NetMessage.SendData(MessageID.SyncItem, number: item);


            MinesweeperData.UpdateNumbersOfMines3x3(i, j);
        }
        else if (!unsolvedMine && data.HasOrAtLeastHadMine && data.HasFlag)
        {
            data.HasFlag = false; // remove the flag
        }
    }

    public override bool CanReplace(int i, int j, int type, int tileTypeBeingPlaced)
    {
        Tile tile = Main.tile[i, j];
        ref var data = ref tile.Get<MinesweeperData>();
        if (data.data == 0)
        {
            return base.CanReplace(i, j, type, tileTypeBeingPlaced);
        }
        return JJUtils.IsTileSolidForMine(tileTypeBeingPlaced);
    }
}
