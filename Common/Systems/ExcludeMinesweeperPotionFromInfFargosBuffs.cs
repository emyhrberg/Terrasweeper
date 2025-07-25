using Fargowiltas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terrasweeper.Content.Items;

namespace Terrasweeper.Common.Systems
{
    [JITWhenModsEnabled("Fargowiltas")]
    [ExtendsFromMod("Fargowiltas")]
    internal class ExcludeMinesweeperPotionFromInfFargosBuffs : ModSystem
    {
        public override void PostSetupContent()
        {
            FargoSets.Items.PotionCannotBeInfinite[ModContent.ItemType<MinesweeperPotion>()] = true;
        }
    }
}
