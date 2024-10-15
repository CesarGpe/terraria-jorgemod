using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace eslamio.Content.Items.Consumables;

public class Pancreas : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 5;

        ItemID.Sets.FoodParticleColors[Item.type] = [
            new Color(232, 198, 100),
            new Color(172, 130, 26),
            new Color(120, 61, 4)
        ];
        //ItemID.Sets.IsFood[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.DefaultToFood(40, 22, BuffID.Poisoned, 1800); // 30 seconds: 30 * 60
        Item.value = Item.sellPrice(gold: 3);
        Item.rare = ItemRarityID.Orange;
        Item.width = 40;
        Item.height = 22;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        foreach (var tooltip in tooltips)
        {
            if (tooltip.Name == "Consumable")
                tooltip.Text = Language.GetTextValue("Mods.eslamio.Items.Pancreas.Consumable");
        }
    }
}