using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Content.Items.Accessories;
using Terrasweeper.Content.NPCs.NineBoss;

namespace Terrasweeper.Content.Items
{
    public class NineBossBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BossBag[Type] = true;
            ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;

            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;

            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;

            Item.rare = ItemRarityID.Purple;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Drop  100% of Time 10 to 20 mines
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MinedMine>(), 1, 10, 20));

            // Drop 100% of time 3 to 6 potions
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MinesweeperPotion>(), 1, 3, 6));

            // 25% drop chance of mine radar
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MineRadarInfoAccessoryItem>(), 1, 3, 6));

            // Drop Rare Bar 20% of Time - 2 - 5 bars

            if (Main.hardMode)
            {
                // 25% chance to drop radar
            }

            // Add Expert Only items
            //itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<MinedMine>(), 3));

            // Add Money
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<NineBoss>()));
        }
    }
}