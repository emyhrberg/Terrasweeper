using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Content.Tiles;

namespace Terrasweeper.Content.Items
{
    public class MinesweeperTrophy : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<MinesweeperTrophyTile>());
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<MinedMine>(99);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.Register();

        }
    }
}
