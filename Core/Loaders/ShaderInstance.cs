using Terraria.Graphics.Shaders;

namespace eslamio.Core.Loaders;

public class ShaderInstance(Effect effect, string pass) : ScreenShaderData(new Ref<Effect>(effect), pass);