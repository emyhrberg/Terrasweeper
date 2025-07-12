using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JulyJam.Content.TileEntities
{
    /// <summary>Flag tile entity for toggling flags on tiles.</summary>
    public class FlagTileEntity : ModTileEntity
    {
        // Checks if the tile at (x, y) is valid for a flag.
        public override bool IsTileValidForEntity(int x, int y) => Main.tile[x, y].HasTile;

        /// <summary>Gets the FlagTileEntity at a point if it exists.</summary>
        public static bool TryGet(Point p, out FlagTileEntity f)
        {
            if (ByPosition.TryGetValue(new Point16(p.X, p.Y), out TileEntity te) && te is FlagTileEntity ft)
            {
                f = ft;
                return true;
            }
            f = null;
            return false;
        }

        /// <summary>Places a flag entity at (x, y).</summary>
        private static int PlaceDirect(int x, int y)
        {
            var entity = new FlagTileEntity();
            return entity.Place(x, y);
        }

        /// <summary>Toggles flag presence at a tile, syncing in multiplayer.</summary>
        public static void Toggle(Point p)
        {
            // Remove flag if present.
            if (TryGet(p, out var existing))
            {
                existing.Kill(p.X, p.Y);
                //if (Main.netMode == NetmodeID.MultiplayerClient)
                    //NetMessage.SendData(MessageID.TileEntitySharing, remoteClient: -1, ignoreClient: -1, text: null, number: p.X, number2: p.Y);
                return;
            }

            // Place flag if absent.
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //NetMessage.SendData(MessageID.TileEntityPlacement, remoteClient: -1, ignoreClient: -1, text: null, number: p.X, number2: p.Y, number3: ModContent.TileEntityType<FlagTileEntity>());
                return;
            }

            int id = PlaceDirect(p.X, p.Y);
            //NetMessage.SendData(MessageID.TileEntitySharing, remoteClient: -1, ignoreClient: -1, text: null, number: id, number2: p.X, number3: p.Y);
        }
    }
}
