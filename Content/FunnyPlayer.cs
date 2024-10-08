using eslamio.Content.Config;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;

namespace eslamio.Content
{
	[Autoload(Side = ModSide.Client)]
	public class FunnyPlayer : ModPlayer
	{
		bool playedSound = false;
		SoundStyle desastre = new("eslamio/Assets/Sounds/Knuckles/Desastre");
		SoundStyle tranquilidad = new("eslamio/Assets/Sounds/Knuckles/Tranquilidad");
		public override void PreUpdate()
		{
			if (Main.netMode != NetmodeID.Server || !Main.dedServ)
			{
				var config = ModContent.GetInstance<ClientConfig>();

				if (config.LlamasDelDesastre && Player.justJumped)
					SoundEngine.PlaySound(desastre, Player.position);

				if (config.AguasDeLaTranquilidad)
				{
					if (Player.controlDown && !playedSound)
					{
						SoundEngine.PlaySound(tranquilidad, Player.position);
						playedSound = true;
					}
					else if (!Player.controlDown && playedSound)
						playedSound = false;
				}
			}

		}
	}
}