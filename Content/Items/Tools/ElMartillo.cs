using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Items.Tools
{
	public class ElMartillo : ModItem
	{
		public override void SetDefaults() {
			Item.damage = 26;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 7.5f;
			Item.scale = 0.5f;

			Item.width = 217;
			Item.height = 318;

			Item.useTime = 1;
			Item.useAnimation = 27;
			Item.useStyle = ItemUseStyleID.Swing;

			Item.value = 10000;
			Item.rare = ItemRarityID.Green;

			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true; // Automatically re-swing/re-use this item after its swinging animation is over.

			//Item.axe = 30; // How much axe power the weapon has, note that the axe power displayed in-game is this value multiplied by 5
			Item.hammer = 100; // How much hammer power the weapon has
			Item.attackSpeedOnlyAffectsWeaponAnimation = true; // Melee speed affects how fast the tool swings for damage purposes, but not how fast it can dig
		}

		public override void MeleeEffects(Player player, Rectangle hitbox) {
			if (Main.rand.NextBool(2)) { // This creates a 1/10 chance that a dust will spawn every frame that this item is in its 'Swinging' animation.
				// Creates a dust at the hitbox rectangle, following the rules of our 'if' conditional.
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.MagicMirror);
			}
		}

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.GoldHammer)
				.AddIngredient(ItemID.TVHeadMask)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.PlatinumHammer)
				.AddIngredient(ItemID.TVHeadMask)
				.AddTile(TileID.Anvils)
				.Register();
		}
    }
}
