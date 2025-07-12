using System;
using System.Collections.Generic;
using JulyJam.Common.BuilderToggles;
using JulyJam.Content.TileEntities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace JulyJam.Common.Systems
{
    internal class TileDrawSystem : ModSystem
    {
        public override void PostDrawTiles()
        {
            base.PostDrawTiles();

            Main.spriteBatch.Begin(default, default, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
            //Main.spriteBatch.Begin();

            DrawFlags();
            DrawMines();

            if (NumbersVisibleState.Visible)
                DrawNumbers();

            Main.spriteBatch.End();
        }

        public void DrawFlags()
        {
            foreach (var kv in TileEntity.ByPosition)
            {
                if (kv.Value is not FlagTileEntity) continue;

                int i = kv.Key.X; // tile coords
                int j = kv.Key.Y;

                Vector2 worldPos = new(i * 16 + 8, j * 16 + 8);   // tile centre in pixels
                Vector2 screenPos = worldPos - Main.screenPosition;

                Main.spriteBatch.Draw(Ass.Flag.Value, screenPos, null, Color.White,     // no rotation
                        0f, Ass.Flag.Value.Size() / 2f, 1f, SpriteEffects.None, 0f);
            }
        }

        public void DrawMines()
        {
            foreach (var kv in TileEntity.ByPosition)
            {
                if (kv.Value is not MineTileEntity) continue;

                int i = kv.Key.X; // tile coords
                int j = kv.Key.Y;

                Vector2 worldPos = new(i * 16 + 8, j * 16 + 8);   // tile centre in pixels
                Vector2 screenPos = worldPos - Main.screenPosition;

                Main.spriteBatch.Draw(Ass.Mine.Value, screenPos, null, Color.White,0f, Ass.Flag.Value.Size() / 2f, 1f, SpriteEffects.None, 0f);
            }
        }

        public void DrawNumbers()
        {
            // keep track so we don't draw the same tile twice
            var processed = new HashSet<Point16>();

            foreach (var kv in TileEntity.ByPosition)
            {
                if (kv.Value is not MineTileEntity) continue;   // start from every mine

                int mi = kv.Key.X;
                int mj = kv.Key.Y;

                // walk the 8 neighbours around that mine
                for (int dy = -1; dy <= 1; dy++)
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        if (dx == 0 && dy == 0) continue;           // skip the mine itself
                        int ni = mi + dx;
                        int nj = mj + dy;

                        Point16 nPos = new(ni, nj);
                        if (!WorldGen.InWorld(ni, nj) || processed.Contains(nPos))
                            continue;                               // already handled / out of map

                        // don't put a number on a mine tile
                        if (TileEntity.ByPosition.TryGetValue(nPos, out var te) && te is MineTileEntity)
                        {
                            processed.Add(nPos);
                            continue;
                        }

                        // count mines around this neighbour
                        int count = 0;
                        for (int ddy = -1; ddy <= 1; ddy++)
                            for (int ddx = -1; ddx <= 1; ddx++)
                            {
                                if (ddx == 0 && ddy == 0) continue;
                                Point16 cPos = new(ni + ddx, nj + ddy);
                                if (TileEntity.ByPosition.TryGetValue(cPos, out var te2) && te2 is MineTileEntity)
                                    count++;
                            }

                        if (count > 0)
                        {
                            Vector2 worldPos = new(ni * 16 + 8, nj * 16 + 8);
                            Vector2 screenPos = worldPos - Main.screenPosition;
                            screenPos += new Vector2(-2, -8);

                            // draw centred, a bit smaller than default
                            Utils.DrawBorderString(Main.spriteBatch,
                                                   count.ToString(),
                                                   screenPos,
                                                   Color.White,
                                                   scale: 0.8f);
                        }

                        processed.Add(nPos);
                    }
            }
        }

    }
}
