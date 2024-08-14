using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace eslamio.Content.Buffs
{
	public class FlightMobilityBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.jumpSpeedBoost += 1.8f;
			player.wingAccRunSpeed *= 10;
			player.wingRunAccelerationMult *= 10;

			//player.moveSpeed += 0.075f;
			//player.moveSpeed += 0.5f;
			player.runAcceleration *= 2f;
			player.maxRunSpeed *= 2;
			player.accRunSpeed *= 2;
			player.runSlowdown *= 2;

			player.noFallDmg = true;
		}
	}
}
