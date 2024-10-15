using Terraria.ID;

namespace eslamio.Content.Items.Weapons;

public class CoolSwagSword : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Flymeal);
        Item.DamageType = DamageClass.Default;
        Item.damage = 9999999;
        Item.crit = 999;
        Item.knockBack = 99;
        Item.rare = ItemRarityID.Red;
    }

    public override void HoldItem(Player player)
    {
        player.GetModPlayer<CoolSwagPlayer>().canHitNPC = true;
    }
}

class CoolSwagPlayer : ModPlayer
{
    public bool canHitNPC;

    public override void ResetEffects()
    {
        canHitNPC = false;
    }

    public override bool? CanHitNPCWithItem(Item item, NPC target)
    {
        if (canHitNPC) return true;
        else return null;
    }
}