using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader.Config.UI;

namespace Terrasweeper.Common.Configs
{
    class MineSpawnChanceConfigElement : FloatElement
    {
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
            // TextDisplayFunction = () => $"{Language.GetTextValue("Mods.Terrasweeper.Configs.Config.MineSpawnChance.Label")}: {GetValue()}%";
            TextDisplayFunction = () => $"Mine Spawn Chance: {GetValue()}%";
        }
    }
}