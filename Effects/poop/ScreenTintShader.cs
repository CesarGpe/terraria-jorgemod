using Terraria.Graphics.Shaders;

namespace eslamio.Effects;

public class ScreenTintShader : ScreenShaderData
{
	public ScreenTintShader(Effect effect, string pass) : base(new Ref<Effect>(effect), pass)
	{
	}
}