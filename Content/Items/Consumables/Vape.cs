using Terraria.ID;

namespace eslamio.Content.Items.Consumables
{
	public class Vape : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 69;
		}

		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 38;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Green;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.consumable = true;
			Item.scale = 0.8f;
			Item.autoReuse = true;
		}

		public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer)
				Main.LocalPlayer.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason("Los pulmones de " + Main.LocalPlayer.name + " dejaron de funcionar."), 69420420f, 1);
			return true;
		}
	}
}