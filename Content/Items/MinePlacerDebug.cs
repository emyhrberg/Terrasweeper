using JulyJam.Common.Systems;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JulyJam.Content.Items
{
    /// <summary>Left-click toggles a mine on the tile under the cursor.</summary>
    public class MinePlacerDebug : ModItem
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
            // Get the tile under the cursor
            int i = Player.tileTargetX;
            int j = Player.tileTargetY;
            Tile tile = Main.tile[i, j];
            ref var data = ref tile.Get<MinesweeperData>();

            // Set the mine status
            data.MineStatus = data.HasOrAtLeastHadMine ? MineStatus.None : MineStatus.UnsolvedMine;

            // Update the mine count around this tile
            MinesweeperData.UpdateNumbersOfMines3x3(i, j);

            SoundEngine.PlaySound(SoundID.Grab, player.Center);
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            // Check if the player is the local player
            if (player.whoAmI != Main.myPlayer)
                return false;

            // Getting the tile under the cursor
            int i = Player.tileTargetX;
            int j = Player.tileTargetY;
            Tile tile = Framing.GetTileSafely(i, j);
            ref var data = ref tile.Get<MinesweeperData>();

            if ((!JJUtils.IsTileSolidForMine(tile) && !data.HasOrAtLeastHadMine) || !JJUtils.WithinPlacementRange(player, i, j))
            {
                return false;
            }
            return true;
        }

    }
}