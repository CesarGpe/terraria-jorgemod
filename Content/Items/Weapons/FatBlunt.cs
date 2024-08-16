using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Items.Weapons
{
	public class FatBlunt : ModItem
	{
		public override void SetDefaults() {
			Item.damage = 20;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 10;
			//Item.scale = 2f;

			Item.width = 100;
			Item.height = 100;

			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Swing;
			
			Item.value = 10000;
			Item.rare = ItemRarityID.Orange;

			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
		}

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
			if (Main.rand.NextBool(8)) { // This creates a 1/10 chance that a dust will spawn every frame that this item is in its 'Swinging' animation.
				// Creates a dust at the hitbox rectangle, following the rules of our 'if' conditional.
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.FlameBurst);
			}
        }

    }
}