using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terrasweeper.Content.Items;
using Terrasweeper.Content.Projectiles;
using TutorialMod.Common.Systems;

namespace Terrasweeper.Content.NPCs.NineBoss
{
    [AutoloadBossHead]
    public class NineBoss : ModNPC
    {
        #region setup
        public enum NineBossState
        {
            Idle,
            Charging,
            ShootingMines
        }

        private NineBossState State
        {
            get => (NineBossState)NPC.ai[0];
            set => NPC.ai[0] = (int)value;
        }

        // values stored in NPC.ai
        private ref float Timer => ref NPC.ai[1];   // generic time in state
        private ref float ShotsFired => ref NPC.ai[2];

        private const string NumbersTexturePath = "Terrasweeper/Content/NPCs/NineBoss/NineBoss";   // sheet
                 
        // Movement tuning
        private const float DashSpeedClassic = 7f;
        private const float DashSpeedExpert = 9f;
        private const float HoverSpeed = 4f;
        private const float HoverAccel = 0.04f;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 9;

            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers drawMods = new()
            {
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawMods);
        }

        public override void SetDefaults()
        {
            // Hitbox
            NPC.width = 30;
            NPC.height = 30;

            // Damage and Defence
            NPC.damage = 32;
            NPC.defense = 15;

            // Maximum Health
            NPC.lifeMax = 9000;

            // Knockback Resistance
            NPC.knockBackResist = 0f;

            // Sounds
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            // Collision
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            // Boss Settings
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 10f;
        }

        #endregion

        #region AI
        public override void AI()
        {
            // Ensure we have an alive target or despawn
            if (!AcquireTargetOrDespawn()) return;

            Player player = Main.player[NPC.target];

            // Pick the behaviour function – one line, no break/continue clutter
            Action<Player> action = State switch
            {
                NineBossState.Idle => HandleIdle,
                NineBossState.Charging => HandleCharging,
                NineBossState.ShootingMines => HandleShootingMines,
                _ => static _ => { }   // safety fallback
            };

            action(player);
        }

        private void HandleIdle(Player player)
        {
            const float speed = 5f;
            const float accel = 0.05f;
            MoveToTarget(player, speed, accel, out _);

            Timer++;

            // every few seconds pick the next behaviour
            float threshold = Main.expertMode ? 180f : 240f;          // 3–4 s
            if (Timer >= threshold)
            {
                State = Main.rand.NextBool()
                    ? NineBossState.Charging
                    : NineBossState.ShootingMines;

                Timer = ShotsFired = 0;
                NPC.netUpdate = true;
            }
        }

        private void HandleCharging(Player player)
        {
            float dashSpeed = Main.expertMode ? DashSpeedExpert : DashSpeedClassic;
            const int dashTicks = 20;
            const int totalTicks = 60;

            if (Timer == 0)
            {
                Vector2 dir = Vector2.Normalize(player.Center - NPC.Center);
                NPC.velocity = dir * dashSpeed;
                NPC.netUpdate = true;
            }

            if (Timer > dashTicks)
                NPC.velocity *= 0.96f;

            if (++Timer >= totalTicks)
            {
                State = NineBossState.Idle;
                Timer = 0;
                NPC.netUpdate = true;
            }
        }

        private void HandleShootingMines(Player player)
        {
            MoveToTarget(player, HoverSpeed, HoverAccel, out _);

            const int shootInterval = 90;                 // classic
            const int shootIntervalExpert = 60;           // expert
            const int maxShots = 5;

            int tickGap = Main.expertMode ? shootIntervalExpert : shootInterval;

            if (++Timer % tickGap == 0)
            {
                float projSpeed = Main.expertMode ? 8f : 6f;
                int damage = (int)(NPC.damage * 0.60f);

                ShootProjectileToward(player,
                    ModContent.ProjectileType<MinedMineProjectile>(),
                    projSpeed,
                    damage,
                    3f);

                if (++ShotsFired >= maxShots)
                {
                    // done – back to idle
                    State = NineBossState.Idle;
                    Timer = ShotsFired = 0;
                    NPC.netUpdate = true;
                }
            }
        }

        private void ShootProjectileToward(Player player, int type, float speed, int dmg, float kb)
        {
            Vector2 dir = Vector2.Normalize(player.Center - NPC.Center) * speed;
            Vector2 spawn = NPC.Center + dir * 10f;

            if (Main.netMode != NetmodeID.MultiplayerClient)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), spawn, dir, type, dmg, kb, Main.myPlayer);
        }

        /// <summary>Accelerates the NPC toward <paramref name="player"/>.</summary>
        private void MoveToTarget(Player player, float topSpeed, float accel, out float distance)
        {
            distance = Vector2.Distance(NPC.Center, player.Center);
            if (distance == 0) return;

            Vector2 desired = (player.Center - NPC.Center) / distance * topSpeed;

            // Smoothly approach the desired velocity.
            if (NPC.velocity.X < desired.X) NPC.velocity.X += accel;
            if (NPC.velocity.X > desired.X) NPC.velocity.X -= accel;
            if (NPC.velocity.Y < desired.Y) NPC.velocity.Y += accel;
            if (NPC.velocity.Y > desired.Y) NPC.velocity.Y -= accel;
        }

        #endregion

        #region animation
        public override void FindFrame(int frameHeight)
        {
            // advance slightly faster when moving
            NPC.frameCounter += 0.15 + NPC.velocity.Length() * 0.02;

            if (NPC.frameCounter >= 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[Type])
                    NPC.frame.Y = 0;                          // loop back to first frame
            }
        }

        //  draw the life-digit overlay
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(NumbersTexturePath).Value;
            const int w = 30, h = 30;
            int digit = Utils.Clamp((NPC.life - 1) / 1000, 0, 8);     // 0-8

            Rectangle src = new(digit * w, 0, w, h);
            Vector2 pos = NPC.Center - screenPos + new Vector2(0, -NPC.height * 0.6f);

            spriteBatch.Draw(tex,
                             pos,
                             src,
                             drawColor,
                             0f,
                             new Vector2(w / 2f, h / 2f),
                             1f,
                             SpriteEffects.None,
                             0f);
        }
        #endregion

        private bool AcquireTargetOrDespawn()
        {
            if (NPC.target < 0 || NPC.target >= 255 || Main.player[NPC.target].dead)
                NPC.TargetClosest();

            Player p = Main.player[NPC.target];
            if (p.dead || !p.active)
            {
                NPC.velocity.Y -= 0.04f;
                NPC.EncourageDespawn(10);
                return false;
            }
            return true;
        }

        #region misc helpers and utility methods
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MinedMine>(), 
                chanceDenominator: 1, 
                minimumDropped: 10, 
                maximumDropped: 20));
        }

        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref BossDownedSystem.downedNineBoss, -1);
        }
        #endregion
    }
}