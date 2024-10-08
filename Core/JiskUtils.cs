using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.ID;

namespace eslamio.Core;

[Autoload(Side = ModSide.Client)]
public class JiskUtils : ModSystem
{
    private static SlotId _currentSlotId;
    private static float _originalMusicVolume;
    private static bool _isPlayingSoundLastFrame;

    /// <summary>
    ///     Plays a sound while temporarily lowering the background music volume.
    ///     The background music volume is restored once the sound has finished playing.
    ///     This is useful for preventing dissonance when playing longer sounds that would overlap with background music.
    /// </summary>
    /// <param name="style">The sound style containing the parameters for the sound to be played.</param>
    /// <param name="volumeMultiplier">The multiplier for quieting the background music volume.</param>
    public static void PlaySoundOverBGM(in SoundStyle style, float volumeMultiplier = 0.45f, Vector2? position = null)
    {
        if (Main.netMode != NetmodeID.Server || !Main.dedServ)
        {
            var slotId = SoundEngine.PlaySound(style, position);
            if (Main.musicVolume <= 0) return;
            _originalMusicVolume = Main.musicVolume;
            _currentSlotId = slotId;

            float nVolume = Main.soundVolume * volumeMultiplier;
            //Main.musicVolume = Main.soundVolume * volumeMultiplier;
            Tween.To(() => Main.musicVolume, x => { Main.musicVolume = x; }, nVolume, 0.75f);
        }
    }

    public override void PostUpdateEverything()
    {
        if (!SoundEngine.TryGetActiveSound(_currentSlotId, out var activeSound))
        {
            if (!_isPlayingSoundLastFrame) return;
            _isPlayingSoundLastFrame = false;
            _currentSlotId = default;

            Tween.To(() => Main.musicVolume, x => { Main.musicVolume = x; }, _originalMusicVolume, 0.75f);

            return;
        }

        if (!activeSound.IsPlaying) return;
        _isPlayingSoundLastFrame = true;
    }
}