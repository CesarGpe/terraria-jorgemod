using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace eslamio.Effects
{
    [Autoload(Side = ModSide.Client)]
    class SlimeShaderPlayer : ModPlayer
	{
		private bool WasActiveLastTick;
		public bool IsActive;
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

		/*public void SetSlime(float radius, float colorFadeDistance, float opacity) => SetSlime(radius, colorFadeDistance, opacity, Color.Black, Main.screenPosition);

		public void SetSlime(float radius, float colorFadeDistance, float opacity, Color color, Vector2 targetPosition)
		{
			Radius = radius;
			TargetPosition = targetPosition;
			FadeDistance = colorFadeDistance;
			Color = color;
			Opacity = opacity;
			IsActive = true;
		}*/

		public override void PostUpdateMiscEffects()
		{
			/*
			if (!IsActive)
			{
    			return;
			}
			*/

			//eslamio.slimeShader.UseColor(Color);
			//eslamio.slimeShader.UseIntensity(Opacity);
			//eslamio.slimeShader.Parameters["Radius"].SetValue(Radius);
			//eslamio.slimeShader.Parameters["FadeDistance"].SetValue(FadeDistance);
			Player.ManageSpecialBiomeVisuals("eslamio:SlimeShader", IsActive || WasActiveLastTick, TargetPosition);
		}
	}
}
