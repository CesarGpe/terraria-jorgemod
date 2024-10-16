using Terraria.Graphics.Shaders;

namespace eslamio.Core;

public class BasicTintShader : ScreenShaderData
{
    public BasicTintShader(string passName) : base(passName)
    {
    }

    private void UpdateIndex()
    {
    }

    public override void Apply()
    {
        UpdateIndex();
        base.Apply();
    }
}