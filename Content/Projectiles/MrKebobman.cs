using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace eslamio.Content.Projectiles
{
	public class MrKebobman : ModProjectile
	{
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.scale = 0.75f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Confused, 600);
            target.AddBuff(BuffID.Bleeding, 600);
            target.AddBuff(BuffID.Ichor, 600);
            target.AddBuff(BuffID.BrokenArmor, 600);
            target.AddBuff(BuffID.WitheredArmor, 600);
            target.AddBuff(BuffID.CursedInferno, 600);
            target.AddBuff(BuffID.Poisoned, 600);
            target.AddBuff(BuffID.Venom, 600);
            target.AddBuff(BuffID.OnFire, 600);
            target.AddBuff(BuffID.OnFire3, 600);
            target.AddBuff(BuffID.Frostburn, 600);
            target.AddBuff(BuffID.Frostburn2, 600);
        }

        public override void AI()
        {
            int maxHeight = 20;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= maxHeight)
            {
                Projectile.velocity.Y += 0.4f;
                Projectile.velocity.X *= 0.97f;
            }
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for (int num631 = 0; num631 < 10; num631++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Stone, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default, 0.75f);
            }
        }
    }
}
