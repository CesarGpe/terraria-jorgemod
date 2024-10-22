using eslamio.Content.NPCs.Enemies;
using eslamio.Core;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace eslamio.Content.Items.Pets.Familiar;
public class FamiliarPetItem : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.ZephyrFish);

        Item.shoot = ModContent.ProjectileType<FamiliarPetProjectile>();
        Item.buffType = ModContent.BuffType<FamiliarPetBuff>();

        Item.value = Item.buyPrice(gold: 20);
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            player.AddBuff(Item.buffType, 3600);
        }
        return true;
    }

    readonly Asset<Texture2D> hair = ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Familiar/Hair");
    readonly Asset<Texture2D> eyes = ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Familiar/Eyes");
    readonly Asset<Texture2D> pupil = ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Familiar/Pupil");
    readonly Asset<Texture2D> skin = ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Familiar/Skin");
    readonly Asset<Texture2D> shirt = ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Familiar/Shirt");
    readonly Asset<Texture2D> pants = ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Familiar/Pants");
    readonly Asset<Texture2D> shoes = ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Familiar/Shoes");
    public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        spriteBatch.Draw(hair.Value, position, frame, Main.LocalPlayer.hairColor, 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(pupil.Value, position, frame, Main.LocalPlayer.eyeColor, 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(skin.Value, position, frame, Main.LocalPlayer.skinColor, 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(shirt.Value, position, frame, Main.LocalPlayer.shirtColor, 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(pants.Value, position, frame, Main.LocalPlayer.pantsColor, 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(shoes.Value, position, frame, Main.LocalPlayer.shoeColor, 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(eyes.Value, position, frame, Color.White, 0, origin, scale, SpriteEffects.None, 0);
        return false;
    }
    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        Main.GetItemDrawFrame(Item.type, out _, out var frame);
        Vector2 origin = frame.Size() / 2f;
        Vector2 position = Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y);

        //spriteBatch.Draw(itemTexture, position, frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(hair.Value, position, frame, Main.LocalPlayer.hairColor.MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(pupil.Value, position, frame, Main.LocalPlayer.eyeColor.MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(skin.Value, position, frame, Main.LocalPlayer.skinColor.MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(shirt.Value, position, frame, Main.LocalPlayer.shirtColor.MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(pants.Value, position, frame, Main.LocalPlayer.pantsColor.MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(shoes.Value, position, frame, Main.LocalPlayer.shoeColor.MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(eyes.Value, position, frame, Color.White.MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);

        return false;
    }
}
public class FamiliarPetBuff : BasePetBuff
{
    protected override int PetProj => ModContent.ProjectileType<FamiliarPetProjectile>();
}
public class FamiliarPetProjectile : ModProjectile
{
    public override string Texture => eslamio.BlankTexture;
    private Player copiedPlayer;

    public override void SetStaticDefaults()
    {
        Main.projFrames[Projectile.type] = 20;
        Main.projPet[Projectile.type] = true;
        ProjectileID.Sets.CharacterPreviewAnimations[Type] = new() { Offset = new(8f, 1f) };
    }

    public override void SetDefaults()
    {
        Projectile.width = 20;
        Projectile.height = 42;
        Projectile.friendly = true;
        Projectile.aiStyle = ProjAIStyleID.Pet;
        AIType = ProjectileID.BlackCat;
        Projectile.scale = 0.75f;
    }

    public void CopyPlayerAttributes(Player parent)
    {
        // copies player attributes
        copiedPlayer.eyeColor = parent.eyeColor;
        copiedPlayer.hairColor = parent.hairColor;
        copiedPlayer.hairDyeColor = parent.hairDyeColor;
        copiedPlayer.pantsColor = parent.pantsColor;
        copiedPlayer.shirtColor = parent.shirtColor;
        copiedPlayer.shoeColor = parent.shoeColor;
        copiedPlayer.skinColor = parent.skinColor;
        copiedPlayer.underShirtColor = parent.underShirtColor;
        copiedPlayer.Male = parent.Male;
        copiedPlayer.skinVariant = parent.skinVariant;
        copiedPlayer.hairDye = parent.hairDye;
        copiedPlayer.hairDyeVar = parent.hairDyeVar;
        copiedPlayer.hair = parent.hair;

        // copies proj attributes
        copiedPlayer.width = Projectile.width;
        copiedPlayer.height = Projectile.height;
        copiedPlayer.oldVelocity = Projectile.oldVelocity;
        copiedPlayer.velocity = Projectile.velocity;
        copiedPlayer.oldDirection = Projectile.oldDirection;
        copiedPlayer.wet = Projectile.wet;
        copiedPlayer.lavaWet = Projectile.lavaWet;
        copiedPlayer.honeyWet = Projectile.honeyWet;
        copiedPlayer.wetCount = Projectile.wetCount;
        if (Projectile.velocity != Vector2.Zero || Projectile.direction == 0)
        {
            copiedPlayer.direction = Projectile.velocity.X < 0f ? -1 : 1;
        }
        copiedPlayer.oldPosition = Projectile.oldPosition;
        copiedPlayer.position = Projectile.position;
        copiedPlayer.position.Y -= 42 * (1f - Projectile.scale);
        copiedPlayer.whoAmI = Projectile.owner;

        copiedPlayer.PlayerFrame();
    }
    private void UpdateTick()
    {
        var owner = Main.player[Projectile.owner];
        copiedPlayer ??= new Player();

        CopyPlayerAttributes(owner);
    }
    public override bool PreAI()
    {
        JiskUtils.UpdateProjActive<FamiliarPetBuff>(Projectile);
        UpdateTick();
        return true;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        if (Projectile.isAPreviewDummy)
        {
            UpdateTick();
            copiedPlayer.headFrame = Main.player[Projectile.owner].headFrame;
            copiedPlayer.bodyFrame = Main.player[Projectile.owner].bodyFrame;
            copiedPlayer.legFrame = Main.player[Projectile.owner].legFrame;
        }
        if (copiedPlayer == null)
        {
            return false;
        }

        if (!Projectile.isAPreviewDummy)
        {
            copiedPlayer.Bottom = Projectile.Bottom;
        }

        Main.PlayerRenderer.DrawPlayer(Main.Camera, copiedPlayer, Projectile.position, 0f, Vector2.Zero, 0f, Projectile.scale);
        return false;
    }
}