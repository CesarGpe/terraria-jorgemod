using Terraria;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;

namespace eslamio.Content.Buffs
{
	public class GravBuff : ModBuff
	{
		const int worldUpLimit = 457;

		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.gravDir = -1f;
			player.gravControl = true;
			player.controlUp = false;
			player.releaseUp = false;
			
			if (player.position.Y <= Main.maxTilesY - worldUpLimit)
				player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " esta fuera de este mundo!!"), 69420420f, 1);
		}
	}
}
