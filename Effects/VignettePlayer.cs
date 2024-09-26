using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;

namespace eslamio.Effects
{
    [Autoload(Side = ModSide.Client)]
    class VignettePlayer : ModPlayer
	{
		private bool WasActiveLastTick;
		public static bool IsActive;
		private Vector2 TargetPosition;
		private float Opacity;
		private float Radius;
		private float FadeDistance;
		private Color Color;

		public override void ResetEffects()
		{
			WasActiveLastTick = IsActive;
			IsActive = false;
		}

		public void SetVignette(float radius, float colorFadeDistance, float opacity) => SetVignette(radius, colorFadeDistance, opacity, Color.Black, Main.screenPosition);

		public void SetVignette(float radius, float colorFadeDistance, float opacity, Color color, Vector2 targetPosition)
		{
			Radius = radius;
			TargetPosition = targetPosition;
			FadeDistance = colorFadeDistance;
			Color = color;
			Opacity = opacity;
			IsActive = true;
		}

		public override void PostUpdateMiscEffects()
		{
			eslamio.vignetteShader.UseColor(Color);
			eslamio.vignetteShader.UseIntensity(Opacity);
			eslamio.vignetteEffect.Parameters["Radius"].SetValue(Radius);
			eslamio.vignetteEffect.Parameters["FadeDistance"].SetValue(FadeDistance);
			Player.ManageSpecialBiomeVisuals("eslamio:Vignette", IsActive || WasActiveLastTick, TargetPosition);
		}
	}

	public class DopChase : ModSceneEffect
	{
		public override bool IsSceneEffectActive(Player player) => VignettePlayer.IsActive;
		public override int Music => MusicLoader.GetMusicSlot("eslamio/Assets/Music/DopChase");
		public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;
		public override string MapBackground => "Terraria/Images/MapBG25";
    }
}
