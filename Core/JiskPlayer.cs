using Terraria.DataStructures;

namespace eslamio.Core;
public class JiskPlayer : ModPlayer
{
    public float? DrawScale;
    public int? DrawForceDye;

    public void PreDraw(ref PlayerDrawSet info)
    {
        if (info.headOnlyRender)
        {
            return;
        }
        if (DrawScale != null)
        {
            var drawPlayer = info.drawPlayer;
            var to = new Vector2((int)drawPlayer.position.X + drawPlayer.width / 2f, (int)drawPlayer.position.Y + drawPlayer.height);
            to -= Main.screenPosition;
            for (int i = 0; i < info.DrawDataCache.Count; i++)
            {
                DrawData data = info.DrawDataCache[i];
                data.position -= (data.position - to) * (1f - DrawScale.Value);
                data.scale *= DrawScale.Value;
                info.DrawDataCache[i] = data;
            }
        }
        if (DrawForceDye != null)
        {
            var drawPlayer = info.drawPlayer;
            for (int i = 0; i < info.DrawDataCache.Count; i++)
            {
                DrawData data = info.DrawDataCache[i];
                data.shader = DrawForceDye.Value;
                info.DrawDataCache[i] = data;
            }
        }
    }
}