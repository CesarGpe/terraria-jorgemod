using Terraria.ID;
using Terraria.Localization;

namespace eslamio.Common.ModSystems;

public class RecipeSystem : ModSystem
{
    internal static RecipeGroup AnyCopperBar;
    internal static RecipeGroup AnySilverBar;
    internal static RecipeGroup AnyGoldBar;
    internal static RecipeGroup AnyDemoniteBar;
    internal static RecipeGroup AnyShadowScale;
    internal static RecipeGroup AnyCobaltBar;
    internal static RecipeGroup AnyMythrilBar;
    internal static RecipeGroup AnyAdamantiteBar;
    internal static RecipeGroup AnyGem;

    public override void Unload()
    {
        AnyCopperBar = null;
        AnySilverBar = null;
        AnyGoldBar = null;
        AnyDemoniteBar = null;
        AnyShadowScale = null;
        AnyCobaltBar = null;
        AnyMythrilBar = null;
        AnyAdamantiteBar = null;
    }

    public override void AddRecipeGroups()
    {
        AnyCopperBar = new RecipeGroup(() => GetText($"RecipeGroup.{nameof(AnyCopperBar)}"), 20, 703);
        AnySilverBar = new RecipeGroup(() => GetText($"RecipeGroup.{nameof(AnySilverBar)}"), 21, 705);
        AnyGoldBar = new RecipeGroup(() => GetText($"RecipeGroup.{nameof(AnyGoldBar)}"), 19, 706);
        AnyDemoniteBar = new RecipeGroup(() => GetText($"RecipeGroup.{nameof(AnyDemoniteBar)}"), 57, 1257);
        AnyShadowScale = new RecipeGroup(() => GetText($"RecipeGroup.{nameof(AnyShadowScale)}"), 86, 1329);
        AnyCobaltBar = new RecipeGroup(() => GetText($"RecipeGroup.{nameof(AnyCobaltBar)}"), 381, 1184);
        AnyMythrilBar = new RecipeGroup(() => GetText($"RecipeGroup.{nameof(AnyMythrilBar)}"), 382, 1191);
        AnyAdamantiteBar = new RecipeGroup(() => GetText($"RecipeGroup.{nameof(AnyAdamantiteBar)}"), 391, 1198);
        AnyGem = new RecipeGroup(() => GetText($"RecipeGroup.{nameof(AnyGem)}"), ItemID.Sapphire, ItemID.Ruby,
            ItemID.Emerald, ItemID.Topaz, ItemID.Amethyst, ItemID.Diamond, ItemID.Amber);

        AnyGoldBar = RecipeGroup.recipeGroups[RecipeGroup.RegisterGroup("GoldBar", AnyGoldBar)];
        AnySilverBar = RecipeGroup.recipeGroups[RecipeGroup.RegisterGroup("SilverBar", AnySilverBar)];
        AnyCopperBar = RecipeGroup.recipeGroups[RecipeGroup.RegisterGroup("CopperBar", AnyCopperBar)];
        AnyShadowScale = RecipeGroup.recipeGroups[RecipeGroup.RegisterGroup("ShadowScale", AnyShadowScale)];
        AnyDemoniteBar = RecipeGroup.recipeGroups[RecipeGroup.RegisterGroup("DemoniteBar", AnyDemoniteBar)];
        AnyCobaltBar = RecipeGroup.recipeGroups[RecipeGroup.RegisterGroup("CobaltBar", AnyCobaltBar)];
        AnyMythrilBar = RecipeGroup.recipeGroups[RecipeGroup.RegisterGroup("MythrilBar", AnyMythrilBar)];
        AnyAdamantiteBar = RecipeGroup.recipeGroups[RecipeGroup.RegisterGroup("AdamantiteBar", AnyAdamantiteBar)];
        AnyGem = RecipeGroup.recipeGroups[RecipeGroup.RegisterGroup("Gem", AnyGem)];
    }

    public static string GetText(string str, params object[] arg)
    {
        string text = Language.GetTextValue($"Mods.eslamio.{str}", arg);
        return text;
    }
}