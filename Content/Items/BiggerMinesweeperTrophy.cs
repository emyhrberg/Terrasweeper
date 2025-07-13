using JulyJam.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JulyJam.Content.Items
{
    public class BiggerMinesweeperTrophy : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<BiggerMinesweeperTrophyTile>());
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<MinedMine>(999);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.Register();

        }
    }
}
