using eslamio.Content.Config;
using eslamio.Effects;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader.Utilities;

namespace eslamio.Content.NPCs.Enemies.Faker
{
	public class Dop : ModNPC
	{
		int despawnTimer = 0;
		bool angry = false;
		Player skin;
		SoundStyle disappearSound = new("eslamio/Assets/Sounds/Dop/Disappear") { PitchVariance = 0.5f };
		SoundStyle deathSound = new("eslamio/Assets/Sounds/Dop/Death") { PitchVariance = 0.5f };
		SoundStyle spottedSound = new("eslamio/Assets/Sounds/Dop/Spotted") { PitchVariance = 0.5f };
		SoundStyle hitSound = new(SoundID.NPCHit37.SoundPath) { PitchVariance = 0.5f };


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
			NPC.rarity = 10;
			//NPC.HitSound = SoundID.NPCHit37;
			NPC.HitSound = hitSound;
			NPC.DeathSound = deathSound;
			NPC.value = Main.rand.Next(25000, 50000);
			NPC.knockBackResist = 0f;
			NPC.aiStyle = NPCAIStyleID.Fighter;

			AIType = NPCID.BloodMummy;
			AnimationType = NPCID.BloodMummy;
		}

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				new FlavorTextBestiaryInfoElement("...")
            ]);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            if (NPC.IsABestiaryIconDummy)
				return true;

			if (skin is not null)
			{
				// drawing stuff
				skin.position.X = NPC.position.X;
				skin.position.Y = NPC.position.Y + 6;

				skin.direction = NPC.direction;
				skin.headFrame.Y = NPC.frame.Y;
				skin.bodyFrame.Y = NPC.frame.Y;
				skin.legFrame.Y = NPC.frame.Y;
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
			if (!IsNpcOnscreen(NPC.Center) && angry)
				despawnTimer++;
			else
				despawnTimer = 0;

			if (despawnTimer == 350)
				SoundEngine.PlaySound(disappearSound, null);
			else if (despawnTimer > 360)
			{
				NPC.EncourageDespawn(10);
				NPC.active = false;
				NPC.netSkip = -1;
				NPC.life = 0;

				return false;
			}

			NPC.velocity.X /= speedX;
			return base.PreAI();
		}
		public override void AI()
        {
            NPC.TargetClosest();
			Lighting.AddLight(NPC.position, 0.5f, 0.2f, 0);

			var players = FindPlayersInRadius(1200);
			if (!angry)
			{
				NPC.velocity.X = 0;
				for (int i = 0; i < players.Count; i++)
				{
					VignettePlayer vignettePlayer = players[i].GetModPlayer<VignettePlayer>();
					vignettePlayer.SetVignette(0f, 900f, 0.85f, Color.Black, players[i].Center, 0.35f);
					vignettePlayer.shakeIntensity = 0.6f;
				}
			}
			else
			{
				for (int i = 0; i < players.Count; i++)
				{
					VignettePlayer vignettePlayer = players[i].GetModPlayer<VignettePlayer>();
					vignettePlayer.SetVignette(0f, 500f, 0.9f, Color.Black, players[i].Center, 0.6f);
					vignettePlayer.shakeIntensity = 1.2f;
					vignettePlayer.sceneActive = true;
				}
			}
			
			if (!angry && (CheckForPlayer(200) || NPC.life != NPC.lifeMax))
			{
				angry = true;
				SoundEngine.PlaySound(spottedSound, NPC.position);
			}
        }
        public override void PostAI()
		{
			NPC.velocity.X *= speedX;
			base.PostAI();
		}

		private static bool IsNpcOnscreen(Vector2 center)
		{
			int w = NPC.sWidth + NPC.safeRangeX * 2;
			int h = NPC.sHeight + NPC.safeRangeY * 2;
			Rectangle npcScreenRect = new((int)center.X - w / 2, (int)center.Y - h / 2, w, h);
			foreach (Player player in Main.player)
			{
				if (player.active && player.getRect().Intersects(npcScreenRect))
					return true;
			}
			return false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			// spawns in the underground and cavern layers, and only if no other doppelgangers exist
			if (!NPC.AnyNPCs(Type) && (spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight))
				return SpawnCondition.Cavern.Chance * 0.01f;
			//return SpawnCondition.Cavern.Chance * 0.01f;

			return 0f;
		}

		public override void OnSpawn(IEntitySource source)
		{
			despawnTimer = 0;
			Player victim = FindClosestPlayer(1600);
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

		private bool CheckForPlayer(float maxDetectDistance)
		{
			// Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			// Loop through all Players
			foreach (Player target in Main.ActivePlayers)
			{
				// The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
				float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, NPC.Center);

				// Check if it is within the radius and not through blocks
				if (sqrDistanceToTarget < sqrMaxDetectDistance && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
					return true;
			}

			return false;
		}

		private List<Player> FindPlayersInRadius(float maxDetectDistance)
		{
			List<Player> foundPlayers = [];

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
					foundPlayers.Add(target);
				}
			}

			return foundPlayers;
		}

	
		public override void HitEffect(NPC.HitInfo hit)
		{
			// Create gore when the NPC is killed.
			if (Main.netMode != NetmodeID.Server && !Main.dedServ && NPC.life <= 0)
			{
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