﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Terrasweeper.Content.Tiles
{
    public class MinesweeperTrophyTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(120, 85, 60), Language.GetText("MapObject.Trophy"));
            DustType = DustID.WoodFurniture;
        }
    }
}
