using eslamio.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Items.Weapons
{
	public class ThrowingBlunts : ModItem
	{
		public override void SetDefaults() {
			// Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools

			// Common Properties
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(silver: 5);
			Item.maxStack = 9999;
			Item.width = 102;
			Item.height = 154;

			// Use Properties
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.consumable = true;

			// Weapon Properties			
			Item.damage = 20;
			Item.knockBack = 5f;
			Item.noUseGraphic = true; // The item should not be visible when used
			Item.noMelee = true; // The projectile will do the damage and not the item
			Item.DamageType = DamageClass.Ranged;

			// Projectile Properties
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<BluntProjectile>(); // The projectile that will be thrown
		}
	}
}