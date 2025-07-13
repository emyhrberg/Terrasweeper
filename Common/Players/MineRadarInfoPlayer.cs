using JulyJam.Content.InfoDisplays;
using Terraria;
using Terraria.ModLoader;

namespace JulyJam.Common.Players
{
    /// <summary>
    /// ModPlayer class coupled with <seealso cref="MineRadarInfoDisplay"/> and Mine Radar accessory to show how to add a new info accessory.
    /// </summary>
    public class MineRadarInfoPlayer : ModPlayer
    {
        public bool showMineRadar; // UNUSED!

        // Count of mines nearby
        public int mineRadarNearbyCount;

        // Called every frame, even when paused, to reset info accessory flags
        public override void ResetInfoAccessories()
        {
            mineRadarNearbyCount = 0;
        }

        // Sync info accessory state from team players
        //public override void RefreshInfoAccessoriesFromTeamPlayers(Player otherPlayer)
        //{
        //    var other = otherPlayer.GetModPlayer<MineRadarInfoPlayer>();
        //    if (other.showMineRadar)
        //    {
        //        showMineRadar = true;
        //        mineRadarNearbyCount = other.mineRadarNearbyCount;
        //    }
        //}
    }
}