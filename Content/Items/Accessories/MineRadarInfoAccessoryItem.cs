using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Common.Players;
using Terrasweeper.Common.Systems;
using Terrasweeper.Content.InfoDisplays;

namespace Terrasweeper.Content.Items.Accessories
{
    /// <summary>
    /// ModItem that shows how many mines are nearby.
    /// <seealso cref="MineRadarInfoDisplay"/>
    /// <seealso cref="MineRadarInfoPlayer"/>
    /// </summary>
    public class MineRadarInfoAccessoryItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // If you DON'T want your info accessory to work in the void bag, then add: ItemID.Sets.WorksInVoidBag[Type] = false;
        }

        public override void SetDefaults()
        {
            // We don't need to add anything particularly unique for the stats of this item; so let's just clone the Radar.
            Item.CloneDefaults(ItemID.Radar);
        }

        // This is the main hook that allows for our info display to actually work with this accessory. 
        public override void UpdateInfoAccessory(Player player)
        {
            int radius = 25;
            int mineCount = 0;
            int px = (int)(player.Center.X / 16f);
            int py = (int)(player.Center.Y / 16f);

            for (int x = px - radius; x <= px + radius; x++)
            {
                if (x < 0 || x >= Main.maxTilesX) continue;
                for (int y = py - radius; y <= py + radius; y++)
                {
                    if (y < 0 || y >= Main.maxTilesY) continue;
                    if ((x - px) * (x - px) + (y - py) * (y - py) > radius * radius) continue;

                    ref var data = ref Main.tile[x, y].Get<MinesweeperData>();
                    if (data.MineStatus == MineStatus.UnsolvedMine)
                        mineCount++;
                }
            }

            player.GetModPlayer<MineRadarInfoPlayer>().mineRadarNearbyCount = mineCount;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MinedMine>(), 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
