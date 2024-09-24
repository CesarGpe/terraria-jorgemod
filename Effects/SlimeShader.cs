using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

public class SlimeShader : ScreenShaderData
{
	public SlimeShader(Effect effect, string pass) : base(new Ref<Effect>(effect), pass)
	{
	}
}