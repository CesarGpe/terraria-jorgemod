using Terraria.Graphics.CameraModifiers;
using System;

namespace eslamio.Effects;
/*
[Autoload(Side = ModSide.NoSync)]
class VignettePlayer : ModPlayer
{
    private bool WasActiveLastTick;
    private bool IsActive;

    private Vector2 TargetPosition;
    private float DesatLevel;
    private float Opacity;
    private float Radius;
    private float FadeDistance;
    private float ShakeIntensity;
    private Color Color;
    private float Time;

    public override void ResetEffects()
    {
        WasActiveLastTick = IsActive;
        IsActive = false;

        if (Time > 0f)
            Time -= 0.1f;
        else
            Time = 0f;
    }

    public void SetVignette(float radius, float colorFadeDistance, float opacity, Color color, Vector2 targetPosition, float desatLevel, float shakeIntensity)
    {
        Radius = radius;
        TargetPosition = targetPosition;
        FadeDistance = colorFadeDistance;
        DesatLevel = desatLevel;
        Color = color;
        Opacity = opacity;
        ShakeIntensity = shakeIntensity;
        IsActive = true;
        Time = 1f;
    }

    public override void PostUpdateMiscEffects()
    {
        eslamio.vignetteShader.UseColor(Color);
        eslamio.vignetteShader.UseIntensity(Opacity);
        eslamio.vignetteShader.UseProgress(Time);
        eslamio.vignetteEffect.Parameters["Radius"].SetValue(Radius);
        eslamio.vignetteEffect.Parameters["DesatLevel"].SetValue(DesatLevel);
        eslamio.vignetteEffect.Parameters["FadeDistance"].SetValue(FadeDistance);
        Player.ManageSpecialBiomeVisuals("eslamio:Vignette", IsActive || WasActiveLastTick, TargetPosition);

        if (IsActive || WasActiveLastTick)
        {
            PunchCameraModifier modifier = new(Player.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), ShakeIntensity, 6f, 10, 1f, FullName);
            Main.instance.CameraModifiers.Add(modifier);
        }
    }
}
*/