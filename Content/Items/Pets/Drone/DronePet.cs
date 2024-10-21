using eslamio.Content.Items.Pets.Tsuyar;
using System;
using Terraria.ID;

namespace eslamio.Content.Items.Pets.Drone;
public class DronePetItem : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.ZephyrFish);

        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.width = 22;
        Item.height = 36;

        Item.shoot = ModContent.ProjectileType<DronePetProjectile>();
        Item.buffType = ModContent.BuffType<DronePetBuff>();

        Item.value = Item.buyPrice(gold: 50);
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            player.AddBuff(Item.buffType, 3600);
        }
        return true;
    }
}

public class DronePetBuff : BasePetBuff
{
    protected override int PetProj => ModContent.ProjectileType<DronePetProjectile>();
    protected override bool LightPet => true;
}

public class DronePetProjectile : ModProjectile
{
    public override void SetStaticDefaults()
    {
        Main.projFrames[Projectile.type] = 1;
        Main.projPet[Projectile.type] = true;
        ProjectileID.Sets.TrailingMode[Projectile.type] = 3;

        ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
            .WithOffset(-15, -20f)
            .WithSpriteDirection(1)
            .WithCode(DelegateMethods.CharacterPreview.Float);
    }

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.GlommerPet);
        AIType = ProjectileID.GlommerPet;

        Projectile.width = 50;
        Projectile.height = 18;
    }

    public override bool PreAI()
    {
        Main.player[Projectile.owner].petFlagGlommerPet = false;
        return true;
    }

    public override void AI()
    {
        // keep projectile from despawning
        Player player = Main.player[Projectile.owner];
        if (!player.dead && player.HasBuff(ModContent.BuffType<DronePetBuff>()))
            Projectile.timeLeft = 2;

        // drone "self" light
        Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);

        // flashlight
        const float spread = (float)Math.PI / 10;
        const int numOfBeams = 5;
        const int numOfPoints = 10;
        const float brightness = 0.75f;
        for (int a = 0; a < numOfBeams; a++)
        {
            Vector2 dir = new Vector2(20 * Projectile.direction, 0).RotatedBy(spread * (-0.5 + a / (numOfBeams - 1f)));
            for (int i = 0; i < numOfPoints; i++)
            {
                Vector2 point = Projectile.Center + dir * i;
                if (Collision.IsWorldPointSolid(point, true))
                    break;
                Lighting.AddLight(point, brightness, brightness, brightness);
            }
        }
    }
}
