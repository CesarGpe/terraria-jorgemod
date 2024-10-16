namespace eslamio.Core;

public class PlayerCloneHelper
{
    public Player child;
    public Player ClonePlayer(Player parent)
    {
        child = new()
        {
            // use loadout
            CurrentLoadoutIndex = parent.CurrentLoadoutIndex,

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

            // player stats
            statDefense = parent.statDefense,
            statLifeMax = parent.statLifeMax,
        };
        child.PlayerFrame();

        return child;
    }
}
