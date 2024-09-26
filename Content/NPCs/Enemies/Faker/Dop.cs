using eslamio.Common.ModSystems;
using eslamio.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace eslamio.Content.NPCs.Enemies.Faker
{
	public class Dop : ModNPC
	{
		int despawnTimer = 0;
		Player victim;
		Player skin;
		SoundStyle disappearSound = new("eslamio/Assets/Sounds/Dop/Disappear") {
			PitchVariance = 0.5f
		};
		SoundStyle hitSound = new(SoundID.NPCHit37.SoundPath) {
			PitchVariance = 0.5f
		};
		SoundStyle deathSound = new("eslamio/Assets/Sounds/Dop/Death") {
			PitchVariance = 0.5f
		};


		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BloodMummy];

			// este enemigo no tiene bestiario
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() { Hide = true };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 48;
			NPC.damage = 60;
			NPC.defense = 40;
			NPC.lifeMax = 400;
			//NPC.HitSound = SoundID.NPCHit37;
			NPC.HitSound = hitSound;
			NPC.DeathSound = deathSound;
			NPC.value = 360f;
			NPC.knockBackResist = 0.16f;
			NPC.aiStyle = 3;

			AIType = NPCID.BloodMummy;
			AnimationType = NPCID.BloodMummy;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (skin is not null)
			{
				// drawing stuff
				skin.position.X = NPC.position.X;
				skin.position.Y = NPC.position.Y + 6;

				skin.direction = NPC.direction;
				skin.headFrame.Y = NPC.frame.Y;
				skin.bodyFrame.Y = NPC.frame.Y;
				skin.legFrame.Y = NPC.frame.Y;

				// player camera stuff
				VignettePlayer vignettePlayer = victim.GetModPlayer<VignettePlayer>();
                vignettePlayer.SetVignette(0f, 500f, 0.95f, Color.Black, victim.Center);

				PunchCameraModifier modifier = new(victim.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 1f, 6f, 10, 1f, FullName);
				Main.instance.CameraModifiers.Add(modifier);
			}
			Main.PlayerRenderer.DrawPlayer(Main.Camera, skin, skin.position, skin.fullRotation, skin.fullRotationOrigin, 0f);

			return false;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Cursed, 180);
		}

		// will not despawn naturally, we handle that in PreAI
		public override bool CheckActive() => false;

		const float speedX = 2.5f;
        public override bool PreAI()
        {
			if (!IsNpcOnscreen(NPC.Center))
				despawnTimer++;
			else
				despawnTimer = 0;

			if (despawnTimer == 200 && Main.netMode != NetmodeID.Server)
					SoundEngine.PlaySound(disappearSound, null);
			else if (despawnTimer > 240)
			{
				//ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral("It's no longer after you."), Color.MediumPurple, skin.whoAmI);
				NPC.EncourageDespawn(10);
				NPC.active = false;
				NPC.netSkip = -1;
				NPC.life = 0;

				return false;
			}

			NPC.velocity.X /= speedX;
            return base.PreAI();
        }
        public override void PostAI()
        {
			NPC.velocity.X *= speedX;
            base.PostAI();
        }

		private static bool IsNpcOnscreen(Vector2 center) {
			int w = NPC.sWidth + NPC.safeRangeX * 2;
			int h = NPC.sHeight + NPC.safeRangeY * 2;
			Rectangle npcScreenRect = new Rectangle((int)center.X - w / 2, (int)center.Y - h / 2, w, h);
			foreach (Player player in Main.player) {
				if (player.active && player.getRect().Intersects(npcScreenRect))
					return true;
			}
			return false;
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			// spawns in the underground and cavern layers, and only if no other doppelgangers exist
			if (!NPC.AnyNPCs(Type) && (DopSpawner.moodPhase == -1) && (spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight))
				return SpawnCondition.Cavern.Chance;
				//return SpawnCondition.Cavern.Chance * 0.008f;
			
			return 0f;
		}

        public override void OnSpawn(IEntitySource source)
		{
			despawnTimer = 0;
			victim = FindClosestPlayer(1600);
			if (victim is not null)
			{
				skin = (Player)victim.Clone();
				skin.CurrentLoadoutIndex = victim.CurrentLoadoutIndex;

				NPC.GivenName = skin.name;
				NPC.damage *= (int)(skin.statLifeMax * 0.005);
				NPC.defense = skin.statDefense * 2;
				NPC.lifeMax = skin.statLifeMax * 2;
				NPC.life = NPC.lifeMax;

				//ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral("You can hear someone mining in the distance."), Color.MediumPurple, skin.whoAmI);
			}

			base.OnSpawn(source);
		}

		private Player FindClosestPlayer(float maxDetectDistance)
		{
			Player closestPlayer = null;

			// Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			// Loop through all Players
			foreach (Player target in Main.ActivePlayers)
			{
				// The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
				float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, NPC.Center);

				// Check if it is within the radius
				if (sqrDistanceToTarget < sqrMaxDetectDistance)
				{
					sqrMaxDetectDistance = sqrDistanceToTarget;
					closestPlayer = target;
				}
			}

			return closestPlayer;
		}

		public override void HitEffect(NPC.HitInfo hit) {
			// Create gore when the NPC is killed.
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 2), NPC.velocity, 11);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 2), NPC.velocity, 12);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 3), NPC.velocity, 13);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 3), NPC.velocity, 11);
			}
		}

		/*public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<Items.Sets.RunicSet.Rune>();
			npcLoot.AddCommon<SoulDagger>(25);
		}*/
	}
}