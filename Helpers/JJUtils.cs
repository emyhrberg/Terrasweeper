using JulyJam.Common;
using Terraria;
using Terraria.ID;

namespace JulyJam.Helpers
{
    internal static class JJUtils
    {
        public static bool WithinPlacementRange(Player player, int x, int y) =>
            player.position.X / 16f - Player.tileRangeX - player.inventory[player.selectedItem].tileBoost - player.blockRange <= x
            && (player.position.X + player.width) / 16f + Player.tileRangeX + player.inventory[player.selectedItem].tileBoost - 1f + player.blockRange >= x
            && player.position.Y / 16f - Player.tileRangeY - player.inventory[player.selectedItem].tileBoost - player.blockRange <= y
            && (player.position.Y + player.height) / 16f + Player.tileRangeY + player.inventory[player.selectedItem].tileBoost - 2f + player.blockRange >= y;

        public static bool IsTileSolid(Tile tile)
        {
            return tile.HasTile && !Main.tileFrameImportant[tile.TileType];
        }

        public static bool CanPlaceMine(Tile tile)
        {
            return IsTileSolid(tile) && !TileID.Sets.Falling[tile.TileType] && TileIDSets.CanPlaceMine[tile.TileType];
        }
    }
}
