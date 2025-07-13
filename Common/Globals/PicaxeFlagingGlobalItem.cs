using JulyJam.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace JulyJam.Common.Globals
{
    internal class PicaxeFlagingGlobalItem : GlobalItem
    {
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (item.pick <= 0)
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
            return false;
        }
    }
}
