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
            Item.CloneDefaults(ItemID.Bomb);
            Item.shoot = ModContent.ProjectileType<StickyFlagBombProjectile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<Flag>(20);
            recipe.AddIngredient(ItemID.Bomb);
            recipe.Register();
        }
    }
}
