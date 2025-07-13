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
                TileID.Spikes,
                TileID.Cobweb,
                TileID.Vines,
                TileID.JungleVines,
                TileID.HallowedVines,
                TileID.BreakableIce,
                TileID.CrimsonVines,
                TileID.WoodenSpikes,
                TileID.VineFlowers,
                TileID.CrackedBlueDungeonBrick,
                TileID.CrackedGreenDungeonBrick,
                TileID.CrackedPinkDungeonBrick,
                TileID.MushroomVines,
                TileID.CorruptVines,
                TileID.AshVines,
                TileID.Cactus
            );
    }
}
