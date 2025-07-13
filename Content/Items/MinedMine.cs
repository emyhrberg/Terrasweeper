using JulyJam.Common.Systems;
using JulyJam.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using JulyJam.Content.Projectiles;

namespace JulyJam.Content.Items
{
    internal class MinedMine : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 999;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 1);
            Item.shoot = ModContent.ProjectileType<MinedMineProjectile>();
            Item.damage = 80;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 8f;
            Item.shootSpeed = 5f;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<FlaggedFlag>();
        }

        /*
        public override bool? UseItem(Player player)
        {
            int i = Player.tileTargetX;
            int j = Player.tileTargetY;

            Tile tile = Framing.GetTileSafely(i, j);
            ref var data = ref Main.tile[i, j].Get<MinesweeperData>();
            if (data.HasMine)
            {
                JJUtils.Explode(i, j);
                return true;
            }
            else
            {
                data.HasMine = true;
                MinesweeperData.UpdateNumbersOfMines3x3(i, j);
                SoundEngine.PlaySound(SoundID.Dig, player.Center);
                return true;
            }
            
        }

        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) // ignore other clients
                return false;

            int i = Player.tileTargetX;
            int j = Player.tileTargetY;

            Tile tile = Framing.GetTileSafely(i, j);

            if (!JJUtils.IsTileSolid(tile) || !JJUtils.WithinPlacementRange(player, i, j))
            {
                return false;
            }
            return true;
        }*/
    }
}
