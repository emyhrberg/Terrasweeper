using JulyJam.Common.PacketHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JulyJam.Common.Systems
{
    public class MinesweeperSyncSystem : ModSystem
    {
        public override void Load()
        {
            On_NetMessage.DecompressTileBlock_Inner += Hook_DecompressTileBlock_Inner;
        }

        public override void Unload()
        {
            On_NetMessage.DecompressTileBlock_Inner -= Hook_DecompressTileBlock_Inner;
        }

        private void Hook_DecompressTileBlock_Inner(On_NetMessage.orig_DecompressTileBlock_Inner orig, BinaryReader reader, int xStart, int yStart, int width, int height)
        {
            orig(reader, xStart, yStart, width, height);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModNetHandler.minesweeperPacketHandler.SendResyncRequest(xStart, yStart, width, height);
            }
        }
    }
}
