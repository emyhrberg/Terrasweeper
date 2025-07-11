using System;
using Terraria;
using Terraria.ModLoader;

namespace JulyJam.Common.Systems
{
    internal class TileDrawSystem : ModSystem
    {
        public override void PostDrawTiles()
        {
            base.PostDrawTiles();
            Main.spriteBatch.Begin();

            // Check if the toggle is enabled
            if (!BuilderToggles.NumbersVisibleState.Visible)
            {
                // If not visible, skip drawing numbers
                Main.spriteBatch.End();
                return;
            }

            // Draw test numbers
            int padding = -5;
            int tileStartX = Math.Max((int)(Main.screenPosition.X / 16) - padding, 0);
            int tileStartY = Math.Max((int)(Main.screenPosition.Y / 16) - padding, 0);
            int tileEndX = Math.Min((int)((Main.screenPosition.X + Main.screenWidth) / 16) + padding, Main.maxTilesX - 1);
            int tileEndY = Math.Min((int)((Main.screenPosition.Y + Main.screenHeight) / 16) + padding, Main.maxTilesY - 1);

            for (int x = tileStartX; x <= tileEndX; x++)
            {
                for (int y = tileStartY; y <= tileEndY; y++)
                {
                    Vector2 drawPos = new(x * 16 - Main.screenPosition.X, y * 16 - Main.screenPosition.Y);
                    Utils.DrawBorderString(Main.spriteBatch, "1", drawPos, Color.Red);
                }
            }

            // Always end
            Main.spriteBatch.End();
        }
    }
}
