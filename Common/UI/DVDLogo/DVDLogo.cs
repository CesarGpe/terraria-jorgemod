using eslamio.Content.Items.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace eslamio.Common.UI.DVDLogo
{
	internal class DVDLogo : UIState
	{
		private UIElement area;
		private UIImage logo;
		private UIText text;

		// dvd logo stuff
		Rectangle logoHitbox;
        readonly Random rnd = new();
		private bool init = false;
		private const float vel = 0.0025f; // fraction of screen width per step
		private float velX = 0;
		private float velY = 0;
		private int bounces = 0;
		private int cornerCount = 0;
		private readonly Color[] colors = [Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Purple];
		private Asset<Texture2D> Logo => ModContent.Request<Texture2D>("eslamio/Common/UI/DVDLogo/DVDLogo");
		//

		public override void OnInitialize()
		{
			// Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
			// UIElement is invisible and has no padding.
			area = new UIElement();
			area.Width.Set(Main.screenWidth, 0f);
			area.Height.Set(Main.screenHeight, 0f);
			area.Left.Set(0, 0);
			area.Top.Set(0, 0);

			logo = new UIImage(Logo);
			logo.Width.Set(138, 0f);
			logo.Height.Set(34, 0f);
			logo.Left.Set(-1000, 0f);
			logo.Top.Set(-1000, 0f);

			text = new UIText("Bounces: 0\nCorner hits: 0", 0.8f); // text to show stats
			text.Width.Set(138, 0f);
			text.Height.Set(34, 0f);
			text.Left.Set(-10, 0f);
			text.Top.Set(Main.screenHeight - 50, 0f);

			area.Append(logo);
			area.Append(text);
			Append(area);
		}

		public void InitDVDLogo()
		{
			logoHitbox = new Rectangle(0, 0, Utils.Width(Logo), Utils.Height(Logo));
			logoHitbox.Location = new Point(rnd.Next(0, Main.screenWidth - logoHitbox.Width), rnd.Next(0, Main.screenHeight - logoHitbox.Height));
				
			int markiplier = (rnd.Next(0, 2) == 0) ? 1 : -1;
			velX = velY = Math.Max(vel * Main.screenWidth, 1) * markiplier;

			init = true;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			// This prevents drawing unless we are using the DVD pet thing
			if (!Main.LocalPlayer.HasBuff(ModContent.BuffType<DVDPetBuff>()))
				return;

			base.Draw(spriteBatch);
		}

		// Here we draw our UI
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
		}

		public override void Update(GameTime gameTime)
		{
			if (!Main.LocalPlayer.HasBuff(ModContent.BuffType<DVDPetBuff>()))
				return;

			if (!init)
			{
				logoHitbox = new Rectangle(0, 0, Utils.Width(Logo), Utils.Height(Logo));
				logoHitbox.Location = new Point(rnd.Next(0, Main.screenWidth - logoHitbox.Width), rnd.Next(0, Main.screenHeight - logoHitbox.Height));
				
				int markiplier = (rnd.Next(0, 2) == 0) ? 1 : -1;
				velX = velY = Math.Max(vel * Main.screenWidth, 1) * markiplier;
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
					bounces++;
					count++;
				}
				if (logoHitbox.Location.Y < 0)
				{
					velY = Math.Abs(velY);
					bounces++;
					count++;
				}
				// bottom right corners
				if (logoHitbox.Location.X > Main.screenWidth - logoHitbox.Width)
				{
					velX = -Math.Abs(velX);
					bounces++;
					count++;
				}
				if (logoHitbox.Location.Y > Main.screenHeight - logoHitbox.Height)
				{
					velY = -Math.Abs(velY);
					bounces++;
					count++;
				}

				logo.Left.Set(logoHitbox.Location.X, 0f);
				logo.Top.Set(logoHitbox.Location.Y, 0f);
				logo.Color = colors[bounces % colors.Length];

				// hit corner
				if (count == 2)
					cornerCount++;

				text.SetText($"Bounces: {bounces}\nCorner hits: {cornerCount}");
			}

			base.Update(gameTime);
		}
	}

	// This class will only be autoloaded/registered if we're not loading on a server
	[Autoload(Side = ModSide.Client)]
	internal class DVDUISystem : ModSystem
	{
		private UserInterface DVDUserInterface;

		internal DVDLogo DVDLogo;

		public override void Load()
		{
			DVDLogo = new();
			DVDUserInterface = new();
			DVDUserInterface.SetState(DVDLogo);
		}

		public override void UpdateUI(GameTime gameTime)
		{
			DVDUserInterface?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int DVDLogoIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (DVDLogoIndex != -1)
			{
				layers.Insert(DVDLogoIndex, new LegacyGameInterfaceLayer(
					"eslamio: DVD Logo Pet",
					delegate
					{
						DVDUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
    }
}
