using Terraria.Graphics.Effects;
using ReLogic.Content;
using eslamio.Effects;
using eslamio.Core;

namespace eslamio;

public class eslamio : Mod
{
    public static Effect screenTintEffect;
    public static ScreenTintShader screenTintShader;

    public static Effect slimeEffect;
    public static SlimeShader slimeShader;

    public static Effect vignetteEffect;
    public static Vignette vignetteShader;

    public override void Load()
    {
        // All of this loading needs to be client-side.
        if (!Main.dedServ)
        {
            // basic shader for screen tints
            screenTintEffect = ModContent.Request<Effect>("eslamio/Effects/ScreenTintShader", AssetRequestMode.ImmediateLoad).Value;
            screenTintShader = new ScreenTintShader(screenTintEffect, "MainPS");
            Filters.Scene["eslamio:ScreenTintShader"] = new Filter(screenTintShader, (EffectPriority)100);
            //Filters.Scene["eslamio:BasicTintShader"] = new Filter(new BasicTintShader("FilterMiniTower").UseColor(0f, 0f, 255f).UseOpacity(0.001f), EffectPriority.VeryHigh);

            // jorge boss
            slimeEffect = ModContent.Request<Effect>("eslamio/Effects/SlimeShader", AssetRequestMode.ImmediateLoad).Value;
            slimeShader = new SlimeShader(slimeEffect, "MainPS");
            Filters.Scene["eslamio:SlimeShader"] = new Filter(slimeShader, (EffectPriority)100);

            // doppelganger vignette
            vignetteEffect = ModContent.Request<Effect>("eslamio/Effects/Vignette", AssetRequestMode.ImmediateLoad).Value;
            vignetteShader = new Vignette(vignetteEffect, "MainPS");
            Filters.Scene["eslamio:Vignette"] = new Filter(vignetteShader, (EffectPriority)100);
        }
    }
}