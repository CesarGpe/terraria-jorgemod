using eslamio.Common;
using Terraria.DataStructures;
using Terraria.GameInput;

namespace eslamio.Content.Players;

public class KillbindPlayer : ModPlayer
{
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (KeybindSystem.KillBind.JustPressed)
            Player.KillMe(PlayerDeathReason.ByPlayerItem(Player.whoAmI, Player.HeldItem), 9999999999999999999, 1);
    }
}