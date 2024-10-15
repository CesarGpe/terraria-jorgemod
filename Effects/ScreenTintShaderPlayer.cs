namespace eslamio.Effects;

[Autoload(Side = ModSide.Client)]
class UnusedScreenTintShaderPlayer : ModPlayer
{
    /*public bool IsActive;

    private float Red;
    private float Green;
    private float Blue;

    public override void ResetEffects()
    {
        IsActive = false;
        Red = 1f;
        Green = 1f;
        Blue = 1f;
    }

    public void SetColor(float red, float green, float blue)
    {
        IsActive = true;
        Red = red;
        Green = green;
        Blue = blue;
    }

    public override void PostUpdateMiscEffects()
    {
        eslamio.screenTintEffect.Parameters["Red"].SetValue(Red);
        eslamio.screenTintEffect.Parameters["Green"].SetValue(Green);
        eslamio.screenTintEffect.Parameters["Blue"].SetValue(Blue);
        Player.ManageSpecialBiomeVisuals("eslamio:ScreenTintShader", IsActive, Main.screenPosition);
    }*/
}
