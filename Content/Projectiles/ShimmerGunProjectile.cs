using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Projectiles
{
	public class ShimmerGunProjectile : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.SlimeGun);
			AIType = ProjectileID.SlimeGun;
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {

			hit.Damage = 0;
			hit.SourceDamage = 0;
			hit.HideCombatText = true;

			if (target.life != target.lifeMax)
				target.life += 1;
			damageDone = 0;

			if (NPCID.Sets.ShimmerTownTransform[target.type])
			{
				// esto cambia al npc
				NPC.ShimmeredTownNPCs[target.type] = !NPC.ShimmeredTownNPCs[target.type];
				target.townNpcVariationIndex = target.IsShimmerVariant ? 0 : 1;

				// el resto son particulas
				Gore.NewGore(target.GetSource_Death(), target.position + new Vector2(0, 2), target.velocity, 11);
				Gore.NewGore(target.GetSource_Death(), target.position + new Vector2(0, 2), target.velocity, 12);
				Gore.NewGore(target.GetSource_Death(), target.position + new Vector2(0, 3), target.velocity, 13);
				Gore.NewGore(target.GetSource_Death(), target.position + new Vector2(0, 3), target.velocity, 11);

				SoundEngine.PlaySound(SoundID.Item176, target.position);
				SoundEngine.PlaySound(SoundID.Item29, target.position);
				SoundEngine.PlaySound(SoundID.Splash, target.position);
			}


		}
    }
}
