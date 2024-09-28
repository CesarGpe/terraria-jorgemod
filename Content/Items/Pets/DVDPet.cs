using Terraria.ID;

namespace eslamio.Content.Items.Pets
{
	public class DVDPetItem : ModItem
	{
		// Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.ZephyrFish); // Copy the Defaults of the Zephyr Fish Item.

			Item.shoot = ProjectileID.None; // "Shoot" your pet projectile.
			//Item.shoot = ModContent.ProjectileType<DVDPetProjectile>(); // "Shoot" your pet projectile.
			Item.buffType = ModContent.BuffType<DVDPetBuff>(); // Apply buff upon usage of the Item.
			Item.scale = 0.25f;
		}

        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer) {
				player.AddBuff(Item.buffType, 3600);
			}
   			return true;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.IronBar, 5)
				.AddIngredient(ItemID.Marble, 2)
				.AddIngredient(ItemID.Glass, 2)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.LeadBar, 5)
				.AddIngredient(ItemID.Marble, 2)
				.AddIngredient(ItemID.Glass, 2)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class DVDPetBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) { // This method gets called every frame your buff is active on your player.
			//bool unused = false;
			//player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<DVDPetProjectile>());
		}
	}

	public class DVDPetProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			/*Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;

			// This code is needed to customize the vanity pet display in the player select screen. Quick explanation:
			// * It uses fluent API syntax, just like Recipe
			// * You start with ProjectileID.Sets.SimpleLoop, specifying the start and end frames as well as the speed, and optionally if it should animate from the end after reaching the end, effectively "bouncing"
			// * To stop the animation if the player is not highlighted/is standing, as done by most grounded pets, add a .WhenNotSelected(0, 0) (you can customize it just like SimpleLoop)
			// * To set offset and direction, use .WithOffset(x, y) and .WithSpriteDirection(-1)
			// * To further customize the behavior and animation of the pet (as its AI does not run), you have access to a few vanilla presets in DelegateMethods.CharacterPreview to use via .WithCode(). You can also make your own, showcased in MinionBossPetProjectile
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
				.WithOffset(-10, 0f)
				.WithSpriteDirection(1)
				.WithCode(DelegateMethods.CharacterPreview.Float);*/
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;

			Projectile.width = 39;
			Projectile.height = 55;
		}

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];

			player.zephyrfish = false; // Relic from AIType

			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];

			// Keep the projectile from disappearing as long as the player isn't dead and has the pet buff.
			if (!player.dead && player.HasBuff(ModContent.BuffType<DVDPetBuff>())) {
				Projectile.timeLeft = 2;
			}
		}
	}
}
