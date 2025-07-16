using Terraria.ModLoader.Config.UI;

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