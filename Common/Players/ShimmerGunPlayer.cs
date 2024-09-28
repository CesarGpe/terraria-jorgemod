using eslamio.Content.Projectiles;

namespace eslamio.Common.Players
{
	public class ShimmerGunPlayer : ModPlayer
	{
		public bool canHitNPC;

		public override void ResetEffects() {
			canHitNPC = false;
		}

        public override bool? CanHitNPCWithProj(Projectile proj, NPC target)
        {
            if (canHitNPC && target.townNPC && proj.type == ModContent.ProjectileType<ShimmerGunProjectile>())
				return true;
			else
				return null;
        }
	}
}
