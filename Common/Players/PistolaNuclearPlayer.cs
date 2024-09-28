using Terraria.ID;

namespace eslamio.Common.Players
{
	public class UnusedPistolaNuclearPlayer : ModPlayer
	{
		public bool canHitNPC;

		public override void ResetEffects() {
			canHitNPC = false;
		}

        public override bool? CanHitNPCWithProj(Projectile proj, NPC target)
        {
            if (canHitNPC && target.townNPC && proj.type == ProjectileID.GrenadeI)
				return true;
			else
				return null;
        }
	}
}
