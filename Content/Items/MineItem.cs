using JulyJam.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JulyJam.Content.Items
{
    internal class MineItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 99;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = Item.useAnimation;
            Item.consumable = true;
            Item.value = Terraria.Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
        }

        public override bool? UseItem(Player player)
        {
            int i = Player.tileTargetX;
            int j = Player.tileTargetY;

            Tile tile = Framing.GetTileSafely(i, j);

            if (tile.HasTile && !tile.IsActuated)
            {
                if (JJUtils.WithinPlacementRange(player, i, j))
                {
                    ref var data = ref Main.tile[i, j].Get<MinesweeperData>();
                    data.HasMine = !data.HasMine;
                    SoundEngine.PlaySound(SoundID.Dig, player.Center);
                    NetMessage.SendTileSquare(-1, i, j, 1);
                    return true;
                }
            }
            return false;
        }
    }
}
