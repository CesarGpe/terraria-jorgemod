using Terraria.Graphics.Shaders;

namespace eslamio.Effects;

public class SlimeShader : ScreenShaderData
{
	public SlimeShader(Effect effect, string pass) : base(new Ref<Effect>(effect), pass)
	{
	}
}