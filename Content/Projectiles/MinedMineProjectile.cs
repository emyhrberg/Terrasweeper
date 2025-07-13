using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Content.Dusts;

namespace Terrasweeper.Content.Projectiles
{
    public class MinedMineProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Explosive[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 900;
        }

        public override void AI()
        {
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.PrepareBombToBlow();
            }

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 10f)
            {
                Projectile.ai[0] = 10f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X *= 0.96f;
                    if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y += 0.2f;
            }

            Projectile.rotation += Projectile.velocity.X * 0.1f;

            if (Projectile.velocity.X == 0)
            {
                Projectile.ai[1] += 1;
                if (Projectile.ai[1] == 180)
                {
                    // Increase damage after 3 seconds after moving (hopefully)
                    SoundEngine.PlaySound(SoundID.Item53, Projectile.Center);
                    Projectile.damage = 120;
                }
            }
        }

        public override void PrepareBombToBlow()
        {
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.Resize(64, 64);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0, 0, 100, default(Color), 1.5f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0, 0, 100, default(Color), 2.5f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[dustIndex].velocity *= 3f;
            }
            int goreIndex = Gore.NewGore(Projectile.Center, default(Vector2), Main.rand.Next(61, 64));
            Main.gore[goreIndex].velocity *= 0.4f;
            Main.gore[goreIndex].velocity.X += 1f;
            Main.gore[goreIndex].velocity.Y += 1f;
            goreIndex = Gore.NewGore(Projectile.Center, default(Vector2), Main.rand.Next(61, 64));
            Main.gore[goreIndex].velocity *= 0.4f;
            Main.gore[goreIndex].velocity.X -= 1f;
            Main.gore[goreIndex].velocity.Y += 1f;
            goreIndex = Gore.NewGore(Projectile.Center, default(Vector2), Main.rand.Next(61, 64));
            Main.gore[goreIndex].velocity *= 0.4f;
            Main.gore[goreIndex].velocity.X += 1f;
            Main.gore[goreIndex].velocity.Y -= 1f;
            goreIndex = Gore.NewGore(Projectile.Center, default(Vector2), Main.rand.Next(61, 64));
            Main.gore[goreIndex].velocity *= 0.4f;
            Main.gore[goreIndex].velocity.X -= 1f;
            Main.gore[goreIndex].velocity.Y -= 1f;
        }
    }
}
