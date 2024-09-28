using System.Collections.Generic;
using Terraria.ID;

namespace eslamio.Content.Items.Consumables
{
    public class CesarSpawner : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 54;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.consumable = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item30;
            Item.makeNPC = ModContent.NPCType<NPCs.Cesar>();
        }

        public override void HoldItem(Player player)
        {
            Player.tileRangeX += 600;
            Player.tileRangeY += 600;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<NPCs.Cesar>());
        }

        public override void OnConsumeItem(Player player)
        {
            Main.NewText("<cesar> chale andaba bien agusto ahi", 255, 255, 255);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			foreach (var tooltip in tooltips)
			{
				if (tooltip.Name == "Consumable")
					tooltip.Hide();
			}
        }
    }
}