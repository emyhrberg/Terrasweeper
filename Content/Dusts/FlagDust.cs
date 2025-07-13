using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Terrasweeper.Content.Dusts
{
    public class FlagDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, 0, 16, 16);
        }

        public override bool Update(Dust dust)
        {
            //return true;
            dust.position += dust.velocity;
            dust.alpha += 5;
            if (dust.alpha >= 255) dust.active = false;
            return false;
        }
    }
}
