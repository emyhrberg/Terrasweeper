﻿using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Content.Projectiles;

namespace Terrasweeper.Content.Items
{
    internal class MinedMine : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 9999;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 1);
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<FlaggedFlag>();

            // Projectile stuff
            Item.shoot = ModContent.ProjectileType<MinedMineProjectile>();
            Item.damage = 80;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 8f;
            Item.shootSpeed = 5f;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item1, player.position);
            return base.UseItem(player);
        }
    }
}
