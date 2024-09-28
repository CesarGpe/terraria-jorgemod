using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;

namespace eslamio.Content.Items.Consumables
{
	public class Transmutator : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 69;
		}

		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 38;
			Item.maxStack = 9999;
			Item.value = Item.buyPrice(platinum: 1);
			Item.rare = ItemRarityID.Master;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
			Item.scale = 0.8f;
			Item.autoReuse = true;
		}

		public override bool CanUseItem(Player player) {
			return Main.LocalPlayer.inventory[0] != null 
				&& Main.LocalPlayer.inventory[0].type != ItemID.None 
				&& Main.LocalPlayer.inventory[0].stack == 1 
				&& !Main.LocalPlayer.inventory[0].favorited;
		}

		public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer) {
				
				if (Main.LocalPlayer.inventory[0].type < ItemLoader.ItemCount - 1)
				{
					//int prevStackPercent = Main.LocalPlayer.inventory[0].stack / Main.LocalPlayer.inventory[0].maxStack;
					Main.LocalPlayer.inventory[0].SetDefaults(Main.LocalPlayer.inventory[0].type + 1);
					//Main.LocalPlayer.inventory[0].stack = Main.LocalPlayer.inventory[0].maxStack * prevStackPercent;
					SoundEngine.PlaySound(SoundID.ResearchComplete, player.position);
				}
				else
				{
					ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral("No hay mas ID's para ciclar!"), Colors.RarityOrange, Main.LocalPlayer.whoAmI);
					Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].stack++;
					SoundEngine.PlaySound(SoundID.DoorOpen, player.position);
					SoundEngine.PlaySound(SoundID.MenuTick, player.position);
				}
			}
			return true;
		}
	}
}