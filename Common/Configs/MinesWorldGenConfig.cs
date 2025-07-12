using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace JulyJam.Common.Configs
{
    public class MinesWorldGenConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Worldgen")]
        [DefaultValue(10)]
        [Range(0, 10080000)]
        public int numMines;
    }
}
