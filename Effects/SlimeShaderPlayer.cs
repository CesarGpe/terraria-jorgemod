namespace eslamio.Effects
{
    [Autoload(Side = ModSide.Client)]
    class SlimeShaderPlayer : ModPlayer
	{
		private bool WasActiveLastTick;
		public bool IsActive;
		private Vector2 TargetPosition;

		public override void ResetEffects()
		{
			WasActiveLastTick = IsActive;
			IsActive = false;
		}

		public override void PostUpdateMiscEffects()
		{
			Player.ManageSpecialBiomeVisuals("eslamio:SlimeShader", IsActive || WasActiveLastTick, TargetPosition);
		}
	}
}
