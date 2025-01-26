using ReLogic.Content;
using System.IO;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace eslamio.Content.Items.Pets.Familiar;
public class FamiliarPetItem : ModItem
{
    public Color[] colors = null;
    public bool Male;
    public int skinVariant;
    public int hairVariant;
    private string customName = "Player Voodoo Doll";

    public byte dollOwner;

    public override void OnCreated(ItemCreationContext context)
    {
        SetColors();
    }

    public override ModItem NewInstance(Item entity)
    {
        SetColors();
        return base.NewInstance(entity);
    }

    public void SetColors()
    {
        Player copiedPlayer = Main.LocalPlayer;

        customName = $"{copiedPlayer.name}'s Voodoo Doll";
        Item.SetNameOverride(customName);

        colors = new Color[7];
        colors[0] = copiedPlayer.hairColor;
        colors[1] = copiedPlayer.eyeColor;
        colors[2] = copiedPlayer.skinColor;
        colors[3] = copiedPlayer.shirtColor;
        colors[4] = copiedPlayer.underShirtColor;
        colors[5] = copiedPlayer.pantsColor;
        colors[6] = copiedPlayer.shoeColor;

        Male = copiedPlayer.Male;
        skinVariant = copiedPlayer.skinVariant;
        hairVariant = copiedPlayer.hair;
    }

    public override void PostUpdate()
    {
        Item.SetNameOverride(customName);
    }
    public override void UpdateInventory(Player player)
    {
        Item.SetNameOverride(customName);
    }

    public override ModItem Clone(Item newEntity)
    {
        FamiliarPetItem clone = (FamiliarPetItem)base.Clone(newEntity);

        clone.colors = (Color[])colors?.Clone();
        clone.Male = Male;
        clone.skinVariant = skinVariant;
        clone.hairVariant = hairVariant;
        clone.customName = (string)customName?.Clone();

        return clone;
    }

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.ZephyrFish);

        Item.shoot = ModContent.ProjectileType<FamiliarPetProjectile>();
        Item.buffType = ModContent.BuffType<FamiliarPetBuff>();

        Item.noUseGraphic = true;
        Item.useStyle = ItemUseStyleID.Thrust;
        Item.value = Item.buyPrice(gold: 20);
    }

    public override void SaveData(TagCompound tag)
    {
        tag["Colors"] = colors;
        tag["Male"] = Male;
        tag["SkinVariant"] = skinVariant;
        tag["HairVariant"] = hairVariant;

        if (customName != "Player Voodoo Doll")
            tag["CustomName"] = customName;
    }

    public override void LoadData(TagCompound tag)
    {
        colors = tag.Get<Color[]>("Colors");
        Male = tag.GetBool("Male");
        skinVariant = tag.GetInt("SkinVariant");
        hairVariant = tag.GetInt("HairVariant");

        if (tag.TryGet("CustomName", out string foundName))
            customName = foundName;
    }

    public override void NetSend(BinaryWriter writer)
    {
        writer.WriteRGB(colors[0]);
        writer.WriteRGB(colors[1]);
        writer.WriteRGB(colors[2]);
        writer.WriteRGB(colors[3]);
        writer.WriteRGB(colors[4]);
        writer.WriteRGB(colors[5]);
        writer.WriteRGB(colors[6]);
        writer.Write(Male);
        writer.Write(skinVariant);
        writer.Write(hairVariant);
        writer.Write(customName);
    }

    public override void NetReceive(BinaryReader reader)
    {
        colors[0] = (Color)reader?.ReadRGB();
        colors[1] = (Color)reader?.ReadRGB();
        colors[2] = (Color)reader?.ReadRGB();
        colors[3] = (Color)reader?.ReadRGB();
        colors[4] = (Color)reader?.ReadRGB();
        colors[5] = (Color)reader?.ReadRGB();
        colors[6] = (Color)reader?.ReadRGB();
        Male = reader.ReadBoolean();
        skinVariant = reader.ReadInt32();
        hairVariant = reader.ReadInt32();
        customName = reader.ReadString();
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
        if (colors == null)
            SetColors();

        spriteBatch.Draw(hair.Value, position, frame, colors[0], 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(pupil.Value, position, frame, colors[1], 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(skin.Value, position, frame, colors[2], 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(shirt.Value, position, frame, colors[4], 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(pants.Value, position, frame, colors[5], 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(shoes.Value, position, frame, colors[6], 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(eyes.Value, position, frame, Color.White, 0, origin, scale, SpriteEffects.None, 0);

        return false;
    }
    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        if (colors == null)
            SetColors();

        Main.GetItemDrawFrame(Item.type, out _, out var frame);
        Vector2 origin = frame.Size() / 2f;
        Vector2 position = Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y);

        spriteBatch.Draw(hair.Value, position, frame, colors[0].MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(pupil.Value, position, frame, colors[1].MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(skin.Value, position, frame, colors[2].MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(shirt.Value, position, frame, colors[4].MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(pants.Value, position, frame, colors[5].MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(shoes.Value, position, frame, colors[6].MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(eyes.Value, position, frame, Color.White.MultiplyRGBA(lightColor), 0, origin, scale, SpriteEffects.None, 0);

        return false;
    }
}

public class FamiliarPetBuff : BasePetBuff
{
    protected override int PetProj => ModContent.ProjectileType<FamiliarPetProjectile>();
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        Main.buffNoSave[Type] = true;
    }
}