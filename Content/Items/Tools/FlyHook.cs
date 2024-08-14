using System;
using System.Collections.Generic;
using eslamio.Common.ModSystems;
using eslamio.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Items.Tools
{
	internal class FlyHookItem : ModItem
	{
		public override void SetDefaults() {
			// Copy values from the Amethyst Hook
			Item.CloneDefaults(ItemID.AmethystHook);
			Item.shootSpeed = 6f; // This defines how quickly the hook is shot.
			Item.shoot = ModContent.ProjectileType<FlyHookProjectile>();
			Item.rare = ItemRarityID.Green;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.GrapplingHook)
				.AddIngredient(ItemID.Magiluminescence)
				.AddIngredient(ItemID.Feather, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	internal class FlyHookProjectile : ModProjectile
	{
		private static Asset<Texture2D> chainTexture;

		public override void Load() { // This is called once on mod (re)load when this piece of content is being loaded.
			// This is the path to the texture that we'll use for the hook's chain. Make sure to update it.
			chainTexture = ModContent.Request<Texture2D>("eslamio/Content/Items/Tools/FlyHookChain");
		}

		public override void Unload() { // This is called once on mod reload when this piece of content is being unloaded.
			// It's currently pretty important to unload your static fields like this, to avoid having parts of your mod remain in memory when it's been unloaded.
			chainTexture = null;
		}

		public override void SetStaticDefaults() {
			ProjectileID.Sets.SingleGrappleHook[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
			Projectile.extraUpdates = 2;
		}

        // Amethyst Hook is 300, Static Hook is 600.
        public override float GrappleRange() {
			return 400f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks) {
			numHooks = 0; // The amount of hooks that can be shot out
		}

		// default is 11, Lunar is 24
		public override void GrappleRetreatSpeed(Player player, ref float speed) {
			speed = 8f; // How fast the grapple returns to you after meeting its max shoot distance
		}

		public override bool? GrappleCanLatchOnTo(Player player, int x, int y)
		{
			Tile tile = Main.tile[x, y];
			if (Main.tile[x, y].HasTile && Main.tileSolid[tile.TileType] && Main.tile[x, y].HasUnactuatedTile || tile.TileType == TileID.MinecartTrack)
			{
				// vanilla code sets this to 1f to say the grappling hook is returning to the player
				Projectile.ai[0] = 1f;

				player.RefreshExtraJumps();
				player.wingTime = player.wingTimeMax + 100;
				player.rocketTime = player.rocketTimeMax;
				player.AddBuff(ModContent.BuffType<FlightMobilityBuff>(), 300);

				SoundStyle railSound = SoundID.Item52;
				railSound.Volume = 1.5f;

				SoundStyle starSound = SoundID.Item4;
				starSound.Volume = 0.25f;

				SoundEngine.PlaySound(SoundID.Item52, Projectile.position);
				SoundEngine.PlaySound(starSound, Projectile.position);

				for (int i = 0; i < 5; i++) {
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
					dust.velocity *= 2f;
					dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1f);
					dust.velocity *= 1.5f;
				}	

			}
			return false;
		}

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
