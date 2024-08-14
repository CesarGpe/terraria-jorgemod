﻿using eslamio.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
