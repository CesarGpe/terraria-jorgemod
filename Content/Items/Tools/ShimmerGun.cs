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
            Item.CloneDefaults(ItemID.WaterGun);

            Item.width = 40;
            Item.height = 30;
            //Item.expert = true;
            Item.rare = ItemRarityID.Expert;
            Item.shoot = ModContent.ProjectileType<ShimmerGunProjectile>();
            Item.shootSpeed = 10f;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.scale = 1f;

            // se consigue fulgoreando la pistola de slime
            ItemID.Sets.ShimmerTransformToItem[ItemID.SlimeGun] = Type;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.SlimeGun;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (var tooltip in tooltips)
            {
                if (tooltip.Name == "Placeable" || tooltip.Name == "Damage" || tooltip.Name == "Speed" || tooltip.Name == "Knockback")
                    tooltip.Hide();
            }
        }

        public override Vector2? HoldoutOffset() => new Vector2(2f, -2f);
        public override void HoldItem(Player player)
        {
            player.GetModPlayer<ShimmerGunPlayer>().canHitNPC = true;
        }
    }

    public class ShimmerGunProjectile : ModProjectile
    {
        // evil ass private access hack
        readonly Type npcType = typeof(NPC);
        MethodInfo NPCShimmeredMethod;
        readonly Type itemType = typeof(Item);
        MethodInfo ItemShimmeredMethod;

        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SlimeGun}";

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.SlimeGun);

            // :smiling_imp:
            NPCShimmeredMethod = npcType.GetMethod("GetShimmered", BindingFlags.NonPublic | BindingFlags.Instance);
            ItemShimmeredMethod = itemType.GetMethod("GetShimmered", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public override void AI()
        {
            Projectile.scale -= 0.015f;
            if (Projectile.scale <= 0f)
            {
                Projectile.velocity *= 5f;
                Projectile.oldVelocity = Projectile.velocity;
                Projectile.Kill();
            }

            if (Projectile.ai[0] > 3f)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    Rectangle ProjectileHitbox = new((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        if (Main.npc[i].active && !Main.npc[i].dontTakeDamage && Main.npc[i].lifeMax > 1)
                        {
                            Rectangle NPCHitbox = new((int)Main.npc[i].position.X, (int)Main.npc[i].position.Y, Main.npc[i].width, Main.npc[i].height);
                            if (ProjectileHitbox.Intersects(NPCHitbox))
                            {
                                Projectile.Kill();
                                NPC target = Main.npc[i];
                                ShimmerThisNPC(target);
                            }
                        }
                    }
                    for (int i = 0; i < Main.maxItems; i++)
                    {
                        if (Main.item[i].active)
                        {
                            Rectangle ItemHitbox = new((int)Main.item[i].position.X, (int)Main.item[i].position.Y, Main.item[i].width, Main.item[i].height);
                            if (ProjectileHitbox.Intersects(ItemHitbox))
                            {
                                Projectile.Kill();
                                Item target = Main.item[i];
                                target.position.Y -= target.height;
                                ShimmerThisItem(target);
                            }
                        }
                    }
                }

                Projectile.ai[0] += Projectile.ai[1];
                if (Projectile.ai[0] > 30f)
                {
                    Projectile.velocity.Y += 0.1f;
                }

                int dustAlpha = 175;
                int dustToShoot = DustID.ShimmerSpark;
                for (int num549 = 0; num549 < 6; num549++)
                {
                    Vector2 dustVector = Projectile.velocity * num549 / 6f;
                    int dustOffset = 6;
                    int swagDust = Dust.NewDust(Projectile.position + Vector2.One * 6f, Projectile.width - dustOffset * 2, Projectile.height - dustOffset * 2, dustToShoot, 0f, 0f, dustAlpha, default, 1.2f);
                    Dust dust1;
                    Dust dust2;
                    if (Main.rand.NextBool(2))
                    {
                        dust1 = Main.dust[swagDust];
                        dust2 = dust1;
                        dust2.alpha += 25;
                    }
                    if (Main.rand.NextBool(2))
                    {
                        dust1 = Main.dust[swagDust];
                        dust2 = dust1;
                        dust2.alpha += 25;
                    }
                    if (Main.rand.NextBool(2))
                    {
                        dust1 = Main.dust[swagDust];
                        dust2 = dust1;
                        dust2.alpha += 25;
                    }
                    Main.dust[swagDust].noGravity = true;
                    dust1 = Main.dust[swagDust];
                    dust2 = dust1;
                    dust2.velocity *= 0.3f;
                    dust1 = Main.dust[swagDust];
                    dust2 = dust1;
                    //dust2.velocity += Projectile.velocity * 0.5f;
                    Main.dust[swagDust].position = Projectile.Center;
                    Main.dust[swagDust].position.X -= dustVector.X;
                    Main.dust[swagDust].position.Y -= dustVector.Y;
                    dust1 = Main.dust[swagDust];
                    dust2 = dust1;
                    dust2.velocity *= 0.2f;
                }
                if (Main.rand.NextBool(4))
                {
                    int dustOffset = 6;
                    Color swagColor = Main.rand.NextBool() ? Color.Pink : new(156, 230, 255, 255);
                    int swagDust = Dust.NewDust(Projectile.position + Vector2.One * 6f, Projectile.width - dustOffset * 2, Projectile.height - dustOffset * 2, dustToShoot, 0f, 0f, dustAlpha, swagColor, 1.2f);
                    Dust dust1 = Main.dust[swagDust];
                    Dust dust2 = dust1;
                    dust2.velocity *= 0.5f;
                    dust1 = Main.dust[swagDust];
                    dust2 = dust1;
                    //dust2.velocity += Projectile.velocity * 0.5f;
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
        }

        void ShimmerThisNPC(NPC target)
        {
            if (NPCID.Sets.ShimmerTownTransform[target.type])
                Effects(target);

            NPCShimmeredMethod?.Invoke(target, null);
        }

        void ShimmerThisItem(Item target)
        {
            //if (ItemID.Sets.ShimmerTransformToItem[target.type] > 0)
            Effects(target);

            ItemShimmeredMethod?.Invoke(target, null);
        }

        static void Effects(Entity target)
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
