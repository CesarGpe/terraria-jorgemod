using eslamio.Core;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;

namespace eslamio.Content.Items.Pets.Familiar;
public class FamiliarPetItem : ModItem
{
    public override string Texture => eslamio.BlankTexture;

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

    Asset<Texture2D> hair = ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Familiar/Hair");
    Asset<Texture2D> eyes = ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Familiar/Eyes");
    Asset<Texture2D> skin = ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Familiar/Skin");
    Asset<Texture2D> shirt = ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Familiar/Shirt");
    Asset<Texture2D> pants = ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Familiar/Pants");
    Asset<Texture2D> shoes = ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Familiar/Shoes");
    public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        spriteBatch.Draw(hair.Value, position, frame, Main.LocalPlayer.hairColor, 0f, origin, scale, SpriteEffects.None, 0f);
        spriteBatch.Draw(eyes.Value, position, frame, Main.LocalPlayer.eyeColor, 0f, origin, scale, SpriteEffects.None, 0f);
        spriteBatch.Draw(skin.Value, position, frame, Main.LocalPlayer.skinColor, 0f, origin, scale, SpriteEffects.None, 0f);
        spriteBatch.Draw(shirt.Value, position, frame, Main.LocalPlayer.shirtColor, 0f, origin, scale, SpriteEffects.None, 0f);
        spriteBatch.Draw(pants.Value, position, frame, Main.LocalPlayer.pantsColor, 0f, origin, scale, SpriteEffects.None, 0f);
        spriteBatch.Draw(shoes.Value, position, frame, Main.LocalPlayer.shoeColor, 0f, origin, scale, SpriteEffects.None, 0f);
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

    private Player dummyPlayer;

    public override void SetStaticDefaults()
    {
        Main.projFrames[Projectile.type] = 20;
        Main.projPet[Projectile.type] = true;
        ProjectileID.Sets.CharacterPreviewAnimations[Type] = new() { Offset = new(10f, 10f), };
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
        dummyPlayer.eyeColor = parent.eyeColor;
        dummyPlayer.hairColor = parent.hairColor;
        dummyPlayer.hairDyeColor = parent.hairDyeColor;
        dummyPlayer.pantsColor = parent.pantsColor;
        dummyPlayer.shirtColor = parent.shirtColor;
        dummyPlayer.shoeColor = parent.shoeColor;
        dummyPlayer.skinColor = parent.skinColor;
        dummyPlayer.underShirtColor = parent.underShirtColor;
        dummyPlayer.Male = parent.Male;
        dummyPlayer.skinVariant = parent.skinVariant;
        dummyPlayer.hairDye = parent.hairDye;
        dummyPlayer.hairDyeVar = parent.hairDyeVar;
        dummyPlayer.hair = parent.hair;

        //dummyPlayer.selectedItem = 0;
        //dummyPlayer.inventory[dummyPlayer.selectedItem] = parent.HeldItem.Clone();

        // copies proj attributes
        dummyPlayer.width = Projectile.width;
        dummyPlayer.height = Projectile.height;
        dummyPlayer.oldVelocity = Projectile.oldVelocity;
        dummyPlayer.velocity = Projectile.velocity;
        dummyPlayer.oldDirection = Projectile.oldDirection;
        dummyPlayer.wet = Projectile.wet;
        dummyPlayer.lavaWet = Projectile.lavaWet;
        dummyPlayer.honeyWet = Projectile.honeyWet;
        dummyPlayer.wetCount = Projectile.wetCount;
        if (Projectile.velocity != Vector2.Zero || Projectile.direction == 0)
        {
            dummyPlayer.direction = Projectile.velocity.X < 0f ? -1 : 1;
        }
        dummyPlayer.oldPosition = Projectile.oldPosition;
        dummyPlayer.position = Projectile.position;
        dummyPlayer.position.Y -= 42 * (1f - Projectile.scale);
        dummyPlayer.whoAmI = Projectile.owner;

        dummyPlayer.PlayerFrame();
    }
    private void UpdateTick()
    {
        var parent = Main.player[Projectile.owner];
        dummyPlayer ??= new Player();

        CopyPlayerAttributes(parent);
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
            dummyPlayer.headFrame = Main.player[Projectile.owner].headFrame;
            dummyPlayer.bodyFrame = Main.player[Projectile.owner].bodyFrame;
            dummyPlayer.legFrame = Main.player[Projectile.owner].legFrame;
        }
        if (dummyPlayer == null)
        {
            return false;
        }

        dummyPlayer.Jisk().DrawScale = Projectile.scale;
        dummyPlayer.Jisk().DrawForceDye = Main.CurrentDrawnEntityShader;

        if (!Projectile.isAPreviewDummy)
        {
            dummyPlayer.Bottom = Projectile.Bottom;
        }

        Main.PlayerRenderer.DrawPlayer(Main.Camera, dummyPlayer, Projectile.position, 0f, Vector2.Zero, 0f, Projectile.scale);
        return false;
    }
}