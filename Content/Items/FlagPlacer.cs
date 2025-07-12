using JulyJam.Common.Systems;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JulyJam.Content.Items
{
    /// <summary>Left-click toggles a flag on the tile under the cursor.</summary>
    public class FlagPlacer : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
            ItemID.Sets.DuplicationMenuToolsFilter[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.UseSound = SoundID.Dig;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 50);
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) // ignore other clients
                return null;

            // convert to mouse to tile coords
            Vector2 worldPos = Main.MouseWorld;
            player.LimitPointToPlayerReachableArea(ref worldPos); // limit to player reach
            Point tilePos = worldPos.ToTileCoordinates();
            Tile tile = Main.tile[tilePos];

            if (!tile.HasTile || Main.tileFrameImportant[tile.TileType])
                return false; // only toggle if the tile exists and not a frameimportant

            
            ref var data = ref tile.Get<MinesweeperData>();
            data.HasFlag = !data.HasFlag; // toggle flag presence

            //FlagTileEntity.Toggle(tilePos);

            SoundEngine.PlaySound(SoundID.Grab, player.Center);
            return true;
        }

    }
}