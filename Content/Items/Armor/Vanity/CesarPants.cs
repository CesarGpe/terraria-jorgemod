using Terraria.ID;

namespace eslamio.Content.Items.Armor.Vanity
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Legs)]
	public class CesarPants : ModItem
	{
		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 18;
			Item.rare = ItemRarityID.Expert;
			Item.vanity = true;
		}
	}
}
