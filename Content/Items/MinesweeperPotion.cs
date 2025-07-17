using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Content.Buffs;

namespace Terrasweeper.Content.Items
{
    internal class MinesweeperPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 20;

            // Dust that will appear in these colors when the item with ItemUseStyleID.DrinkLiquid is used
            ItemID.Sets.DrinkParticleColors[Type] = [
                new Color(240, 240, 240),
                new Color(200, 200, 200),
                new Color(140, 140, 140)
            ];
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(silver: 1);
            Item.buffType = ModContent.BuffType<MinesVisibleBuff>(); // Specify an existing buff to be applied when used.
            const int tick = 60; // 1 second = 60 ticks
            Item.buffTime = 2 * tick; // 5 seconds duration
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MinedMine>(), 20)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
