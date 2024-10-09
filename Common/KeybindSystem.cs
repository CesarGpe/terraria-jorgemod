using Terraria.ID;
using Terraria.Localization;

namespace eslamio.Common;
public class KeybindSystem : ModSystem
{
    public static ModKeybind KillBind { get; private set; }
    public static ModKeybind ExplodeNPCBind { get; private set; }
    public static ModKeybind CrashGameKey { get; private set; }

    public override void Load()
    {
        KillBind = KeybindLoader.RegisterKeybind(Mod, "KillBind", "K");
        ExplodeNPCBind = KeybindLoader.RegisterKeybind(Mod, "ExplodeNearestNPC", "L");
        CrashGameKey = KeybindLoader.RegisterKeybind(Mod, "CrashGameKey", "NumPad1");
    }
    public override void Unload()
    {
        KillBind = null;
        ExplodeNPCBind = null;
    }
}