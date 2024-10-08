using eslamio.Core;

namespace eslamio.Content
{
	[Autoload(Side = ModSide.Client)]
	public class CaveSounds : ModPlayer
	{
		private int noiseTimer = 0;

		private void PlaySound()
		{
			bool choose = Main.rand.NextBool();
			int sound = Main.rand.Next(4);
			if (choose)
				JiskUtils.PlaySoundOverBGM(new($"eslamio/Assets/Sounds/Dop/CaveNoise{sound}"), 0.5f, Player.position);
			else
				JiskUtils.PlaySoundOverBGM(new($"eslamio/Assets/Sounds/Dop/Stalk{sound}"), 0.5f, Player.position);
		}

		public override void PreUpdate()
		{
			//ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral($"noiseTimer: {noiseTimer}"), Color.MediumPurple, Player.whoAmI);
			if (noiseTimer >= 28800)
			{
                PlaySound();
				noiseTimer = 0;
			}
			else if (Player.ZoneDirtLayerHeight || Player.ZoneRockLayerHeight)
				noiseTimer += Main.rand.Next(2);
			//else if (noiseTimer > 0) 
			//	noiseTimer--;
				
			base.PreUpdate();
		}
	}
}