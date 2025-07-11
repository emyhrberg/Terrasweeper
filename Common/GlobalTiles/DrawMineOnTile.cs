using JulyJam.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace JulyJam.Common.GlobalTiles
{
    internal class DrawMineOnTile : GlobalTile
    {
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            base.PostDraw(i, j, type, spriteBatch);
            Tile tile = Main.tile[i, j];

            if (tile.Get<MinesweeperData>().HasMine)
            {
                Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
                spriteBatch.Draw(
                Ass.Mine.Value,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(0, 0, 16, 16),
                Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
