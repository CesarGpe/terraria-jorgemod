using Terraria.ID;
using Terraria.Localization;

namespace eslamio.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class CacaCabeza : ModItem
	{
		public static readonly float DamageResistanceBonus = 1f;
		//public static readonly int AdditiveGenericDamageBonus = 20;

		public static readonly int MoveSpeedBonus = -101;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MoveSpeedBonus);

		public static LocalizedText SetBonusText { get; private set; }

		public override void SetStaticDefaults() {
			// If your head equipment should draw hair while drawn, use one of the following:
			// ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
			// ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
			// ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
			// ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true;

			SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs(DamageResistanceBonus);
			//SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs(AdditiveGenericDamageBonus);
		}

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(copper: 0); // How many coins the item is worth
			Item.rare = ItemRarityID.Orange; // The rarity of the item
			Item.defense = -32; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player) {
			player.moveSpeed += MoveSpeedBonus / 100f; // Increase the movement speed of the player
			player.stinky = true;
		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return head.type == ModContent.ItemType<CacaCabeza>() && body.type == ModContent.ItemType<CacaCuerpo>() && legs.type == ModContent.ItemType<CacaPiernas>();
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player) {
			player.setBonus = SetBonusText.Value;
			player.endurance = 1f;
			player.moveSpeed = 0f;
			player.maxRunSpeed = 0f;
			player.runAcceleration = 0f;
			player.dashType = 0;
			player.wings = 0;
			player.wingTime = 0;
			player.wingTimeMax = 0;
			player.wingAccRunSpeed = 0;
			player.accRunSpeed = 0;
			player.powerrun = false;
			player.noBuilding = true;
			player.blockExtraJumps = true;
			player.equippedWings = null;
			player.webbed = true;
			player.GetDamage(DamageClass.Generic) *= 0;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.PoopBlock, 32)
				.Register();
		}
	}
}
