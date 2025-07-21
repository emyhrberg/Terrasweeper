using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Common.BuilderToggles;
using Terrasweeper.Content.Buffs;

namespace Terrasweeper.Common.Systems
{
    internal class MinesweeperDrawSystem : ModSystem
    {
        // Fade variables
        private float CurrentFadeTimer; // The time it has spent fading
        private static readonly TimeSpan FadeTime = TimeSpan.FromSeconds(2);
        private float ShowMineBuffOpacity;

        private const int maxDistanceOfMinesVisibilityForMinesweeperPotionInTiles = 7;

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

                    // Minesweeper Potion Buff
                    int showMinesBuff = ModContent.BuffType<MinesVisibleBuff>();
                    int slot = Main.LocalPlayer.FindBuffIndex(showMinesBuff);

                    if (slot != -1)
                    {
                        int ticks = Main.LocalPlayer.buffTime[slot];
                        // WARNING:
                        // For some reson, Log and NewChat crashes here, so dont use it!!!
                        //Log.Info("ticks: " + ticks);
                        // Ticks start at 180 (3 seconds).
                        // Ticks between 180 and 150 should lerp from 0 to 1 opacity.
                        // Ticks between 30 and 0 should lerp from 1 to 0 opacity.
                        if (ticks >= 150)
                        {
                            float t = (180f - ticks) / 30f; // 0 to 1
                            ShowMineBuffOpacity = MathHelper.Lerp(0, 1, t);
                        }
                        else if (ticks >= 30)
                        {
                            ShowMineBuffOpacity = 1f;
                        }
                        else
                        {
                            float t = ticks / 30f; // 1 to 0
                            ShowMineBuffOpacity = MathHelper.Lerp(0, 1, t);
                        }
                    }

                    // Mines
                    ShowMinesBuilderToggle mineVisibilityToggle = ModContent.GetInstance<ShowMinesBuilderToggle>();

                    bool showMines = mineVisibilityToggle.Active() && mineVisibilityToggle.CurrentState == 0;
                    bool mineTile = unsolvedMine && !data.HasFlag && isTileSolidForMine;
                    if (showMines && mineTile)
                    {
                        // Unsolved Mine
                        Main.spriteBatch.Draw(
                            Ass.Minesweeper.Value,
                            drawPos,
                            MinesweeperTextures.GetRectangle(MinesweeperTexturesEnum.Mine),
                            color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                    // Minesweeper Potion Buff
                    else if (mineTile && Main.LocalPlayer.HasBuff(showMinesBuff))
                    {
                        // Getting distancebetween Player and tile
                        float distanceInTiles = Vector2.Distance(Main.LocalPlayer.position, new Vector2(i, j) * 16f) / 16f;

                        // If distance > 7(in tiles) - skip that mine
                        if (distanceInTiles > 7)
                        {
                            continue;
                        }

                        // Inversion of a percent (thats why 1f - ) also ^2 to make an opacity effect less stronger (maybe you can set bigger power like 2.5 or 3; because distanceInTiles / maxDistanceOfMinesVisibilityForMinesweeperPotionInTiles is between 0 and 1, taking it in power makes it smaller, so 1 - this_percent^2 would be bigger, so opacity would be weaker and glowing would be stronger)
                        float aPercentageOfDistance = 1f - (float)Math.Pow(distanceInTiles / maxDistanceOfMinesVisibilityForMinesweeperPotionInTiles, 2f); 

                        // Draving Mine
                        Main.spriteBatch.Draw(
                            Ass.Minesweeper.Value,
                            drawPos,
                            MinesweeperTextures.GetRectangle(MinesweeperTexturesEnum.Mine),
                            color * ShowMineBuffOpacity * aPercentageOfDistance, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        // Drawing dust and glow
                        if (Main.rand.NextBool(40))
                        {
                            Dust d = Dust.NewDustDirect(
                                Position: new Vector2(i * 16, j * 16),
                                Width: 16, Height: 16,
                                DustID.TreasureSparkle,                 // same gold-spelunker dust
                                SpeedX: 0f, SpeedY: 0f, Alpha: 150,
                                newColor: default,
                                Scale: 0.05f);
                            d.fadeIn = 1.50f;
                            d.velocity *= 0.10f;
                            d.noLight = false;     // let the dust itself cast tiny light
                        }

                        // steady “glow” so the tile pops in dark caves
                        Lighting.AddLight(i, j, r: 0.65f * aPercentageOfDistance, g: 0.55f * aPercentageOfDistance, b: 0.15f * aPercentageOfDistance); // warm gold-yellow
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
                        Log.Warn($"Wrong data on MINE!");
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
                            Log.Warn($"Wrong data on NUMBER!");
                        }
                    }

                }
            }

            Main.spriteBatch.End();
        }

    }
}
