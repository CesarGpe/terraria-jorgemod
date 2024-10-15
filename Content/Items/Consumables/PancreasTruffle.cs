using eslamio.Content.Buffs;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace eslamio.Content.Items.Consumables;

public class PancreasTruffle : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 5;

        ItemID.Sets.FoodParticleColors[Item.type] = [
            new Color(95, 99, 255),
            new Color(203, 206, 255),
            new Color(210, 199, 170)
        ];
        //ItemID.Sets.IsFood[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.DefaultToFood(44, 32, BuffID.WellFed3, 28800); // 8 minutes: 8 * 60 * 60
        Item.value = Item.sellPrice(gold: 5);
        Item.rare = ItemRarityID.Orange;
        Item.width = 44;
        Item.height = 32;
    }

    public override void OnConsumeItem(Player player)
    {
        player.AddBuff(ModContent.BuffType<GlebaBuff>(), 3600);
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        foreach (var tooltip in tooltips)
        {
            if (tooltip.Name == "Consumable")
                tooltip.Text = Language.GetTextValue("Mods.eslamio.Items.PancreasTruffle.Consumable");
        }
    }
}