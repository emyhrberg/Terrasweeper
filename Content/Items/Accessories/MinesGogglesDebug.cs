using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Common.Players;

namespace Terrasweeper.Content.Items.Accessories
{
    internal class MinesGogglesDebug : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Sunglasses;

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Sunglasses);
            Item.accessory = true;
            Item.headSlot = -1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MineGogglesPlayer>().active = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MineGogglesPlayer>().active = true;
        }
        public override void UpdateInventory(Player player)
        {
            player.GetModPlayer<MineGogglesPlayer>().active = true;
        }
    }
}
