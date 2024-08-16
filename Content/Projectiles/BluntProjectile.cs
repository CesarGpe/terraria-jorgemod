using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Projectiles
{
	public class BluntProjectile : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.JavelinFriendly);
			AIType = ProjectileID.JavelinFriendly;
		}
		
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(BuffID.OnFire3, 600);
		}

        public override void AI()
        {
			if (Main.rand.NextBool(10))
				Dust.NewDustPerfect(Projectile.Center, DustID.FlameBurst);

            base.AI();
        }

    }
}
