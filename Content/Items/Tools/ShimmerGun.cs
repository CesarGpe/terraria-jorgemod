using Terraria.Audio;
using Terraria.ID;

namespace eslamio.Content.Items.Tools
{
	public class ShimmerGun : ModItem
	{
		public override void SetDefaults() {

			// clonar la pistola de slime
			Item.CloneDefaults(ItemID.SlimeGun);

			// Common Properties
			Item.width = 40;
			Item.height = 30;
			Item.rare = ItemRarityID.Expert;

			// Gun Properties
			Item.shoot = ModContent.ProjectileType<ShimmerGunProjectile>();
			Item.shootSpeed = 10f;

			// Weapon Properties
			Item.damage = 1;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.knockBack = 0f;
			Item.noMelee = true;

			// se consigue fulgoreando la pistola de slime
			ItemID.Sets.ShimmerTransformToItem[ItemID.SlimeGun] = Type;
		}

		public override Vector2? HoldoutOffset() {
			return new Vector2(2f, -2f);
		}

		public override void HoldItem(Player player) {
            player.GetModPlayer<ShimmerGunPlayer>().canHitNPC = true;
        }
	}

	public class ShimmerGunProjectile : ModProjectile
	{
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SlimeGun}";

        public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.SlimeGun);
			Projectile.aiStyle = ProjAIStyleID.WaterJet;
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
				target.netUpdate = true;
				target.netUpdate2 = true;

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
