using Terraria.ID;

namespace eslamio.Common.GlobalNPCs
{
	public class WitchDoctorGlobalNPC : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) {
			return npc.type == NPCID.WitchDoctor;
		}

		public override void AI(NPC npc) {
			// makes witch doctor big !!! :drooling_face: 
			npc.scale = 1.5f;
			//npc.color = Color.ForestGreen;
		}
	}
}
