using Terraria.Graphics.Effects;
using Terraria.ID;
using ReLogic.Content;

namespace eslamio
{
	public class eslamio : Mod
	{
		public static Effect slimeEffect;
		public static SlimeShader slimeShader;

		public static Effect vignetteEffect;
        public static Vignette vignetteShader;

		public override void Load()
		{
			// All of this loading needs to be client-side.
			if (Main.netMode != NetmodeID.Server)
			{
				// jorge boss
				slimeEffect = ModContent.Request<Effect>("eslamio/Effects/SlimeShader", AssetRequestMode.ImmediateLoad).Value;
				slimeShader = new SlimeShader(slimeEffect, "MainPS");
				Filters.Scene["eslamio:SlimeShader"] = new Filter(slimeShader, (EffectPriority)100);

				// doppelganger vignette
				vignetteEffect = ModContent.Request<Effect>("eslamio/Effects/Vignette", AssetRequestMode.ImmediateLoad).Value;
				vignetteShader = new Vignette(vignetteEffect, "MainPS");
				Filters.Scene["eslamio:Vignette"] = new Filter(vignetteShader, (EffectPriority)100);

				// doppleganger desaturation effect
				//Asset<Effect> desatShader = Assets.Request<Effect>("Effects/Desaturation");
				//Filters.Scene["eslamio:Desaturation"] = new Filter(new ScreenShaderData(desatShader, "MainPS"), EffectPriority.Medium);

				// doppelganger glitch shader
				//Asset<Effect> glitchShader = Assets.Request<Effect>("Effects/GlitchShader");
				//GameShaders.Misc["eslamio:GlitchShader"] = new MiscShaderData(glitchShader, "P0");
			}
		}
	}
}