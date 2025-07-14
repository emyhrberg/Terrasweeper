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

        [CustomModConfigItem(typeof(MineSpawnChanceConfigElement))]
        [DefaultValue(10f)]
        public float MineSpawnChance;

        [DefaultValue(true)]
        public bool MinesEverywhere;

        /// <summary> Config instance helper. Example: Config.C.YourValue </summary>
        public static Config C => ModContent.GetInstance<Config>();
    }
}
