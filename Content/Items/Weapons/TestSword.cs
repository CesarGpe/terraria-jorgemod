using eslamio.Content.Projectiles;
using System;
using Terraria.DataStructures;
using Terraria.ID;

namespace eslamio.Content.Items.Weapons
{
	public class TestSword : ModItem
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

            // Shoot
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 10f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            Vector2 heading = target - position;

            float shootSpeed = velocity.Length();
            heading.X *= shootSpeed * 0.00021f;
            heading.Y = -1;
            heading *= shootSpeed;

            var projType = Main.rand.NextBool(4) ? ModContent.ProjectileType<MrKebobman>() : ModContent.ProjectileType<OpilaBird>();
            Projectile.NewProjectile(source, position, heading, projType, damage, knockback, player.whoAmI);
            
            return false;
        }

        /*public override void HoldItem(Player player)
        {
    		player.AddBuff(ModContent.BuffType<GravBuff>(), 0);
			player.GetModPlayer<SlimeShaderPlayer>().IsActive = true;
		}*/

        /*public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.noGravity = true;
            target.GravityMultiplier *= 100;
        }*/
    }
}
