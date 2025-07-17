using Terraria;
using Terraria.ModLoader;
using Terrasweeper.Content.Items;

namespace Terrasweeper.Content.Buffs
{
    /// <summary>
    /// This buff is applied to the player when they use the <see cref="MinesweeperPotion"/> 
    /// </summary>
    internal class MinesVisibleBuff : ModBuff
    {
        // public override LocalizedText Description => Language.GetText("BuffDescription." + Type);

        public override void Update(Player player, ref int buffIndex)
        {
            base.Update(player, ref buffIndex);
        }
    }
}
