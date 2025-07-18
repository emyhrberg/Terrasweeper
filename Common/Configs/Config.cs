using System;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

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

        /// <summary> Config instance helper. Example: Config.C.YourValue </summary>
        public static Config C => ModContent.GetInstance<Config>();
    }
}
