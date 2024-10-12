using eslamio.Content.NPCs.TownNPCs;
using Terraria.ID;

namespace eslamio.Content.Items;

public class ItemOverrides : GlobalItem
{

    public override bool? UseItem(Item item, Player player)
    {
        if (item.type == ItemID.BugNet || item.type == ItemID.FireproofBugNet || item.type == ItemID.GoldenBugNet)
        {
            for (int v = 0; v < 200; ++v)
            {
                NPC npc = Main.npc[v];
                if (npc.active && npc.townNPC)
                {
                    if (npc.type == ModContent.NPCType<Cesar>())
                    {
                        Main.npcCatchable[npc.type] = true;
                        npc.catchItem = ModContent.ItemType<Items.Consumables.CesarSpawner>();
                    }
                    if (npc.type == ModContent.NPCType<Isaac>())
                    {
                        Main.npcCatchable[npc.type] = true;
                        npc.catchItem = ModContent.ItemType<Items.Consumables.IsaacSpawner>();
                    }
                }
            }
        }
        return base.UseItem(item, player);
    }
}
