using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using Terraria;
using Terraria.DataStructures;
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

        public override void Load()
        {
            Terraria.On_Player.ItemCheck_OwnerOnlyCode += VanillaItemCheck;
        }

        private void VanillaItemCheck(On_Player.orig_ItemCheck_OwnerOnlyCode orig, Player self, ref Player.ItemCheckContext context, Item sItem, int weaponDamage, Rectangle heldItemFrame)
        {
            if (sItem.type == ModContent.ItemType<RegrowthHamaxe>())
			{
				sItem.type = 5295;
				orig(self, ref context, sItem, weaponDamage, heldItemFrame);
				sItem.type = ModContent.ItemType<RegrowthHamaxe>();
			}
			else
				orig(self, ref context, sItem, weaponDamage, heldItemFrame);
        }

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
