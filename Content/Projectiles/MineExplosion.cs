using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terrasweeper.Content.Projectiles;

public class MineExplosion : ModProjectile
{
    private const int RadiusPx = 300;

    public override void SetDefaults()
    {
        Projectile.width = RadiusPx * 2;
        Projectile.height = RadiusPx * 2;

        Projectile.damage = 5000;
        Projectile.knockBack = 3f;

        Projectile.hostile = true;
        Projectile.friendly = false;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 5;
        Projectile.DamageType = DamageClass.Generic;
        Projectile.aiStyle = -1;

        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;

        Projectile.hide = true;
    }

    public override void AI()
    {
        base.AI();
        if (Projectile.localAI[0] == 0f)
        {
            Projectile.localAI[0] = 1f;
            SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);

            for (int i = 0; i < 100; i++)
            {
                var d = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Smoke,
                    Main.rand.NextVector2Circular(20f, 20f));

                d.scale = 2f;
                d.noGravity = true;
            }
        }
    }
}
