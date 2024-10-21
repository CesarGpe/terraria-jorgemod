using eslamio.Core;

namespace eslamio.Content.Players;

[Autoload(Side = ModSide.Client)]
public class CaveSounds : ModPlayer
{
    private int noiseTimer = 0;

    public void PlaySound()
    {
        noiseTimer = 0;

        bool choose = Main.rand.NextBool();
        int sound = Main.rand.Next(4);
        if (choose)
            JiskUtils.PlaySoundOverBGM(new($"eslamio/Assets/Sounds/Dop/CaveNoise{sound}"), 0.5f, Player);
        else
            JiskUtils.PlaySoundOverBGM(new($"eslamio/Assets/Sounds/Dop/Stalk{sound}"), 0.5f, Player);
    }

    public void PlaySound(int sound, bool stalkSound)
    {
        noiseTimer = 0;

        if (stalkSound)
            JiskUtils.PlaySoundOverBGM(new($"eslamio/Assets/Sounds/Dop/Stalk{sound}"), 0.5f, Player);
        else
            JiskUtils.PlaySoundOverBGM(new($"eslamio/Assets/Sounds/Dop/CaveNoise{sound}"), 0.5f, Player);
    }

    public override void PreUpdate()
    {
        if (!Main.dedServ)
        {
            if (noiseTimer >= 28800)
                PlaySound();
            else if (Player.ZoneDirtLayerHeight || Player.ZoneRockLayerHeight)
                noiseTimer += Main.rand.Next(2);
        }
    }
}