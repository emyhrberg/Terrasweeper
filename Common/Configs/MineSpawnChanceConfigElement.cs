using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader.Config.UI;
using Terraria.ModLoader.UI;

namespace Terrasweeper.Common.Configs
{
    class MineSpawnChanceConfigElement : FloatElement
    {
        public static MineSpawnChanceToggleConfigElement element;
        // ctor
        public MineSpawnChanceConfigElement() : base()
        {
            Min = 0f;
            Max = 100f;
            Increment = 1f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            backgroundColor = element.Value ? UICommon.DefaultUIBlue : Color.Gray;
            TextDisplayFunction = () => $"Mine Spawn Chance: {GetValue()}%";

        }
    }
}