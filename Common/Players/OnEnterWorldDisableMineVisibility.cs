using Terraria.ModLoader;
using Terrasweeper.Common.BuilderToggles;

namespace Terrasweeper.Common.Players
{
    public class OnEnterWorldDisableMineVisibility : ModPlayer
    {
        public override void OnEnterWorld()
        {
            // force set state to 1 (disabled) on world enter
            ModContent.GetInstance<ShowMinesBuilderToggle>().CurrentState = 1;
        }
    }
}
