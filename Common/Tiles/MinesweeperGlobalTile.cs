using JulyJam.Common.Systems;
using JulyJam.Content.Projectiles;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace JulyJam.Common.Tiles;

/// <summary>Automatically kills the matching tile-entity when its host tile is broken.</summary>
public class MineFlagGlobalTile : GlobalTile
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
        if (data.HasMine && !data.HasFlag)
        {
            data.ClearMineFlagData();
            TriggerMine(i, j);
            MinesweeperData.UpdateNumbersOfMines3x3(i, j);
        }
        else if (data.HasFlag && !data.HasMine)
        {
            data.ClearMineFlagData();
            TriggerMine(i, j);
            MinesweeperData.UpdateNumbersOfMines3x3(i, j);
        }
        else if (data.HasMine && data.HasFlag)
        {
            // if the tile has a mine and a flag, we just remove the flag
            data.ClearMineFlagData();
            MinesweeperData.UpdateNumbersOfMines3x3(i, j);
        }
    }

    private static void TriggerMine(int i, int j)
    {
        // centre of the tile in world coords
        Vector2 centre = new((i + 0.5f) * 16f, (j + 0.5f) * 16f);

        Projectile.NewProjectile(
            new EntitySource_TileInteraction(Main.LocalPlayer, i, j),
            centre,
            Vector2.Zero,
            ModContent.ProjectileType<MineExplosion>(),
            50, 3f, 255);
    }
}
