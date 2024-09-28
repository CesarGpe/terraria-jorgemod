using eslamio.Common.Players;
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
			Item.shoot = ModContent.ProjectileType<Projectiles.ShimmerGunProjectile>();
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
}
