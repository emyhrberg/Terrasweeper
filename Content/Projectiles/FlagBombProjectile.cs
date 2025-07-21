﻿using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Common.PacketHandlers;
using Terrasweeper.Common.Systems;
using Terrasweeper.Content.Dusts;

namespace Terrasweeper.Content.Projectiles
{
    public class FlagBombProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bomb);
            Projectile.aiStyle = ProjAIStyleID.Explosive;
            Projectile.timeLeft = 180; // 60 ticks = 1 second
            Projectile.width = 22;
            Projectile.height = 30;
        }

        public override void PrepareBombToBlow()
        {
            Projectile.Resize(128, 128);
            Projectile.damage = 100;
            Projectile.knockBack = 8f;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.TryGettingHitByOtherPlayersExplosives();

            int blastRadius = 5;
            int minI = (int)(Projectile.Center.X / 16f - (float)blastRadius);
            int maxI = (int)(Projectile.Center.X / 16f + (float)blastRadius);
            int minJ = (int)(Projectile.Center.Y / 16f - (float)blastRadius);
            int maxJ = (int)(Projectile.Center.Y / 16f + (float)blastRadius);
            if (minI < 0) minI = 0;
            if (maxI > Main.maxTilesX) maxI = Main.maxTilesX;
            if (minJ < 0) minJ = 0;
            if (maxJ > Main.maxTilesY) maxJ = Main.maxTilesY;
            MarkTiles(Projectile.Center, blastRadius, minI, maxI, minJ, maxJ);

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
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
            for (int i = 0; i < 8; i++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<FlagDust>());
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

        private void MarkTiles(Vector2 compareSpot, int radius, int minI, int maxI, int minJ, int maxJ)
        {
            for (int i = minI; i <= maxI; i++)
            {
                for (int j = minJ; j <= maxJ; j++)
                {
                    float distX = i - compareSpot.X / 16f;
                    float distY = j - compareSpot.Y / 16f;
                    if (!(distX * distX + distY * distY <= radius * radius) || !JJUtils.IsTileSolidForMine(i, j))
                    {
                        continue;
                    }
                    Tile tile = Framing.GetTileSafely(i, j);
                    ref var data = ref tile.Get<MinesweeperData>();
                    if (data.MineStatus == MineStatus.UnsolvedMine)
                    {
                        data.MineStatus = MineStatus.Solved;
                        data.HasFlag = false;
                        ModNetHandler.minesweeperPacketHandler.SendSingleTile(i, j);
                    }
                }
            }
        }
    }
}
