using Terraria.DataStructures;
using Terraria.ID;

namespace eslamio.Content.Items.Pets.Familiar;
public class FamiliarPetProjectile : ModProjectile
{
    public override string Texture => eslamio.BlankTexture;

    private FamiliarPetItem originItem;
    private Player skin;

    public Color[] colors = null;
    public bool Male;
    public int skinVariant;
    public int hairVariant;

    public override void SetStaticDefaults()
    {
        Main.projFrames[Projectile.type] = 20;
        Main.projPet[Projectile.type] = true;
        ProjectileID.Sets.CharacterPreviewAnimations[Type] = new() { Offset = new(8f, 1f) };
    }

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.BlackCat);

        Projectile.width = 20;
        Projectile.height = 42;
        Projectile.friendly = true;
        Projectile.aiStyle = ProjAIStyleID.Pet;
        AIType = ProjectileID.BlackCat;
        Projectile.scale = 0.75f;
    }

    public override void OnSpawn(IEntitySource source)
    {
        if (source is EntitySource_ItemUse parent && parent.Item is Item something && something.ModItem is FamiliarPetItem item)
        {
            originItem = item;
            SetSkin();
        }
        base.OnSpawn(source);
    }

    private void SetSkin()
    {
        var owner = Main.player[Projectile.owner];
        if (originItem is null)
        {
            if (owner.HasBuff<FamiliarPetBuff>() && owner.miscEquips[0].ModItem is FamiliarPetItem item)
                originItem = item;
            else
            {
                Projectile.Kill();
                owner.ClearBuff(ModContent.BuffType<FamiliarPetBuff>());
            }
        }

        if (originItem.colors is null)
            originItem.SetColors();

        colors = originItem.colors;
        Male = originItem.Male;
        skinVariant = originItem.skinVariant;
        hairVariant = originItem.hairVariant;
    }

    private void UpdateTick()
    {
        skin ??= new Player();
        SetSkin();

        skin.hairColor = colors[0];
        skin.eyeColor = colors[1];
        skin.skinColor = colors[2];
        skin.shirtColor = colors[3];
        skin.underShirtColor = colors[4];
        skin.pantsColor = colors[5];
        skin.shoeColor = colors[6];

        skin.Male = Male;
        skin.skinVariant = skinVariant;
        skin.hair = hairVariant;

        skin.width = Projectile.width;
        skin.height = Projectile.height;
        skin.oldVelocity = Projectile.oldVelocity;
        skin.velocity = Projectile.velocity;
        skin.oldDirection = Projectile.oldDirection;
        skin.wet = Projectile.wet;
        skin.lavaWet = Projectile.lavaWet;
        skin.honeyWet = Projectile.honeyWet;
        skin.wetCount = Projectile.wetCount;
        if (Projectile.velocity != Vector2.Zero || Projectile.direction == 0)
        {
            skin.direction = Projectile.velocity.X < 0f ? -1 : 1;
        }
        skin.oldPosition = Projectile.oldPosition;
        skin.position = Projectile.position;
        skin.position.Y -= 42 * (1f - Projectile.scale);
        skin.whoAmI = Projectile.owner;

        skin.PlayerFrame();
    }

    public override bool PreAI()
    {
        UpdateTick();

        Player player = Main.player[Projectile.owner];
        player.blackCat = false;

        return true;
    }

    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        if (!player.dead && player.HasBuff(ModContent.BuffType<FamiliarPetBuff>()))
        {
            Projectile.timeLeft = 2;
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        if (Projectile.isAPreviewDummy)
            return false;

        if (skin is null)
            UpdateTick();

        skin.Bottom = Projectile.Bottom;
        Main.PlayerRenderer.DrawPlayer(Main.Camera, skin, Projectile.position, 0f, Vector2.Zero, 0f, Projectile.scale);
        return false;
    }
}