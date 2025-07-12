using JulyJam.Content.Projectiles;
using JulyJam.Content.TileEntities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JulyJam.Common.Tiles;

/// <summary>Automatically kills the matching tile-entity when its host tile is broken.</summary>
public class MineFlagGlobalTile : GlobalTile
{
    public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (fail || effectOnly) return; // tile survived or just an effect

        RemoveEntityIfPresent(i, j);
    }

    private static void RemoveEntityIfPresent(int i, int j)
    {
        Point16 pos = new(i, j);
        if (!TileEntity.ByPosition.TryGetValue(pos, out var te)) return;

        switch (te)
        {
            case MineTileEntity mine:
                mine.Kill(i, j);
                TriggerMine(new Point(i, j)); // trigger the mine explosion
                break;
            case FlagTileEntity flag:
                flag.Kill(i, j);
                break;
            default: return;                         // some other TE, ignore
        }

        // Tell clients the TE is gone
        if (Main.netMode == NetmodeID.Server)
            NetMessage.SendData(MessageID.TileEntitySharing,
                                 number: i, number2: j);  // kill packet
    }

    private static void TriggerMine(Point tilePos)
    {
        // remove the mine entity first
        if (MineTileEntity.TryGet(tilePos, out var mt))
            mt.Kill(tilePos.X, tilePos.Y);

        // centre of the tile in world coords
        Vector2 centre = new((tilePos.X + 0.5f) * 16f, (tilePos.Y + 0.5f) * 16f);

        Projectile.NewProjectile(
            new EntitySource_TileInteraction(Main.LocalPlayer, tilePos.X, tilePos.Y),
            centre,
            Vector2.Zero,
            ModContent.ProjectileType<MineExplosion>(),
            50, 3f, 255);
    }
}
