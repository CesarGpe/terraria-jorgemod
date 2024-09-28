using Terraria.ID;

namespace eslamio.Content.Items.Consumables
{
	public class Chocolate : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 5;
			
			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(113, 52, 40),
				new Color(92, 38, 30),
				new Color(69, 25, 18)
			};
			//ItemID.Sets.IsFood[Type] = true;
		}

		public override void SetDefaults() {
			// DefaultToFood sets all of the food related item defaults such as the buff type, buff duration, use sound, and animation time.
			Item.DefaultToFood(72, 60, BuffID.WellFed3, 57600); // 57600 is 16 minutes: 16 * 60 * 60
			Item.value = Item.buyPrice(0, 3);
			Item.rare = ItemRarityID.Orange;
			Item.width = 72;
			Item.height = 60;
		}

		// If you want multiple buffs, you can apply the remainder of buffs with this method.
		// Make sure the primary buff is set in SetDefaults so that the QuickBuff hotkey can work properly.
		public override void OnConsumeItem(Player player) {
			player.AddBuff(BuffID.SugarRush, 3600);
		}
	}
}