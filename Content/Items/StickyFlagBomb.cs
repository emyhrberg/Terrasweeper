using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Content.Projectiles;

namespace Terrasweeper.Content.Items
{
    public class StickyFlagBomb : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StickyBomb);
            Item.shoot = ModContent.ProjectileType<StickyFlagBombProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<FlaggedFlag>(20);
            recipe.AddIngredient(ItemID.StickyBomb);
            recipe.Register();

            CreateRecipe()
            .AddIngredient<FlagBomb>(1)
            .AddIngredient(ItemID.Gel)
            .Register();
        }
    }
}
