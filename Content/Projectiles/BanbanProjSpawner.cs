using Terraria.DataStructures;

namespace eslamio.Content.Projectiles
{
	public class BanbanProjSpawner : ModProjectile
	{
        public override string Texture => "eslamio/Assets/Textures/Blank";

        public override void SetDefaults()
        {
            Projectile.timeLeft = 100;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity.X *= Projectile.velocity.Length() * 0.001f;
            Projectile.velocity.Y = -2.35f;
            Projectile.velocity *= Projectile.velocity.Length();

            var projType = Main.rand.NextBool(3) ? ModContent.ProjectileType<MrKebobman>() : ModContent.ProjectileType<OpilaBird>();
            Projectile.NewProjectile(source, Projectile.position, Projectile.velocity, projType, Projectile.damage, Projectile.knockBack, default);
        }

        public override void AI()
        {
            Projectile.Kill();
        }
    }
}
