using Terraria.ID;

namespace eslamio.Content.Items.Tools
{
	public class InstaPickaxe : ModItem
	{
		public override void SetDefaults() 
		{
			Item.damage = -1;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 1;
			Item.useAnimation = 5;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.attackSpeedOnlyAffectsWeaponAnimation = true;

			Item.pick = 64;
		}

        public override void HoldItem(Player player)
        {
            player.pickSpeed -= 1000f;
        }

        public override void UpdateInventory(Player player)
        {
            if (NPC.downedMoonlord)
				Item.pick = 230;
			else if (NPC.downedGolemBoss)
				Item.pick = 210;
            else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                Item.pick = 200;
            else if (NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3)
				Item.pick = 180;
			else if (Main.hardMode)
				Item.pick = 100;
			else if (NPC.downedBoss2)
				Item.pick = 99;
        }

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.CopperPickaxe)
				.AddIngredient(ItemID.AncientChisel)
				.AddTile(TileID.Anvils)
				.Register();
		}
    }
}
