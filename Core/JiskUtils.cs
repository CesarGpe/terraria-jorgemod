using ReLogic.Utilities;
using Terraria.Audio;

namespace eslamio.Core;
public class JiskUtils : ModSystem
{
    /// <summary>
    ///     Returns true if an NPC of the specified type is within the given radius.
    /// </summary>
    /// <param name="npcType">The ID of the NPC to look for.</param>
    /// <param name="searchCenter">The center of the radius to search.</param>
    /// <param name="maxDetectDistance">The radius to search for the NPC.</param>
    public static bool NPCInDistance(int npcType, Vector2 searchCenter, float maxDetectDistance)
    {
        if (!NPC.AnyNPCs(npcType))
            return false;

        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;
        foreach (var npc in Main.ActiveNPCs)
        {
            if (npc.life < 0 || npc.type != npcType)
                continue;

            float sqrDistanceToTarget = Vector2.DistanceSquared(searchCenter, npc.Center);
            if (sqrDistanceToTarget < sqrMaxDetectDistance)
                return true;
        }
        return false;
    }

    /// <summary>
    ///     Returns a clone of the provided player.
    /// </summary>
    /// <param name="parent">The player to clone.</param>
    public static Player ClonePlayer(Player parent)
    {
        Player child = new()
        {
            // base player stuff
            name = parent.name,
            eyeColor = parent.eyeColor,
            hairColor = parent.hairColor,
            hairDyeColor = parent.hairDyeColor,
            pantsColor = parent.pantsColor,
            shirtColor = parent.shirtColor,
            shoeColor = parent.shoeColor,
            skinColor = parent.skinColor,
            underShirtColor = parent.underShirtColor,
            Male = parent.Male,
            skinVariant = parent.skinVariant,
            hairDye = parent.hairDye,
            hairDyeVar = parent.hairDyeVar,
            hair = parent.hair,

            // player equipment stuff
            armor = parent.armor,
            dye = parent.dye,
            miscEquips = parent.miscEquips,
            miscDyes = parent.miscDyes,
            hideVisibleAccessory = parent.hideVisibleAccessory,

            // dye stuff
            cHead = parent.cHead,
            cFace = parent.cFace,
            cFaceHead = parent.cFaceHead,
            cNeck = parent.cNeck,
            cBody = parent.cBody,
            cWaist = parent.cWaist,
            cLegs = parent.cLegs,
            cShoe = parent.cShoe,
            cFront = parent.cFront,
            cAngelHalo = parent.cAngelHalo,
            cBack = parent.cBack,
            cBackpack = parent.cBackpack,
            cBalloon = parent.cBalloon,
            cBalloonFront = parent.cBalloonFront,
            cBeard = parent.cBeard,
            cCarpet = parent.cCarpet,
            cFaceFlower = parent.cFaceFlower,
            cFlameWaker = parent.cFlameWaker,
            cFloatingTube = parent.cFloatingTube,
            cLeinShampoo = parent.cLeinShampoo,
            cSapling = parent.cSapling,
            cShield = parent.cShield,
            cShieldFallback = parent.cShieldFallback,
            cTail = parent.cTail,
            cWings = parent.cWings,
            cYorai = parent.cYorai,

            // player stats
            statDefense = parent.statDefense,
            statLifeMax = parent.statLifeMax,
        };
        child.PlayerFrame();

        return child;
    }

    private static SlotId _currentSlotId;
    private static Player playerToSound;
    private static float _originalMusicVolume;
    private static bool _isPlayingSoundLastFrame;

    /// <summary>
    ///     Plays a sound while temporarily lowering the background music volume.
    ///     The background music volume is restored once the sound has finished playing.
    ///     This is useful for preventing dissonance when playing longer sounds that would overlap with background music.
    /// </summary>
    /// <param name="style">The sound style containing the parameters for the sound to be played.</param>
    /// <param name="volumeMultiplier">The multiplier for quieting the background music volume.</param>
    /// <param name="client">The client to play the sound for.</param>
    public static void PlaySoundOverBGM(in SoundStyle style, float volumeMultiplier, Player client)
    {
        if (!Main.dedServ && Main.myPlayer == client.whoAmI)
        {
            playerToSound = client;

            var slotId = SoundEngine.PlaySound(style, playerToSound.position);
            if (Main.musicVolume <= 0) return;
            _originalMusicVolume = Main.musicVolume;
            _currentSlotId = slotId;

            float nVolume = Main.soundVolume * volumeMultiplier;
            Tween.To(() => Main.musicVolume, x => { Main.musicVolume = x; }, nVolume, 0.75f);
        }
    }

    public static bool UpdateProjActive(Projectile projectile, int buff)
    {
        if (!Main.player[projectile.owner].active || Main.player[projectile.owner].dead)
        {
            Main.player[projectile.owner].ClearBuff(buff);
            return false;
        }
        if (Main.player[projectile.owner].HasBuff(buff))
        {
            projectile.timeLeft = 2;
        }
        return true;
    }
    public static bool UpdateProjActive<T>(Projectile projectile) where T : ModBuff
    {
        return UpdateProjActive(projectile, ModContent.BuffType<T>());
    }
    public static bool UpdateProjActive(Projectile projectile, ref bool active)
    {
        if (Main.player[projectile.owner].dead)
            active = false;
        if (active)
            projectile.timeLeft = 2;
        return active;
    }

    public override void PostUpdateEverything()
    {
        if (!Main.dedServ && playerToSound is not null && Main.myPlayer == playerToSound.whoAmI)
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
}