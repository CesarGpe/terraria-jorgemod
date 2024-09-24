using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content
{
	public class CustomModMenu : ModMenu
	{
		private const string menuAssetPath = "eslamio/Assets/Textures/Menu"; // Creates a constant variable representing the texture path, so we don't have to write it out multiple times
		private bool HasClicked;
        private float LogoSquishIntensity;
		private Vector2 logoCenter = Vector2.Zero;
		private static Asset<Texture2D> MenuTexture;
		public static readonly SoundStyle LogoClickSound = new("eslamio/Assets/Sounds/squish", SoundType.Sound);
		public static readonly SoundStyle selectSound = new("eslamio/Assets/Sounds/pam", SoundType.Sound);

		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{menuAssetPath}/logo");
		//public override Asset<Texture2D> Logo => base.Logo;
		//public override Asset<Texture2D> SunTexture => ModContent.Request<Texture2D>($"{menuAssetPath}/cesarfumo");
		//public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>($"{menuAssetPath}/iscaca");

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/menu");

		//public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<ExampleSurfaceBackgroundStyle>();

		// screen saver
		Rectangle logoHitbox;
		Random rnd = new Random();
		private bool init = false;
		private float vel = 0.003f; // fraction of screen width per step
        private float velX = 0;
        private float velY = 0;
		private int bounces = 0;
		private int cornerCount = 0;
		private Color[] colors = {Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Purple};
		//

		public override string DisplayName => "JORGE MENU";

		public override void OnSelected() {
			SoundEngine.PlaySound(selectSound, null);

			if (init)
			{
				logoHitbox.Location = new Point(rnd.Next(0, Main.screenWidth - logoHitbox.Width), rnd.Next(0, Main.screenHeight - logoHitbox.Height));

				int markiplier = (rnd.Next(0, 2) == 0) ? 1 : -1;
				velX = velY = Math.Max(vel * Main.screenWidth, 1) * markiplier;
			}
		}

		public override void OnDeselected() {
			SoundStyle fartSound = SoundID.Item16;
			fartSound.Volume = 10f;
			SoundEngine.PlaySound(fartSound);

			//un-hide the sun when this menu is switched
            Main.sunModY = 0;
		}

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
		{
			//drawColor = Main.DiscoColor;

			//hides the sun offscreen so you cant click it
            Main.sunModY = -300;

            //set daytime to true 
            Main.time = 27000;
            Main.dayTime = true;

			//draw the menu background
            MenuTexture ??= ModContent.Request<Texture2D>($"{menuAssetPath}/background");

			Vector2 drawOffset = Vector2.Zero;
            float xScale = (float)Main.screenWidth / MenuTexture.Width();
            float yScale = (float)Main.screenHeight / MenuTexture.Height();
            float scale = xScale;

			if (xScale != yScale)
            {
                if (yScale > xScale)
                {
                    scale = yScale;
                    drawOffset.X -= (MenuTexture.Width() * scale - Main.screenWidth - 10) * 0.5f;
                }
                else
                {
                    drawOffset.Y -= (MenuTexture.Height() * scale - Main.screenHeight) * 0.5f;
                }
            }

			spriteBatch.Draw(MenuTexture.Value, drawOffset, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

			//draw the actual menu logo
			if (!init)
			{
                logoHitbox = new Rectangle(0, 0, Utils.Width(Logo) + 600, Utils.Height(Logo) + 120);
                logoHitbox.Location = new Point(rnd.Next(0, Main.screenWidth - logoHitbox.Width), rnd.Next(0, Main.screenHeight - logoHitbox.Height));
                
				int markiplier = (rnd.Next(0, 2) == 0) ? 1 : -1;
				velX = velY = Math.Max(vel * Main.screenWidth, 1) * markiplier;
				
				logoCenter = logoHitbox.Center.ToVector2();
				init = true;
			}
			else
			{
				logoHitbox.X += (int)velX;
            	logoHitbox.Y += (int)velY;

				int count = 0;
				// top left corners
				if (logoHitbox.Location.X < 0)
				{
					velX = Math.Abs(velX);
					Bounce();
					count++;
				}
				if (logoHitbox.Location.Y < 0)
				{
					velY = Math.Abs(velY);
					Bounce();
					count++;
				}
				// bottom right corners
				if (logoHitbox.Location.X > Main.screenWidth - logoHitbox.Width)
				{
					velX = -Math.Abs(velX);
					Bounce();
					count++;
				}
				if (logoHitbox.Location.Y > Main.screenHeight - logoHitbox.Height)
				{
					velY = -Math.Abs(velY);
					Bounce();
					count++;
				}

				logoCenter = logoHitbox.Center.ToVector2();
				drawColor = colors[bounces % colors.Length];

				// hit corner
				if (count == 2)
				{
					cornerCount++;
				}
			}
			//logoCenter = logoDrawCenter;
            Main.EntitySpriteDraw(Logo.Value, logoCenter, new Rectangle(0, 0, Utils.Width(Logo), Utils.Height(Logo)), drawColor, 0, Utils.Size(Logo) / 2f, new Vector2(1f + LogoSquishIntensity, 1f - LogoSquishIntensity), SpriteEffects.None, 0);

			// draw stats
			Utils.DrawBorderString(spriteBatch, $"Bounces: {bounces}\nCorner hits: {cornerCount}", new Vector2(20, 20), Color.White);

			return false;
		}

		private void Bounce()
		{
			bounces++;

			
		}

        public override void Update(bool isOnTitleScreen)
        {
            LogoSquishIntensity *= 0.9f;
            if (Main.mouseLeft && !HasClicked &&
				Math.Abs(Main.MouseScreen.X - logoCenter.X) < 300f &&
				Math.Abs(Main.MouseScreen.Y - logoCenter.Y) < 70f)
            {
                LogoSquishIntensity = 1f;

                if (Math.Abs(LogoSquishIntensity) < 0.1f)
                {
                    LogoSquishIntensity = Math.Sign(LogoSquishIntensity) * 0.1f;
                }

                SoundEngine.PlaySound(LogoClickSound, null);
            }
            HasClicked = Main.mouseLeft;
        }
    }
}
