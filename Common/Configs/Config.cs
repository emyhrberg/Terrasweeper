using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terrasweeper.Common.Systems;

namespace Terrasweeper.Common.Configs
{
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("WorldGen")]

        [CustomModConfigItem(typeof(MineSpawnChanceToggleConfigElement))]
        [DefaultValue(false)]
        public bool CustomMinePer100TilesValue;

        [CustomModConfigItem(typeof(MineSpawnChanceConfigElement))]
        [DefaultValue(12f)]
        public float MinesPer100Tile;

        public override void OnChanged()
        {
            base.OnChanged();

            float targetValue = CustomMinePer100TilesValue
                                    ? MinesPer100Tile
                                    : WorldgenMinesPass.MakeMineRatio();     // your default

            Log.Info("Config changed with new mine ratio: " + targetValue);

            MinesweeperWorldHelper.AdjustMineDensity(targetValue);          // ← incremental fix-up
        }

        public static Config C => ModContent.GetInstance<Config>();
    }
}
