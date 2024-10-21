using System.IO;

namespace eslamio.Content.NPCs.Enemies;
public class DopSkinSystem : ModSystem
{
    /// <summary>
    ///     The ID of the player the Doppleganger is currently copying.
    /// </summary>
    public static byte dopSkinID = 255;

    public override void ClearWorld()
    {
        dopSkinID = 255;
    }

    public override void NetSend(BinaryWriter writer)
    {
        writer.Write(dopSkinID);
    }
    public override void NetReceive(BinaryReader reader)
    {
        dopSkinID = reader.ReadByte();
    }
}
