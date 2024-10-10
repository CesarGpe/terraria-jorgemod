using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.Audio;
using Terraria.ID;

namespace eslamio.Content.Items.Tools
{
    public class ShimmerGun : ModItem
    {
        public override void SetDefaults()
        {
            // clonar la pistola de slime
            Item.CloneDefaults(ItemID.SlimeGun);

            // Common Properties
            Item.width = 40;
            Item.height = 30;
            Item.rare = ItemRarityID.Expert;

            // Gun Properties
            Item.shoot = ModContent.ProjectileType<ShimmerGunProjectile>();
            Item.shootSpeed = 10f;

            // Weapon Properties
            Item.damage = 1;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.knockBack = 0f;
            Item.noMelee = true;

            // se consigue fulgoreando la pistola de slime
            ItemID.Sets.ShimmerTransformToItem[ItemID.SlimeGun] = Type;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.SlimeGun;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (var tooltip in tooltips)
            {
                if (tooltip.Name == "Placeable" || tooltip.Name == "Damage" || tooltip.Name == "Speed" || tooltip.Name == "Knockback" )
                    tooltip.Hide();
            }
        }

        public override Vector2? HoldoutOffset() => new Vector2(2f, -2f);
        public override void HoldItem(Player player) => player.GetModPlayer<ShimmerGunPlayer>().canHitNPC = true;
    }

    public class ShimmerGunProjectile : ModProjectile
    {
        // evil ass private access hack
        readonly Type npcType = typeof(NPC);
        MethodInfo GetShimmeredMethod;

        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SlimeGun}";

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.SlimeGun);
            Projectile.aiStyle = ProjAIStyleID.WaterJet;
            AIType = ProjectileID.SlimeGun;

            // :smiling_imp:
            GetShimmeredMethod = npcType.GetMethod("GetShimmered", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hit.Damage = 0;
            hit.SourceDamage = 0;
            hit.HideCombatText = true;

            if (target.life != target.lifeMax)
                target.life += 1;

            // checar que tenga variante de shimmer
            if (NPCID.Sets.ShimmerTownTransform[target.type])
            {
                // particulas
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(target.position, target.width, target.height, DustID.ShimmerSplash);
                    Dust.NewDust(target.position, target.width, target.height, DustID.ShimmerSpark);
                }

                // sonidos
                SoundEngine.PlaySound(SoundID.Item176, target.position);
                SoundEngine.PlaySound(SoundID.Item29, target.position);
                SoundEngine.PlaySound(SoundID.Splash, target.position);
            }

            // invoke the private method on the instance
            GetShimmeredMethod?.Invoke(target, null);
            Projectile.Kill();
            // escribo comentarios en ingles y en español dependiendo del dia o algo no se jaja
        }
    }

    public class ShimmerGunPlayer : ModPlayer
    {
        public bool canHitNPC;
        public override void ResetEffects() => canHitNPC = false;
        public override bool? CanHitNPCWithProj(Projectile proj, NPC target)
        {
            if (canHitNPC && proj.type == ModContent.ProjectileType<ShimmerGunProjectile>())
                return true;
            else
                return null;
        }
    }
}
