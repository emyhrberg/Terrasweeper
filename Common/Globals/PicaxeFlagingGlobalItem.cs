using Terraria;
using Terraria.ModLoader;
using Terrasweeper.Common.BuilderToggles;
using Terrasweeper.Common.PacketHandlers;
using Terrasweeper.Common.Systems;

namespace Terrasweeper.Common.Globals
{
    internal class PicaxeFlagingGlobalItem : GlobalItem
    {
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (item.pick <= 0 || ModContent.GetInstance<NumbersTransparencyBuilderToggle>().CurrentState == 2)
            {
                return false;
            }
            int i = Player.tileTargetX;
            int j = Player.tileTargetY;
            Tile tile = Framing.GetTileSafely(i, j);
            if (!tile.HasTile)
            {
                return false;
            }
            if (!JJUtils.WithinPlacementRange(player, i, j))
            {
                return false;
            }
            ref var data = ref tile.Get<MinesweeperData>();
            data.HasFlag = !data.HasFlag;
            ModNetHandler.minesweeperPacketHandler.SendSingleTile(i, j);
            return false;
        }
    }
}
