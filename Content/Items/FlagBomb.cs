using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Content.Projectiles;

namespace Terrasweeper.Content.Items
{
    public class FlagBomb : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Bomb);
            Item.shoot = ModContent.ProjectileType<FlagBombProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<FlaggedFlag>(10);
            recipe.AddIngredient(ItemID.Bomb);
            recipe.Register();
        }
    }
}
