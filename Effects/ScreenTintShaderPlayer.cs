namespace eslamio.Effects;

class ScreenTintShaderPlayer : ModPlayer
{
    public bool IsActive;

    private float Red;
    private float Green;
    private float Blue;
    //private float Intensity;

    public override void ResetEffects()
    {
        IsActive = false;
        Red = 1f;
        Green = 1f;
        Blue = 1f;
        //Intensity = 0f;
    }

    public void SetColor(float red, float green, float blue)
    {
        IsActive = true;
        Red = red;
        Green = green;
        Blue = blue;
        //Intensity = intensity;
    }

    public override void PostUpdateMiscEffects()
    {
        eslamio.screenTintShader.UseColor(Red, Green, Blue);
        //eslamio.screenTintShader.UseIntensity(Intensity);
        Player.ManageSpecialBiomeVisuals("eslamio:ScreenTintShader", IsActive, Main.screenPosition);
    }
}
