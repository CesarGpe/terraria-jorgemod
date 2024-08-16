using eslamio.Common.ModSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Items.Weapons
{
	internal class PocketKnifeItem : ModItem
	{
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.AmethystHook);
			Item.shoot = ModContent.ProjectileType<PocketKnifeProjectile>();
			
			Item.DamageType = DamageClass.Generic;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noUseGraphic = true;
			Item.damage = 60;
			Item.shootSpeed = 20;
			Item.knockBack = 4;
			Item.rare = ItemRarityID.Pink;
		}

        public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.DualHook)
				.AddIngredient(ItemID.MythrilBar, 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.DualHook)
				.AddIngredient(ItemID.OrichalcumBar, 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
    }

	internal class PocketKnifeProjectile : ModProjectile
	{
		private static Asset<Texture2D> chainTexture;

		public override void Load() {
			chainTexture = ModContent.Request<Texture2D>("eslamio/Content/Items/Weapons/PocketKnifeChain");
		}

		public override void Unload() {
			chainTexture = null;
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
			Projectile.damage = 60;
			//Projectile.usesLocalNPCImmunity = true;
			//Projectile.localNPCHitCooldown = 0;
		}

        // Use this hook for hooks that can have multiple hooks mid-flight: Dual Hook, Web Slinger, Fish Hook, Static Hook, Lunar Hook.
        public override bool? CanUseGrapple(Player player) {
			int hooksOut = 0;
			for (int l = 0; l < 1000; l++) {
				if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == Projectile.type) {
					hooksOut++;
				}
			}

			return hooksOut <= 2;
		}

		// Amethyst Hook is 300, Static Hook is 600.
		public override float GrappleRange() { return 200f; }

		// The amount of hooks that can be shot out
		public override void NumGrappleHooks(Player player, ref int numHooks) { numHooks = 1; }

		// default is 11, Lunar is 24
		// How fast the grapple returns to you after meeting its max shoot distance
		public override void GrappleRetreatSpeed(Player player, ref float speed) { speed = 20f; }

		// How fast you get pulled to the grappling hook projectile's landing position
		public override void GrapplePullSpeed(Player player, ref float speed) { speed = 15; }

		// Draws the grappling hook's chain.
		public override bool PreDrawExtras() {
			Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
			Vector2 center = Projectile.Center;
			Vector2 directionToPlayer = playerCenter - Projectile.Center;
			float chainRotation = directionToPlayer.ToRotation() - MathHelper.PiOver2;
			float distanceToPlayer = directionToPlayer.Length();

			while (distanceToPlayer > 20f && !float.IsNaN(distanceToPlayer)) {
				directionToPlayer /= distanceToPlayer; // get unit vector
				directionToPlayer *= chainTexture.Height(); // multiply by chain link length

				center += directionToPlayer; // update draw position
				directionToPlayer = playerCenter - center; // update distance
				distanceToPlayer = directionToPlayer.Length();

				Color drawColor = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16));

				// Draw chain
				Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
					chainTexture.Value.Bounds, drawColor, chainRotation,
					chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
			}
			// Stop vanilla from drawing the default chain.
			return false;
		}
	}
}
