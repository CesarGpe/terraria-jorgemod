using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Items.Tools
{
	public class RegrowthHamaxe	 : ModItem
	{
		public override void SetDefaults() {

			Item.CloneDefaults(ItemID.AcornAxe);
			Item.hammer = 100;

			Item.width = 58;
			Item.height = 62;
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			foreach (var tooltip in tooltips)
			{
				if (tooltip.Name == "Placeable")
					tooltip.Hide();
			}
        }

		public override void MeleeEffects(Player player, Rectangle hitbox) {
			int poop = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.GrassBlades, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 0, default(Color), 1.2f);
			Main.dust[poop].noGravity = true;
		}

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.AcornAxe)
				.AddIngredient(ItemID.GoldHammer)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.AcornAxe)
				.AddIngredient(ItemID.PlatinumHammer)
				.AddTile(TileID.Anvils)
				.Register();
		}
    }
}
