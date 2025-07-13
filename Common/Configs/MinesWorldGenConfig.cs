using System;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace JulyJam.Common.Configs
{
    public class MinesWorldGenConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;


        [Header("Debugging")]
        [DefaultValue(10)]
        [Range(0, 100)]
        public int ratioOfMines;

        [DefaultValue(false)]
        public bool showMines;

        [Header("Minesweeper")]
        [Range(0, 100)]
        [DefaultValue(25)]
        public int ElementsTransparentsy;
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
        public static MinesWorldGenConfig C
        {
            get
            {
                try
                {
                    return ModContent.GetInstance<MinesWorldGenConfig>();
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

