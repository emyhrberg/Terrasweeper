using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terrasweeper.Common.Players;
using Terrasweeper.Content.Items.Accessories;

namespace Terrasweeper.Content.InfoDisplays
{
    /// <summary>
    /// InfoDisplay that shows how many mines are nearby.
    /// <seealso cref="MineRadarInfoPlayer"/>
    /// <seealso cref="MineRadarInfoAccessoryItem"/>
    /// </summary>
    public class MineRadarInfoDisplay : InfoDisplay
    {
        public static LocalizedText MinesNearbyText { get; private set; }
        public static LocalizedText NoMinesText { get; private set; }

        public override void SetStaticDefaults()
        {
            MinesNearbyText = this.GetLocalization("MinesNearby");
            NoMinesText = this.GetLocalization("NoMines");
        }

        public override bool Active()
        {
            // Only show if the accessory is equipped
            //return Main.LocalPlayer.GetModPlayer<MineRadarInfoPlayer>().showMineRadar;
            // return true;
            // Show if the accessory is in inventory or equipped
            var player = Main.LocalPlayer;
            return player.HasItem(ModContent.ItemType<MineRadarInfoAccessoryItem>());
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            int mineCount = Main.LocalPlayer.GetModPlayer<MineRadarInfoPlayer>().mineRadarNearbyCount;

            if (mineCount > 0)
            {
                displayColor = Color.White;
                // displayShadowColor = Color.Black;
                // if minecount is exactly 1, remove the 's' from the text
                if (mineCount == 1)
                {
                    return MinesNearbyText.Format(mineCount).Replace("s", string.Empty); // "1 mine nearby"
                }
                return MinesNearbyText.Format(mineCount); // "5 mines nearby"
            }
            else
            {
                displayColor = InactiveInfoTextColor;
                return NoMinesText.Value; //  "No mines nearby"
            }
        }
    }
}