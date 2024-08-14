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

		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{menuAssetPath}/logo");
		//public override Asset<Texture2D> Logo => base.Logo;
		public override Asset<Texture2D> SunTexture => ModContent.Request<Texture2D>($"{menuAssetPath}/cesarfumo");
		public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>($"{menuAssetPath}/iscaca");

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/menu");

		//public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<ExampleSurfaceBackgroundStyle>();

		public override string DisplayName => "JORGE MOD";

		public override void OnSelected() {
			//SoundEngine.PlaySound(SoundID.Item16); // Plays a thunder sound when this ModMenu is selected

			SoundStyle fartSound = SoundID.Item16;
			fartSound.Volume = 10f;
			SoundEngine.PlaySound(fartSound);
		}

		public override void OnDeselected() {
			//SoundEngine.PlaySound(SoundID.Item16); // Plays a thunder sound when this ModMenu is selected

			SoundStyle fartSound = SoundID.Item16;
			fartSound.Volume = 10f;
			SoundEngine.PlaySound(fartSound);
		}

		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor) {
			drawColor = Main.DiscoColor; // Changes the draw color of the logo
			return true;
		}
	}
}
