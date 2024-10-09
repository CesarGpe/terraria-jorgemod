using Terraria.ID;

namespace eslamio.Common;

// makes witch doctor big!!! :drooling_face: 
public class WitchDoctorGlobalNPC : GlobalNPC
{
    public override bool AppliesToEntity(NPC npc, bool lateInstantiation) => npc.type == NPCID.WitchDoctor;
    public override void AI(NPC npc) => npc.scale = 1.5f;
}
