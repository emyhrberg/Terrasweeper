using JulyJam.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JulyJam.Content.Items
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
            recipe.AddIngredient<FlaggedFlag>(9);
            recipe.AddIngredient(ItemID.Bomb);
            recipe.Register();
        }
    }
}
