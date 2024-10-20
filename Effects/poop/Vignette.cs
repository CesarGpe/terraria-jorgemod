using Terraria.Graphics.Shaders;

namespace eslamio.Effects;

public class Vignette : ScreenShaderData
{
	public Vignette(Effect effect, string pass) : base(new Ref<Effect>(effect), pass)
	{
	}
}