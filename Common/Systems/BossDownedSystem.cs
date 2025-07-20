using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TutorialMod.Common.Systems
{
    public class BossDownedSystem : ModSystem
    {
        public static bool downedNineBoss = false;

        public override void ClearWorld()
        {
            downedNineBoss = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["downedNineBoss"] = downedNineBoss;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedNineBoss = tag.GetBool("downedNineBoss");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = downedNineBoss;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedNineBoss = flags[0];
        }
    }
}