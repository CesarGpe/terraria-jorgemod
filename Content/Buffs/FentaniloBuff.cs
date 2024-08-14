using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Buffs
{
	public class FentaniloBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			// buff de daño
			player.GetDamage(DamageClass.Generic) += 100;

			// regeneracion de vida negativa
			player.lifeRegen = -1200;
			player.lifeRegenTime = -1000;

			// mensaje de muerte
			if(player.statLife == 1)
				player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " fallecio por una sobredosis de fentanilo."), 69420420f, 1);
		}
	}
}
