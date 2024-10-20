using System.IO;
using Terraria.ModLoader.IO;

namespace eslamio.Content.NPCs.Enemies;
public class DopFollowPlayer : ModPlayer
{
    /// <summary>
    ///     The value to multiply SpawnCondition.Cavern.Chance for to get the spawnrate of the Doppleganger.
    /// </summary>
    public float dopSpawnMultiplier = 0.02f;

    public override void ResetEffects()
    {
        if (dopSpawnMultiplier < 0.02f)
            dopSpawnMultiplier = 0.02f;

        Main.NewText($"dopSpawnMultiplier: {dopSpawnMultiplier}");
    }

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        ModPacket packet = Mod.GetPacket();
        packet.Write((byte)eslamio.MessageType.DopFollowState);
        packet.Write((byte)Player.whoAmI);
        packet.Write(dopSpawnMultiplier);
        packet.Send(toWho, fromWho);
    }

    public void ReceivePlayerSync(BinaryReader reader)
    {
        dopSpawnMultiplier = reader.Read();
    }

    public override void CopyClientState(ModPlayer targetCopy)
    {
        DopFollowPlayer clone = (DopFollowPlayer)targetCopy;
        clone.dopSpawnMultiplier = dopSpawnMultiplier;
    }

    public override void SendClientChanges(ModPlayer clientPlayer)
    {
        DopFollowPlayer clone = (DopFollowPlayer)clientPlayer;

        if (dopSpawnMultiplier != clone.dopSpawnMultiplier)
            SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
    }

    public override void SaveData(TagCompound tag)
    {
        tag["dopSpawnMultiplier"] = dopSpawnMultiplier;
    }

    public override void LoadData(TagCompound tag)
    {
        dopSpawnMultiplier = tag.GetFloat("dopSpawnMultiplier");
    }
}
