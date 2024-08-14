using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Items.Weapons
{
	public class NotGun : ModItem
	{
        // pistola que NO dispara

		public override void SetDefaults() {
			
			Item.DamageType = DamageClass.Melee;
			Item.damage = 28;
			Item.knockBack = 3;

			//Item.shoot = ProjectileID.Bullet;
			//Item.shootSpeed = 8f;

			Item.width = 42;
			Item.height = 30;

			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Swing;

			Item.value = 17500;
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item41;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Handgun)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}