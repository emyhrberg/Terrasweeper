using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terrasweeper.Common.BuilderToggles;

namespace Terrasweeper.Common.Systems
{
    internal class MinesweeperDrawSystem : ModSystem
    {
        public override void PostDrawTiles()
        {
            base.PostDrawTiles();

            DrawMinesweeperElements();
        }

        public void DrawMinesweeperElements()
        {
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

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

            Point screenOverdrawOffset = Main.GetScreenOverdrawOffset();

            for (int i = tileStartX + screenOverdrawOffset.X; i < tileEndX - screenOverdrawOffset.X; i++)
            {
                for (int j = tileStartY + screenOverdrawOffset.Y; j < tileEndY - screenOverdrawOffset.Y; j++)
                {
                    var tile = Main.tile[i, j];

                    ref var data = ref tile.Get<MinesweeperData>();
                    Vector2 drawPos = new(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y);
                    bool isTileSolidForMine = JJUtils.IsTileSolidForMine(tile);
                    bool isTileSolidForNumbers = JJUtils.IsTileSolidForNumbers(tile);
                    bool unsolvedMine = data.MineStatus == MineStatus.UnsolvedMine;

                    // get numbers transparency
                    int state = ModContent.GetInstance<NumbersTransparencyBuilderToggle>().CurrentState;
                    float opacity = 0f;
                    if (state == 0) // 100%
                        opacity = 1f;
                    else if (state == 1) // 50%
                        opacity = 0.4f;
                    else if (state == 2)
                        opacity = 0f; // 0%
                    Color color = Lighting.GetColor(i, j) * opacity;

                    // Mines (only debug)
                    if (unsolvedMine && !data.HasFlag && isTileSolidForMine && 
                        ModContent.GetInstance<ShowMinesBuilderToggle>().CurrentState == 0)
                    {
                        Main.spriteBatch.Draw(
                            Ass.Minesweeper.Value,
                            drawPos,
                            MinesweeperTextures.GetRectangle(MinesweeperTexturesEnum.Mine),
                            color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                    // Solved Mine
                    else if (data.MineStatus == MineStatus.Solved)
                    {
                        Main.spriteBatch.Draw(
                            Ass.Minesweeper.Value,
                            drawPos,
                            MinesweeperTextures.GetRectangle(MinesweeperTexturesEnum.SolvedMine),
                            color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                    // Failed Mine
                    else if (data.MineStatus == MineStatus.Failed)
                    {
                        Main.spriteBatch.Draw(
                            Ass.Minesweeper.Value,
                            drawPos,
                            MinesweeperTextures.GetRectangle(MinesweeperTexturesEnum.FailedMine),
                            color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                    // Flag
                    else if (data.HasFlag && isTileSolidForMine)
                    {
                        Main.spriteBatch.Draw(
                            Ass.Minesweeper.Value,
                            drawPos,
                            MinesweeperTextures.GetRectangle(MinesweeperTexturesEnum.Flag),
                            color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                    // Error data on tile
                    else if ((data.HasFlag || unsolvedMine) && !isTileSolidForMine)
                    {
                        Main.spriteBatch.Draw(
                            Ass.Minesweeper.Value,
                            drawPos,
                            MinesweeperTextures.GetRectangle(MinesweeperTexturesEnum.Nine),
                            Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        Log.Warn($"Wrong data on tile!");
                    }
                    // Numbers
                    else if (!isTileSolidForNumbers)
                    {
                        // Nothing, no mines near tile
                        if (data.TileNumber == 0)
                        {
                            continue;
                        }
                        // numbers from 1 to 8
                        if (data.TileNumber > 0 && data.TileNumber < 9)
                        {
                            Main.spriteBatch.Draw(
                            Ass.Minesweeper.Value,
                            drawPos,
                            MinesweeperTextures.GetRectangle((MinesweeperTexturesEnum)data.TileNumber),
                            color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        }
                        // Error data on tile
                        else
                        {
                            Main.spriteBatch.Draw(
                            Ass.Minesweeper.Value,
                            drawPos,
                            MinesweeperTextures.GetRectangle(MinesweeperTexturesEnum.Nine), // 9
                            Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                            Log.Warn($"Wrong data on tile!");
                        }
                    }

                }
            }

            Main.spriteBatch.End();
        }

    }
}
