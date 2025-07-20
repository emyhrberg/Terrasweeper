using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Common.Globals;
using Terrasweeper.Common.PacketHandlers;
using Terrasweeper.Common.Systems;
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

        public static bool IsTileSolidForMine(int i, int j)
        {

            Tile tile = Framing.GetTileSafely(i, j);
            return IsTileSolidForMine(tile); ;
        }

        public static bool IsTileSolidForMine(Tile tile)
        {
            bool result = tile.HasTile && IsTileSolidForMine(tile.TileType);
            return result;
        }

        public static bool IsTileSolidForMine(int tileID)
        {
            bool result =
                IsTileSolidForNumbers(tileID) &&
                TileIDSets.CanPlaceMine[tileID] &&
                !TileID.Sets.CrackedBricks[tileID] &&
                !TileID.Sets.Boulders[tileID] &&
                !TileID.Sets.Leaves[tileID] &&
                (TileID.Sets.TouchDamageImmediate[tileID] == 0);
            if (Main.getGoodWorld || WorldGen.getGoodWorldGen)
            {
                result &= tileID != TileID.Ash; // Ash is not solid in ftw worlds
                result &= tileID != TileID.AshGrass;
            }
            return result;
        }

        public static bool IsTileSolidForNumbers(Tile tile)
        {
            bool result = tile.HasTile && IsTileSolidForNumbers(tile.TileType);
            return result;
        }

        public static bool IsTileSolidForNumbers(int tileID)
        {
            bool result = !Main.tileFrameImportant[tileID] &&
                !TileID.Sets.IsVine[tileID] &&
                !TileID.Sets.IsBeam[tileID] &&
                !TileID.Sets.BasicChest[tileID] &&
                !TileID.Sets.BasicDresser[tileID] &&
                !TileID.Sets.CountsAsGemTree[tileID] &&
                !TileID.Sets.IsATreeTrunk[tileID] &&
                !TileID.Sets.CommonSapling[tileID] &&
                !TileID.Sets.Paintings[tileID] &&
                !TileID.Sets.CanBeSatOnForNPCs[tileID] &&
                !TileID.Sets.CanBeSatOnForPlayers[tileID] &&
                !TileID.Sets.CanBeSleptIn[tileID] &&
                !TileID.Sets.Platforms[tileID] &&
                !TileID.Sets.NotReallySolid[tileID] &&
                !TileID.Sets.BreakableWhenPlacing[tileID] &&
                !TileID.Sets.TouchDamageBleeding[tileID] &&
                !TileID.Sets.IsAContainer[tileID] &&
                !Main.tileCut[tileID] &&
                tileID != TileID.Cactus;
            return result;
        }

        public static void Explode(int i, int j)
        {
            // centre of the tile in world coords
            Vector2 centre = new((i + 0.5f) * 16f, (j + 0.5f) * 16f);
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                CreateExplosion(i, j, centre);
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModNetHandler.minesweeperPacketHandler.SendExplosion(i, j, centre);
            }
        }

        public static void CreateExplosion(int i, int j, Vector2 centre)
        {
            Projectile.NewProjectile(
                            new EntitySource_TileBreak(i, j),
                            centre,
                            Vector2.Zero,
                            ModContent.ProjectileType<MineExplosion>(),
                            500, 3f);
        }

        public static void ToggleFlagState(int i, int j)
        {
            
            Tile tile = Framing.GetTileSafely(i, j);
            ref var data = ref tile.Get<MinesweeperData>();
            data.HasFlag = !data.HasFlag;
            SoundEngine.PlaySound(SoundID.Tink, i * 16, j * 16);
            ModNetHandler.minesweeperPacketHandler.SendSingleTile(i, j);
        }
    }
}
