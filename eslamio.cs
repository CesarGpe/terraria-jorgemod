using eslamio.Content.NPCs.Enemies;
using ReLogic.Content;
using System.IO;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace eslamio;
public class eslamio : Mod
{
    public const string BlankTexture = "eslamio/Assets/Textures/Blank";

    public override void Load()
    {
        // All of this loading needs to be client-side.
        if (!Main.dedServ)
        {
            // mod screen shaders
            RegisterSceneShader("eslamio/Effects/Trippy", "MainPS", "Trippy");
            RegisterSceneShader("eslamio/Effects/VignetteLight", "MainPS", "VignetteLight", Color.Black, 0.85f);
            RegisterSceneShader("eslamio/Effects/VignetteStrong", "MainPS", "VignetteStrong", Color.Black, 0.9f);

            // vanilla screen shaders
            Filters.Scene["eslamio:SlimeShader"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 255f).UseOpacity(0.0025f), EffectPriority.VeryHigh);
        }
    }

    public static void RegisterSceneShader(string path, string passName, string effectName)
    {
        var effect = ModContent.Request<Effect>(path, AssetRequestMode.ImmediateLoad);
        var shader = new ScreenShaderData(effect, passName);
        Filters.Scene[$"eslamio:{effectName}"] = new Filter(shader, (EffectPriority)100);
    }

    public static void RegisterSceneShader(string path, string passName, string effectName, Color useColor, float useIntensity)
    {
        var effect = ModContent.Request<Effect>(path, AssetRequestMode.ImmediateLoad);
        var shader = new ScreenShaderData(effect, passName);
        Filters.Scene[$"eslamio:{effectName}"] = new Filter(shader.UseColor(useColor).UseIntensity(useIntensity), (EffectPriority)100);
    }

    internal enum MessageType : byte
    {
        DopFollowState,
        DopSkinSync
    }

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        MessageType msgType = (MessageType)reader.ReadByte();

        switch (msgType)
        {
            case MessageType.DopFollowState:
                byte playerNumber = reader.ReadByte();
                DopFollowPlayer player = Main.player[playerNumber].GetModPlayer<DopFollowPlayer>();
                player.ReceivePlayerSync(reader);
                
                if (Main.netMode == NetmodeID.Server)
                {
                    // Forward the changes to the other clients
                    player.SyncPlayer(-1, whoAmI, false);
                }
                break;
            case MessageType.DopSkinSync:
                DopSkinSystem.dopSkinID = reader.ReadByte();
                break;
            default:
                Logger.WarnFormat("JORGEMOD: Unknown Message type: {0}", msgType);
                break;
        }
    }
}