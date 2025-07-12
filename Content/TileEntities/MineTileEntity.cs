using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JulyJam.Content.TileEntities
{
    /// <summary>Mine tile entity for toggling mines on tiles.</summary>
    public class MineTileEntity : ModTileEntity
    {
        // Checks if the tile at (x, y) is valid for a mine.
        public override bool IsTileValidForEntity(int x, int y) => Main.tile[x, y].HasTile;

        /// <summary>Gets the MineTileEntity at a point if it exists.</summary>
        public static bool TryGet(Point p, out MineTileEntity f)
        {
            if (ByPosition.TryGetValue(new Point16(p.X, p.Y), out TileEntity te) && te is MineTileEntity mt)
            {
                f = mt;
                return true;
            }
            f = null;
            return false;
        }

        /// <summary>Places a mine entity at (x, y).</summary>
        private static int PlaceDirect(int x, int y)
        {
            var entity = new MineTileEntity();
            return entity.Place(x, y);
        }

        /// <summary>Toggles mine presence at a tile, syncing in multiplayer.</summary>
        public static void Toggle(Point p)
        {
            // Remove mine if present.
            if (TryGet(p, out var existing))
            {
                existing.Kill(p.X, p.Y);
                //if (Main.netMode == NetmodeID.MultiplayerClient)
                    //NetMessage.SendData(MessageID.TileEntitySharing, remoteClient: -1, ignoreClient: -1, text: null, number: p.X, number2: p.Y);
                return;
            }

            // Place mine if absent.
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //NetMessage.SendData(MessageID.TileEntityPlacement, remoteClient: -1, ignoreClient: -1, text: null, number: p.X, number2: p.Y, number3: ModContent.TileEntityType<MineTileEntity>());
                return;
            }

            int id = PlaceDirect(p.X, p.Y);
            //NetMessage.SendData(MessageID.TileEntitySharing, remoteClient: -1, ignoreClient: -1, text: null, number: id, number2: p.X, number3: p.Y);
        }
    }
}
