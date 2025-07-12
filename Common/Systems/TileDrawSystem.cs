using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JulyJam.Common.Systems
{
    internal class TileDrawSystem : ModSystem
    {
        public override void PostDrawTiles()
        {
            base.PostDrawTiles();
            if (!BuilderToggles.NumbersVisibleState.Visible) return;

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

            // Draw test numbers using 16x16 tile scaling logic
            Vector2 zero2 = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            int tileStartX = (int)((Main.screenPosition.X - zero2.X) / 16f - 1f);
            int tileStartY = (int)((Main.screenPosition.Y - zero2.Y) / 16f - 1f);
            int tileEndX = (int)((Main.screenPosition.X + Main.screenWidth + zero2.X) / 16f) + 2;
            int tileEndY = (int)((Main.screenPosition.Y + Main.screenHeight + zero2.Y) / 16f) + 5;

            if (tileStartX < 0)
                tileStartX = 0;

            if (tileEndX > Main.maxTilesX)
                tileEndX = Main.maxTilesX;

            if (tileStartY < 0)
                tileStartY = 0;

            if (tileEndY > Main.maxTilesY)
                tileEndY = Main.maxTilesY;

            Microsoft.Xna.Framework.Point screenOverdrawOffset = Main.GetScreenOverdrawOffset();

            for (int i = tileStartX + screenOverdrawOffset.X; i < tileEndX - screenOverdrawOffset.X; i++)
            {
                for (int j = tileStartY + screenOverdrawOffset.Y; j < tileEndY - screenOverdrawOffset.Y; j++)
                {
                    var tile = Main.tile[i, j];
                    if (tile.HasTile && !Main.tileFrameImportant[tile.TileType])
                    {
                        continue;
                    }
                        
                    Vector2 drawPos = new(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y);
                    Main.spriteBatch.Draw(
                        Ass.Mine.Value,
                        drawPos,
                        new Rectangle(18, 0, 16, 16),
                        Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                }
            }

            // Always end
            Main.spriteBatch.End();
        }
    }
}
