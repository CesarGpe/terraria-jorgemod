using Terraria.Graphics.CameraModifiers;
using System;

namespace eslamio.Effects
{
    [Autoload(Side = ModSide.Client)]
    class VignettePlayer : ModPlayer
	{
		private bool WasActiveLastTick;
		private bool IsActive;
		public bool sceneActive;
		public float shakeIntensity = 0f;

		private Vector2 TargetPosition;
		private float DesatLevel;
		private float Opacity;
		private float Radius;
		private float FadeDistance;
		private Color Color;

		public override void ResetEffects()
		{
			WasActiveLastTick = IsActive;
			IsActive = false;
			sceneActive = false;
		}

		public void SetVignette(float radius, float colorFadeDistance, float opacity) => SetVignette(radius, colorFadeDistance, opacity, Color.Black, Main.screenPosition, 0.5f);

		public void SetVignette(float radius, float colorFadeDistance, float opacity, Color color, Vector2 targetPosition, float desatLevel)
		{
			Radius = radius;
			TargetPosition = targetPosition;
			FadeDistance = colorFadeDistance;
			DesatLevel = desatLevel;
			Color = color;
			Opacity = opacity;
			IsActive = true;
		}

		public override void PostUpdateMiscEffects()
		{
			eslamio.vignetteShader.UseColor(Color);
			eslamio.vignetteShader.UseIntensity(Opacity);
			eslamio.vignetteEffect.Parameters["Radius"].SetValue(Radius);
			eslamio.vignetteEffect.Parameters["DesatLevel"].SetValue(DesatLevel);
			eslamio.vignetteEffect.Parameters["FadeDistance"].SetValue(FadeDistance);
			Player.ManageSpecialBiomeVisuals("eslamio:Vignette", IsActive || WasActiveLastTick, TargetPosition);

			if (IsActive || WasActiveLastTick)
			{
				PunchCameraModifier modifier = new(Player.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), shakeIntensity, 6f, 10, 1f, FullName);
				Main.instance.CameraModifiers.Add(modifier);
			}
		}
	}

	[Autoload(Side = ModSide.Client)]
	public class DopChase : ModSceneEffect
	{
		public override bool IsSceneEffectActive(Player player) => player.GetModPlayer<VignettePlayer>().sceneActive;
		public override int Music => MusicLoader.GetMusicSlot("eslamio/Assets/Music/DopChase");
		public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;
		public override string MapBackground => "Terraria/Images/MapBG25";
    }
}
