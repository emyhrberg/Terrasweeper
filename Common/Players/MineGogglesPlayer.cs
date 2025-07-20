using Terraria;
using Terraria.ModLoader;
using Terrasweeper.Content.InfoDisplays;

namespace Terrasweeper.Common.Players
{
    /// <summary>
    /// ModPlayer class coupled with <seealso cref="MineRadarInfoDisplay"/> and Mine Radar accessory to show how to add a new info accessory.
    /// </summary>
    public class MineGogglesPlayer : ModPlayer
    {
        public bool active;

        public override void ResetEffects()
        {
            active = false;
        }
    }
}