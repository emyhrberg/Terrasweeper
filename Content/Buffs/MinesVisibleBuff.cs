using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terrasweeper.Content.Buffs
{
    internal class MinesVisibleBuff : ModBuff
    {
        // public override LocalizedText Description => Language.GetText("BuffDescription." + Type);

        public override void Update(Player player, ref int buffIndex)
        {
            base.Update(player, ref buffIndex);

            // This buff makes mines visible to the player

        }
    }
}
