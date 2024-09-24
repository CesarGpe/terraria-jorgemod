using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Items.Weapons
{
	public class TestGun : ModItem
	{
		public override void SetDefaults() {
			// Common Properties
			Item.width = 62;
			Item.height = 32;
			Item.scale = 0.75f;
			Item.rare = ItemRarityID.Red;

			// Use Properties
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;

			Item.UseSound = SoundID.Item36;

			// Weapon Properties
			Item.DamageType = DamageClass.Default; // Sets the damage type to ranged.
			Item.damage = 1;
			Item.knockBack = 0f;
			Item.noMelee = true;

			// Gun Properties
			Item.shoot = ProjectileID.PurificationPowder;
			//Item.shoot = ModContent.ProjectileType<PlayerCloneProjectile>();
			//Item.shoot = ProjectileID.IvyWhip;
			Item.shootSpeed = 6f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<PlayerCloneProjectile>(), damage, knockback, player.whoAmI, 0, 0, 1);
			return false;
        }

    }

	internal class PlayerCloneProjectile : ModProjectile
	{
		public override string Texture => "eslamio/Content/Projectiles/ShimmerGunProjectile";
		public Player clone;

        public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.JavelinFriendly);
			AIType = ProjectileID.JavelinFriendly;
		}

        public override void AI()
        {
			if (Projectile.ai[2] == 1)
			{
				var player = Main.player[Projectile.owner];
				clone = (Player)player.Clone();
				clone.CurrentLoadoutIndex = player.CurrentLoadoutIndex;
				Projectile.ai[2] = 0;
			}
            base.AI();
        }

        public override bool PreDraw(ref Color lightColor)
        {
			//Player clone = Main.player[Projectile.owner];
			//Player clone = (Player)Main.player[Projectile.owner].Clone();
			Main.PlayerRenderer.DrawPlayer(Main.Camera, clone, Projectile.position, clone.fullRotation, clone.fullRotationOrigin, 0f);

            return false;
        }
    }

	internal class EpicHookProjectile : ModProjectile
	{
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft *= 10;
        }

        public override string Texture => "eslamio/Content/Items/Tools/FlyHookProjectile";

        public override void AI()
        {
            if (Main.player[Projectile.owner].dead || Main.player[Projectile.owner].stoned || Main.player[Projectile.owner].webbed || Main.player[Projectile.owner].frozen)
		{
			Projectile.Kill();
			return;
		}
		Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
		Vector2 vector = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
		float num = mountedCenter.X - vector.X;
		float num12 = mountedCenter.Y - vector.Y;
		float num13 = (float)Math.Sqrt(num * num + num12 * num12);
		Projectile.rotation = (float)Math.Atan2(num12, num) - 1.57f;
		if (Projectile.ai[0] == 2f && Projectile.type == 865)
		{
			float num14 = (float)Math.PI / 2f;
			int num15 = (int)Math.Round(Projectile.rotation / num14);
			Projectile.rotation = (float)num15 * num14;
		}
		if (Main.myPlayer == Projectile.owner)
		{
			int num16 = (int)(Projectile.Center.X / 16f);
			int num17 = (int)(Projectile.Center.Y / 16f);
			if (num16 > 0 && num17 > 0 && num16 < Main.maxTilesX && num17 < Main.maxTilesY && !Main.tile[num16, num17].IsActuated && TileID.Sets.CrackedBricks[Main.tile[num16, num17].TileType] && Main.rand.NextBool(16))
			{
				WorldGen.KillTile(num16, num17);
				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 20, num16, num17);
				}
			}
		}
		if (num13 > 2500f)
		{
			Projectile.Kill();
		}
		if (Projectile.type == 256)
		{
			Projectile.rotation = (float)Math.Atan2(num12, num) + 3.9250002f;
		}
		if (Projectile.type == 446)
		{
			Lighting.AddLight(mountedCenter, 0f, 0.4f, 0.3f);
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 28f)
			{
				Projectile.localAI[0] = 0f;
			}
			DelegateMethods.v3_1 = new Vector3(0f, 0.4f, 0.3f);
			Utils.PlotTileLine(Projectile.Center, mountedCenter, 8f, DelegateMethods.CastLightOpen);
		}
		if (Projectile.type == 652 && ++Projectile.frameCounter >= 7)
		{
			Projectile.frameCounter = 0;
			if (++Projectile.frame >= Main.projFrames[Projectile.type])
			{
				Projectile.frame = 0;
			}
		}
		if (Projectile.type >= 646 && Projectile.type <= 649)
		{
			Vector3 vector2 = Vector3.Zero;
			switch (Projectile.type)
			{
			case 646:
				vector2 = new Vector3(0.7f, 0.5f, 0.1f);
				break;
			case 647:
				vector2 = new Vector3(0f, 0.6f, 0.7f);
				break;
			case 648:
				vector2 = new Vector3(0.6f, 0.2f, 0.6f);
				break;
			case 649:
				vector2 = new Vector3(0.6f, 0.6f, 0.9f);
				break;
			}
			Lighting.AddLight(mountedCenter, vector2);
			Lighting.AddLight(Projectile.Center, vector2);
			DelegateMethods.v3_1 = vector2;
			Utils.PlotTileLine(Projectile.Center, mountedCenter, 8f, DelegateMethods.CastLightOpen);
		}
		if (Projectile.ai[0] == 0f)
		{
			if ((num13 > 300f && Projectile.type == 13) || (num13 > 400f && Projectile.type == 32) || (num13 > 440f && Projectile.type == 73) || (num13 > 440f && Projectile.type == 74) || (num13 > 375f && Projectile.type == 165) || (num13 > 350f && Projectile.type == 256) || (num13 > 500f && Projectile.type == 315) || (num13 > 550f && Projectile.type == 322) || (num13 > 400f && Projectile.type == 331) || (num13 > 550f && Projectile.type == 332) || (num13 > 400f && Projectile.type == 372) || (num13 > 300f && Projectile.type == 396) || (num13 > 550f && Projectile.type >= 646 && Projectile.type <= 649) || (num13 > 600f && Projectile.type == 652) || (num13 > 300f && Projectile.type == 865) || (num13 > 500f && Projectile.type == 935) || (num13 > 480f && Projectile.type >= 486 && Projectile.type <= 489) || (num13 > 500f && Projectile.type == 446))
			{
				Projectile.ai[0] = 1f;
			}
			else if (Projectile.type >= 230 && Projectile.type <= 235)
			{
				int num18 = 300 + (Projectile.type - 230) * 30;
				if (num13 > (float)num18)
				{
					Projectile.ai[0] = 1f;
				}
			}
			else if (Projectile.type == 753)
			{
				int num19 = 420;
				if (num13 > (float)num19)
				{
					Projectile.ai[0] = 1f;
				}
			}
			else if (ProjectileLoader.GrappleOutOfRange(num13, Projectile))
			{
				Projectile.ai[0] = 1f;
			}
			Vector2 vector3 = Projectile.Center - new Vector2(5f);
			Vector2 vector5 = Projectile.Center + new Vector2(5f);
			Point point = (vector3 - new Vector2(16f)).ToTileCoordinates();
			Point point4 = (vector5 + new Vector2(32f)).ToTileCoordinates();
			int num2 = point.X;
			int num3 = point4.X;
			int num4 = point.Y;
			int num5 = point4.Y;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num3 > Main.maxTilesX)
			{
				num3 = Main.maxTilesX;
			}
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.maxTilesY)
			{
				num5 = Main.maxTilesY;
			}
			Player player = Main.player[Projectile.owner];
			List<Point> list = new List<Point>();
			for (int i = 0; i < player.grapCount; i++)
			{
				Projectile projectile = Main.projectile[player.grappling[i]];
				if (projectile.aiStyle != 7 || projectile.ai[0] != 2f)
				{
					continue;
				}
				Point pt = projectile.Center.ToTileCoordinates();
				Tile tileSafely = Framing.GetTileSafely(pt);
				if (tileSafely.TileType != 314 && !TileID.Sets.Platforms[tileSafely.TileType])
				{
					continue;
				}
				for (int j = -2; j <= 2; j++)
				{
					for (int k = -2; k <= 2; k++)
					{
						Point point2 = new Point(pt.X + j, pt.Y + k);
						Tile tileSafely2 = Framing.GetTileSafely(point2);
						if (tileSafely2.TileType == 314 || TileID.Sets.Platforms[tileSafely2.TileType])
						{
							list.Add(point2);
						}
					}
				}
			}
			Vector2 vector4 = default(Vector2);
			for (int l = num2; l < num3; l++)
			{
				for (int m = num4; m < num5; m++)
				{
					/*if (Main.tile[l, m] == null)
					{
						Main.tile[l, m] = default(Tile);
					}*/
					vector4.X = l * 16;
					vector4.Y = m * 16;
					if (!(vector3.X + 10f > vector4.X) || !(vector3.X < vector4.X + 16f) || !(vector3.Y + 10f > vector4.Y) || !(vector3.Y < vector4.Y + 16f))
					{
						continue;
					}
					Tile tile = Main.tile[l, m];
					if ((bool)!GrappleCanLatchOnTo(Main.player[Projectile.owner], l, m) || list.Contains(new Point(l, m)) || (Projectile.type == 403 && tile.TileType != 314) || Main.player[Projectile.owner].IsBlacklistedForGrappling(new Point(l, m)))
					{
						continue;
					}
					if (Main.player[Projectile.owner].grapCount < 10)
					{
						Main.player[Projectile.owner].grappling[Main.player[Projectile.owner].grapCount] = Projectile.whoAmI;
						Main.player[Projectile.owner].grapCount++;
					}
					if (Main.myPlayer != Projectile.owner)
					{
						continue;
					}
					int num6 = 0;
					int num7 = -1;
					int num8 = 100000;
					if (Projectile.type == 73 || Projectile.type == 74)
					{
						for (int n = 0; n < 1000; n++)
						{
							if (n != Projectile.whoAmI && Main.projectile[n].active && Main.projectile[n].owner == Projectile.owner && Main.projectile[n].aiStyle == 7 && Main.projectile[n].ai[0] == 2f)
							{
								Main.projectile[n].Kill();
							}
						}
					}
					else
					{
						int num9 = 3;
						if (Projectile.type == 165)
						{
							num9 = 8;
						}
						if (Projectile.type == 256)
						{
							num9 = 2;
						}
						if (Projectile.type == 372)
						{
							num9 = 2;
						}
						if (Projectile.type == 652)
						{
							num9 = 1;
						}
						if (Projectile.type >= 646 && Projectile.type <= 649)
						{
							num9 = 4;
						}
						ProjectileLoader.NumGrappleHooks(Projectile, Main.player[Projectile.owner], ref num9);
						for (int num10 = 0; num10 < 1000; num10++)
						{
							if (Main.projectile[num10].active && Main.projectile[num10].owner == Projectile.owner && Main.projectile[num10].aiStyle == 7)
							{
								if (Main.projectile[num10].timeLeft < num8)
								{
									num7 = num10;
									num8 = Main.projectile[num10].timeLeft;
								}
								num6++;
							}
						}
						if (num6 > num9)
						{
							Main.projectile[num7].Kill();
						}
					}
					WorldGen.KillTile(l, m, fail: true, effectOnly: true);
					SoundEngine.PlaySound(SoundID.Item1, Main.player[Projectile.owner].position);
					Projectile.velocity.X = 0f;
					Projectile.velocity.Y = 0f;
					Projectile.ai[0] = 2f;
					Projectile.position.X = l * 16 + 8 - Projectile.width / 2;
					Projectile.position.Y = m * 16 + 8 - Projectile.height / 2;
					Rectangle? tileVisualHitbox = WorldGen.GetTileVisualHitbox(l, m);
					if (tileVisualHitbox.HasValue)
					{
						Projectile.Center = tileVisualHitbox.Value.Center.ToVector2();
					}
					Projectile.damage = 0;
					Projectile.netUpdate = true;
					if (Main.myPlayer == Projectile.owner)
					{
						if (Projectile.type == 935)
						{
							Main.player[Projectile.owner].DoQueenSlimeHookTeleport(Projectile.Center);
						}
						NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, Projectile.owner);
					}
					break;
				}
				if (Projectile.ai[0] == 2f)
				{
					break;
				}
			}
		}
		else if (Projectile.ai[0] == 1f)
		{
			float num11 = 11f;
			if (Projectile.type == 32)
			{
				num11 = 15f;
			}
			if (Projectile.type == 73 || Projectile.type == 74)
			{
				num11 = 17f;
			}
			if (Projectile.type == 315)
			{
				num11 = 20f;
			}
			if (Projectile.type == 322)
			{
				num11 = 22f;
			}
			if (Projectile.type >= 230 && Projectile.type <= 235)
			{
				num11 = 11f + (float)(Projectile.type - 230) * 0.75f;
			}
			if (Projectile.type == 753)
			{
				num11 = 15f;
			}
			if (Projectile.type == 446)
			{
				num11 = 20f;
			}
			if (Projectile.type >= 486 && Projectile.type <= 489)
			{
				num11 = 18f;
			}
			if (Projectile.type >= 646 && Projectile.type <= 649)
			{
				num11 = 24f;
			}
			if (Projectile.type == 652)
			{
				num11 = 24f;
			}
			if (Projectile.type == 332)
			{
				num11 = 17f;
			}
			ProjectileLoader.GrappleRetreatSpeed(Projectile, Main.player[Projectile.owner], ref num11);
			if (num13 < 24f)
			{
				Projectile.Kill();
			}
			num13 = num11 / num13;
			num *= num13;
			num12 *= num13;
			Projectile.velocity.X = num;
			Projectile.velocity.Y = num12;
		}
		else if (Projectile.ai[0] == 2f)
		{
			Point point3 = Projectile.Center.ToTileCoordinates();
			/*if (Main.tile[point3.X, point3.Y] == null)
			{
				Main.tile[point3.X, point3.Y] = default(Tile);
			}*/
			bool flag = true;
			if ((bool)!GrappleCanLatchOnTo(Main.player[Projectile.owner], point3.X, point3.Y))
			{
				flag = false;
			}
			if (flag)
			{
				Projectile.ai[0] = 1f;
			}
			else if (Main.player[Projectile.owner].grapCount < 10)
			{
				Main.player[Projectile.owner].grappling[Main.player[Projectile.owner].grapCount] = Projectile.whoAmI;
				Main.player[Projectile.owner].grapCount++;
			}
		}
        }
    }
}
