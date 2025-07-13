using JulyJam.Common.Systems;
using JulyJam.Content.Items;
using JulyJam.Content.Projectiles;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace JulyJam.Common.Globals;

public class MinesweeperGlobalTile : GlobalTile
{
    public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (fail || effectOnly) return; // tile survived or just an effect

        Tile tile = Main.tile[i, j];
        if (!JJUtils.IsTileSolid(tile))
        {
            return;
        }
        ref var data = ref tile.Get<MinesweeperData>();
        bool unsolvedMine = data.MineStatus == MineStatus.UnsolvedMine;
        if (unsolvedMine && !data.HasFlag)
        {
            data.ClearMineFlagData();
            data.MineStatus = MineStatus.Failed;
            JJUtils.Explode(i, j);
            MinesweeperData.UpdateNumbersOfMines3x3(i, j);
        }
        else if (data.HasFlag && !data.HasMine)
        {
            data.ClearMineFlagData();
            JJUtils.Explode(i, j);
            MinesweeperData.UpdateNumbersOfMines3x3(i, j);
        }
        else if (unsolvedMine && data.HasFlag)
        {
            data.ClearMineFlagData();
            data.MineStatus = MineStatus.Solved;
            Item.NewItem(
                new EntitySource_TileInteraction(Main.LocalPlayer, i, j),
                new Vector2(i * 16f, j * 16f),
                ModContent.ItemType<MinedMine>(),
                1);
            MinesweeperData.UpdateNumbersOfMines3x3(i, j);
        }
        else if (!unsolvedMine && data.HasMine && data.HasFlag)
        {
            data.HasFlag = false; // remove the flag
        }
    }
}
