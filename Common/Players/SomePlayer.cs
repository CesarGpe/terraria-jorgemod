﻿namespace eslamio.Common.Players
{
	public class SomePlayer : ModPlayer
	{
		public bool active = false;

		public override void ResetEffects() {
			active = false;
		}
    }
}
