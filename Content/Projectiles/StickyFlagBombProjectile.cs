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
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = new(0, 0);
            return false; // make it stick to the tile by setting velocity 0
        }
    }
}
