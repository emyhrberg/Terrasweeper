using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Terrasweeper.Common.Systems
{
    internal class MinesweeperFallPreventionSystem : ModSystem
    {
        public override void Load()
        {
            On_WorldGen.SpawnFallingBlockProjectile += WorldGen_SpawnFallingBlockProjectile;
        }
        public override void Unload()
        {
            On_WorldGen.SpawnFallingBlockProjectile -= WorldGen_SpawnFallingBlockProjectile;
        }

        private bool WorldGen_SpawnFallingBlockProjectile(On_WorldGen.orig_SpawnFallingBlockProjectile orig, int i, int j, Tile tileCache, Tile tileTopCache, Tile tileBottomCache, int type)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return false;

            if (tileCache == null || tileTopCache == null || tileBottomCache == null)
                return false;

            Tile tile = Framing.GetTileSafely(i, j);
            if (tile.Get<MinesweeperData>().data > 0)
            {
                return false;
            }
            return orig(i, j, tileCache, tileTopCache, tileBottomCache, type);
        }
    }
}
