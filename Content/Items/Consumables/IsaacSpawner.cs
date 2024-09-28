using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ID;

namespace eslamio.Content.Items.Consumables
{
	public class IsaacSpawner : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 37;
			Item.height = 54;
			Item.scale = 2f;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Orange;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.consumable = true;
			Item.noMelee = true;
			//Item.noUseGraphic = true;
			Item.UseSound = new SoundStyle("eslamio/Assets/Sounds/BadToTheBone");
			Item.makeNPC = ModContent.NPCType<NPCs.Isaac>();
		}

		public override void HoldItem(Player player)
		{
			Player.tileRangeX += 600;
			Player.tileRangeY += 600;
		}

		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(ModContent.NPCType<NPCs.Isaac>());
		}

		public override void OnConsumeItem(Player player)
		{
			Main.NewText("THE SKELETON APPEARS", 255, 255, 255);
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