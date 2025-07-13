using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Common.Globals;
using Terrasweeper.Content.Projectiles;

namespace Terrasweeper.Helpers
{
    internal static class JJUtils
    {
        public static bool WithinPlacementRange(Player player, int x, int y) =>
            player.position.X / 16f - Player.tileRangeX - player.inventory[player.selectedItem].tileBoost - player.blockRange <= x
            && (player.position.X + player.width) / 16f + Player.tileRangeX + player.inventory[player.selectedItem].tileBoost - 1f + player.blockRange >= x
            && player.position.Y / 16f - Player.tileRangeY - player.inventory[player.selectedItem].tileBoost - player.blockRange <= y
            && (player.position.Y + player.height) / 16f + Player.tileRangeY + player.inventory[player.selectedItem].tileBoost - 2f + player.blockRange >= y;

        public static bool IsTileSolidForMine(Tile tile)
        {
            bool result = tile.HasTile && IsTileSolidForMine(tile.TileType);
            return result;
        }

        public static bool IsTileSolidForMine(int tileID)
        {
            bool result = !Main.tileFrameImportant[tileID] &&
                TileIDSets.CanPlaceMine[tileID] &&
                !TileID.Sets.IsVine[tileID] &&
                !TileID.Sets.IsBeam[tileID] &&
                !TileID.Sets.BasicChest[tileID] &&
                !TileID.Sets.BasicDresser[tileID] &&
                !TileID.Sets.CountsAsGemTree[tileID] &&
                !TileID.Sets.IsATreeTrunk[tileID] &&
                !TileID.Sets.CommonSapling[tileID] &&
                !TileID.Sets.CrackedBricks[tileID] &&
                !TileID.Sets.Paintings[tileID] &&
                !TileID.Sets.Boulders[tileID] &&
                !TileID.Sets.CanBeSatOnForNPCs[tileID] &&
                !TileID.Sets.CanBeSatOnForPlayers[tileID] &&
                !TileID.Sets.CanBeSleptIn[tileID] &&
                !TileID.Sets.Platforms[tileID] &&
                !TileID.Sets.Leaves[tileID] &&
                !TileID.Sets.NotReallySolid[tileID] &&
                !TileID.Sets.BreakableWhenPlacing[tileID] &&
                !TileID.Sets.TouchDamageBleeding[tileID] &&
                (TileID.Sets.TouchDamageImmediate[tileID] == 0) &&
                !TileID.Sets.IsAContainer[tileID] &&
                !Main.tileCut[tileID];
            if (Main.getGoodWorld || WorldGen.getGoodWorldGen)
            {
                result &= tileID != TileID.Ash; // Ash is not solid in ftw worlds
                result &= tileID != TileID.AshGrass;
            }
            return result;
        }

        public static bool IsTileSolidForNumbers(Tile tile)
        {
            bool result = tile.HasTile && !Main.tileFrameImportant[tile.TileType] &&
                !TileID.Sets.IsVine[tile.TileType] &&
                !TileID.Sets.IsBeam[tile.TileType] &&
                !TileID.Sets.BasicChest[tile.TileType] &&
                !TileID.Sets.BasicDresser[tile.TileType] &&
                !TileID.Sets.CountsAsGemTree[tile.TileType] &&
                !TileID.Sets.IsATreeTrunk[tile.TileType] &&
                !TileID.Sets.CommonSapling[tile.TileType] &&
                !TileID.Sets.Paintings[tile.TileType] &&
                !TileID.Sets.CanBeSatOnForNPCs[tile.TileType] &&
                !TileID.Sets.CanBeSatOnForPlayers[tile.TileType] &&
                !TileID.Sets.CanBeSleptIn[tile.TileType] &&
                !TileID.Sets.Platforms[tile.TileType] &&
                !TileID.Sets.NotReallySolid[tile.TileType] &&
                !TileID.Sets.BreakableWhenPlacing[tile.TileType] &&
                !TileID.Sets.TouchDamageBleeding[tile.TileType] &&
                !TileID.Sets.IsAContainer[tile.TileType] &&
                !Main.tileCut[tile.TileType];
            return result;
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
