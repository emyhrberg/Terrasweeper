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

            Log.Info("Config changed with new mines per 100 tiles:" + MinesPer100Tile);

            // Update ITD to the new mine spawn chance
            WorldgenMinesPass.PlaceMines((int) MinesPer100Tile);
        }

        public static Config C => ModContent.GetInstance<Config>();
    }
}
