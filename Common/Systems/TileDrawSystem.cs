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
            if (!BuilderToggles.NumbersVisibleState.Visible) return;

            Main.spriteBatch.Begin();

            // Draw test numbers using 16x16 tile scaling logic
            int padding = -29;
            int tileStartX = Math.Max((int)(Main.screenPosition.X / 16) - padding, 0);
            int tileStartY = Math.Max((int)(Main.screenPosition.Y / 16) - padding, 0);
            int tileEndX = Math.Min((int)((Main.screenPosition.X + Main.screenWidth) / 16) + padding, Main.maxTilesX - 1);
            int tileEndY = Math.Min((int)((Main.screenPosition.Y + Main.screenHeight) / 16) + padding, Main.maxTilesY - 1);

            for (int x = tileStartX; x <= tileEndX; x++)
            {
                for (int y = tileStartY; y <= tileEndY; y++)
                {
                    Vector2 drawPos = new(x * 16 - Main.screenPosition.X, y * 16 - Main.screenPosition.Y);
                    string number = "1";
                    Utils.DrawBorderString(Main.spriteBatch, number, drawPos, Color.Red, 1.0f);
                }
            }

            // Always end
            Main.spriteBatch.End();
        }
    }
}
