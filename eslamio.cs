using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Content;
using Terraria;

namespace eslamio
{
	public class eslamio : Mod
	{
		public static SlimeShader slimeShader;
		public static Effect slimeEffect;

		public override void Load()
		{
			// All of this loading needs to be client-side.
			if (Main.netMode != NetmodeID.Server)
			{
				// jorge boss
				slimeEffect = ModContent.Request<Effect>("eslamio/Effects/SlimeShader", AssetRequestMode.ImmediateLoad).Value;
				slimeShader = new SlimeShader(slimeEffect, "MainPS");
				Filters.Scene["eslamio:SlimeShader"] = new Filter(slimeShader, (EffectPriority)100);

				// doppelganger glitch shader
				//Asset<Effect> glitchShader = Assets.Request<Effect>("Effects/GlitchShader");
				//GameShaders.Misc["eslamio:GlitchShader"] = new MiscShaderData(glitchShader, "P0");
			}
		}
	}
}