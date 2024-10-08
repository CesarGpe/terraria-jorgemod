using Terraria.ID;

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
			//if (Main.rand.NextBool(10))
			Dust.NewDustPerfect(Projectile.Center, DustID.InfernoFork);

            base.AI();
        }

    }
}
