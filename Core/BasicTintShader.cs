using Terraria.Graphics.Shaders;

namespace eslamio.Core;

public class BasicTintShader : ScreenShaderData
{
    public BasicTintShader(string passName) : base(passName)
    {
    }

    private void UpdateSpookyIndex()
    {
    }

    public override void Apply()
    {
        UpdateSpookyIndex();
        base.Apply();
    }
}