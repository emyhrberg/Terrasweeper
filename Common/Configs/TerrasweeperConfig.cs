using System;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Terrasweeper.Common.Configs
{
    public class TerrasweeperConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Debugging")]
        [DefaultValue(10)]
        [Range(0, 100)]
        public int WorldGenMineRatio;
    }

    public static class Conf
    {
        public static void Save()
        {
            try
            {
                ConfigManager.Save(C);
            }
            catch
            {
                Log.Error("An error occurred while manually saving ModConfig!.");
            }
        }

        // Instance of the Config class
        // Use it like 'Conf.C.YourConfigField' for easy access to the config values
        public static TerrasweeperConfig C
        {
            get
            {
                try
                {
                    return ModContent.GetInstance<TerrasweeperConfig>();
                }
                catch (Exception ex)
                {
                    Log.Error("Error getting config instance: " + ex.Message);
                    return null;
                }
            }
        }
    }
}

