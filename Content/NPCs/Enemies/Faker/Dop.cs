using eslamio.Common.ModSystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace eslamio.Content.NPCs.Enemies.Faker
{
	public class Dop : ModNPC
	{
		public Player victim;

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
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 360f;
			NPC.knockBackResist = 0.16f;
			NPC.aiStyle = 3;

			AIType = NPCID.BloodMummy;
			AnimationType = NPCID.BloodMummy;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (victim is not null)
			{
				victim.position.X = NPC.position.X;
				victim.position.Y = NPC.position.Y + 6;

				victim.direction = NPC.direction;
				victim.headFrame.Y = NPC.frame.Y;
				victim.bodyFrame.Y = NPC.frame.Y;
				victim.legFrame.Y = NPC.frame.Y;
			}
			Main.PlayerRenderer.DrawPlayer(Main.Camera, victim, victim.position, victim.fullRotation, victim.fullRotationOrigin, 0f);

			return false;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Cursed, 180);
		}

		const float speedX = 2.5f;
        public override bool PreAI()
        {
			NPC.velocity.X /= speedX;
            return base.PreAI();
        }
        public override void PostAI()
        {
			NPC.velocity.X *= speedX;
            base.PostAI();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			// spawns in the underground and cavern layers, and only if no other doppelgangers exist
			if (!NPC.AnyNPCs(Type) && (spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight))
			{
				return SpawnCondition.Cavern.Chance * 0.075f;
			}

			return 0f;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Player player = FindClosestPlayer(1600);
			if (player is not null)
			{
				victim = (Player)player.Clone();
				victim.CurrentLoadoutIndex = player.CurrentLoadoutIndex;

				NPC.GivenName = victim.name;
				NPC.damage *= (int)(victim.statLifeMax * 0.005);
				NPC.defense = victim.statDefense * 2;
				NPC.lifeMax = victim.statLifeMax * 2;
				NPC.life = NPC.lifeMax;

				//ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral("You can hear someone mining in the distance."), Color.MediumPurple, victim.whoAmI);
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

		/*public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && NPC.life <= 0)
			{
				NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);

				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.NextBool(2))
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}*/

		/*public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<Items.Sets.RunicSet.Rune>();
			npcLoot.AddCommon<SoulDagger>(25);
		}*/
	}
}