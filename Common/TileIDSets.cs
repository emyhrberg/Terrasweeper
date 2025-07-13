using Terraria.ModLoader;
using Terraria.ID;

namespace JulyJam.Common
{
    [ReinitializeDuringResizeArrays]
    public static class TileIDSets
    {
        public const string CanPlaceMineSetKey = "CanPlaceMine";

        public static bool[] CanPlaceMine = TileID.Sets.Factory.CreateNamedSet(CanPlaceMineSetKey)
            .Description("Tiles that can have mines")
            .RegisterBoolSet(true,
                TileID.DesertFossil
            );
    }
}
