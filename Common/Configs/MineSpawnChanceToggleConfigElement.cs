using Microsoft.Extensions.Primitives;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader.Config.UI;
using Terraria.ModLoader.UI;

namespace Terrasweeper.Common.Configs
{
    class MineSpawnChanceToggleConfigElement : BooleanElement
    {

        public MineSpawnChanceToggleConfigElement() : base()
        {
            MineSpawnChanceConfigElement.element = this;
        }
    }
}