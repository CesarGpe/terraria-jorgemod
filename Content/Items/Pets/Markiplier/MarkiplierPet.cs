using System.Collections.Generic;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace eslamio.Content.Items.Pets.Markiplier;
public class MarkiplierPetItem : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.ZephyrFish);

        Item.shoot = ProjectileID.None;
        Item.buffType = ModContent.BuffType<MarkiplierPetBuff>();
        Item.scale = 0.25f;
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            player.AddBuff(Item.buffType, 3600);
        }
        return true;
    }

    /*public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.IronBar, 5)
            .AddIngredient(ItemID.Marble, 2)
            .AddIngredient(ItemID.Glass, 2)
            .AddTile(TileID.Anvils)
            .Register();

        CreateRecipe()
            .AddIngredient(ItemID.LeadBar, 5)
            .AddIngredient(ItemID.Marble, 2)
            .AddIngredient(ItemID.Glass, 2)
            .AddTile(TileID.Anvils)
            .Register();
    }*/
}

public class MarkiplierPetBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = true;
        Main.vanityPet[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    { // This method gets called every frame your buff is active on your player.
        bool unused = false;
        player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ProjectileID.None);
    }
}

internal class MarkiplierFacecam : UIState
{
    private UIElement area;
    private UIImage cam;
    private Asset<Texture2D> Face => ModContent.Request<Texture2D>("eslamio/Content/Items/Pets/Markiplier/MarkiplierPetItem");

    public override void OnInitialize()
    {
        area = new UIElement();
        area.Width.Set(Main.screenWidth, 0f);
        area.Height.Set(Main.screenHeight, 0f);
        area.Left.Set(0, 0);
        area.Top.Set(0, 0);

        cam = new UIImage(Face);
        cam.Width.Set(271, 0f);
        cam.Height.Set(207, 0f);
        cam.Left.Set(0, 0f);
        cam.Top.Set(150, 0f);

        area.Append(cam);
        Append(area);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!Main.LocalPlayer.HasBuff(ModContent.BuffType<MarkiplierPetBuff>()))
            return;

        base.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        if (!Main.LocalPlayer.HasBuff(ModContent.BuffType<MarkiplierPetBuff>()))
            return;

        base.Update(gameTime);
    }
}

// This class will only be autoloaded/registered if we're not loading on a server
[Autoload(Side = ModSide.Client)]
internal class MarkUISystem : ModSystem
{
    private UserInterface MarkUI;
    internal MarkiplierFacecam MarkFacecam;
    public override void Load()
    {
        MarkFacecam = new();
        MarkUI = new();
        MarkUI.SetState(MarkFacecam);
    }

    public override void UpdateUI(GameTime gameTime)
    {
        MarkUI?.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int MarksIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
        if (MarksIndex != -1)
        {
            layers.Insert(MarksIndex, new LegacyGameInterfaceLayer(
                "eslamio: Markiplier Facecam Pet",
                delegate
                {
                    MarkUI.Draw(Main.spriteBatch, new GameTime());
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}
