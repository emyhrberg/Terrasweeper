using JulyJam.Common;
using JulyJam.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

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

        public static void Explode(int i, int j)
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
}
