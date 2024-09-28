using eslamio.Core;
using Terraria.ID;

namespace eslamio.Content.NPCs.Enemies.Faker
{
	public class DopSpawner : ModPlayer
	{
		int noiseTimer = 0;
		public int moodPhase = 1;

		private void PlaySound()
		{
			int sound = Main.rand.Next(4);
			if (moodPhase < 3)
				JiskUtils.PlaySoundOverBGM(new($"eslamio/Assets/Sounds/Dop/CaveNoise{sound}"));
			else if (moodPhase < 5)
				JiskUtils.PlaySoundOverBGM(new($"eslamio/Assets/Sounds/Dop/Stalk{sound}"));
			else
				moodPhase = -1;
		}

		public override void PreUpdate()
		{
			//28800
			if (Player.ZoneDirtLayerHeight || Player.ZoneRockLayerHeight)
				noiseTimer += Main.rand.Next(2);
			else
				noiseTimer -= Main.rand.Next(2);

			if (noiseTimer >= 240)
			{
				if (Main.netMode != NetmodeID.Server) PlaySound();
				moodPhase++;
				noiseTimer = 0;
			}

			base.PreUpdate();
		}
	}
}