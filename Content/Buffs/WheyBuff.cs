using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Buffs
{
	public class WheyBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			// buff de daño
			player.GetDamage(DamageClass.Generic) += 50;

			// regeneracion de vida negativa
			player.lifeRegen = -600;
			player.lifeRegenTime = -1000;

			// mensaje de muerte
			if(player.statLife == 1)
				player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason("Los musculos de " + player.name + " se licuaron a si mismos debido a los efectos de la cantidad exhorbitantemente grande que consumio de la letal sustancia \"Whey\"."), 69420420f, 1);
		}
	}
}
