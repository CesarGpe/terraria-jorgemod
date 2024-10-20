using eslamio.Content.Items.Consumables;
using eslamio.Content.NPCs.TownNPCs;
using eslamio.Effects;
using Terraria.ID;

namespace eslamio.Content.Items.Weapons;

public class Scalpel : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Flymeal);
        Item.DamageType = DamageClass.Melee;
        Item.rare = ItemRarityID.LightRed;

        Item.damage = 21;
    }

    public override void HoldItem(Player player)
    {
        player.GetModPlayer<ScalpelPlayer>().canHitNPC = true;
    }

    public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
    {
        modifiers.ScalingArmorPenetration += 0.5f;
        if (target.isLikeATownNPC)
            modifiers.ScalingArmorPenetration += 0.5f;
    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (target.isLikeATownNPC && target.life < 0)
        {
            if (target.type == NPCID.Truffle)
                player.QuickSpawnItem(target.GetSource_FromThis(), ModContent.ItemType<PancreasTruffle>());
            else
                player.QuickSpawnItem(target.GetSource_FromThis(), ModContent.ItemType<Pancreas>());
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Diamond, 2)
            .AddIngredient(ItemID.TungstenBar, 6)
            .AddTile(TileID.Anvils)
            .Register();

        CreateRecipe()
            .AddIngredient(ItemID.Diamond, 2)
            .AddIngredient(ItemID.SilverBar, 6)
            .AddTile(TileID.Anvils)
            .Register();
    }
}

class ScalpelPlayer : ModPlayer
{
    public bool canHitNPC;

    public override void ResetEffects()
    {
        canHitNPC = false;
    }

    public override bool? CanHitNPCWithItem(Item item, NPC target)
    {
        if (canHitNPC && target.type != ModContent.NPCType<Banban>()) return true;
        else return null;
    }
}