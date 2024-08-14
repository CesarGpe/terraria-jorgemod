using eslamio.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Items.Weapons
{
	public class GravitySword : ModItem
	{
		public override void SetDefaults() {
			// Common Properties
			Item.width = 46;
			Item.height = 48;
			Item.rare = ItemRarityID.Quest;

			// Use Properties
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;

			// Weapon Properties
			Item.DamageType = DamageClass.Default;
			Item.damage = 1;
		}

        public override void HoldItem(Player player)
        {
    		player.AddBuff(ModContent.BuffType<GravBuff>(), 0);
		}

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.noGravity = true;
            target.GravityMultiplier *= 100;
        }
    }
}
