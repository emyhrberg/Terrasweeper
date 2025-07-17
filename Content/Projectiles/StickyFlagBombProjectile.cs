using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Common.PacketHandlers;
using Terrasweeper.Common.Systems;
using Terrasweeper.Content.Dusts;

namespace Terrasweeper.Content.Projectiles
{
    public class StickyFlagBombProjectile : FlagBombProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.StickyBomb);
            Projectile.aiStyle = ProjAIStyleID.Explosive;
            Projectile.timeLeft = 180; // 60 ticks = 1 second
            Projectile.width = 22;
            Projectile.height = 30;
        }

        public override bool PreAI()
        {
            try
            {
                int num3 = (int)(Projectile.position.X / 16f) - 1;
                int num4 = (int)((Projectile.position.X + (float)Projectile.width) / 16f) + 2;
                int num5 = (int)(Projectile.position.Y / 16f) - 1;
                int num6 = (int)((Projectile.position.Y + (float)Projectile.height) / 16f) + 2;
                if (num3 < 0)
                    num3 = 0;

                if (num4 > Main.maxTilesX)
                    num4 = Main.maxTilesX;

                if (num5 < 0)
                    num5 = 0;

                if (num6 > Main.maxTilesY)
                    num6 = Main.maxTilesY;

                Vector2 vector = default(Vector2);
                for (int j = num3; j < num4; j++)
                {
                    for (int k = num5; k < num6; k++)
                    {
                        if (Main.tile[j, k] == null || !Main.tile[j, k].nactive() || !Main.tileSolid[Main.tile[j, k].type] || Main.tileSolidTop[Main.tile[j, k].type])
                            continue;

                        vector.X = j * 16;
                        vector.Y = k * 16;
                        if (!(Projectile.position.X + (float)Projectile.width - 4f > vector.X) || !(Projectile.position.X + 4f < vector.X + 16f) || !(Projectile.position.Y + (float)Projectile.height - 4f > vector.Y) || !(Projectile.position.Y + 4f < vector.Y + 16f))
                            continue;

                        Projectile.velocity.X = 0f;
                        Projectile.velocity.Y = -0.2f;
                    }
                }
            }
            catch
            {
            }
            return true;
        }  
        
    }
}
