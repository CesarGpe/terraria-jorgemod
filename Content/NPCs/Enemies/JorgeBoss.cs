using eslamio.Effects;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;

namespace eslamio.Content.NPCs.Enemies
{
	[AutoloadBossHead]
	public class JorgeBoss : ModNPC
	{
        public override string Texture => $"Terraria/Images/NPC_{NPCID.KingSlime}";
        public override string BossHeadTexture => "eslamio/Content/NPCs/Enemies/JorgeBoss_Head_Boss";

		public override void Load() {
			// Register head icon manually
			Mod.AddBossHeadTexture(BossHeadTexture);
		}

        public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 6;

			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				PortraitScale = 2f,
				PortraitPositionYOverride = 32f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() {
			NPC.width = 162 * 10;
			NPC.height = 108 * 10;
			NPC.damage = 32;
			NPC.defense = 32;
			NPC.lifeMax = 32000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			NPC.value = Item.buyPrice(gold: 32, silver: 32, copper: 32);
			NPC.rarity = -32;
			NPC.boss = true;
			NPC.npcSlots = 32f; // Take up open spawn slots, preventing random NPCs from spawning during the fight
			NPC.netAlways = true;

			NPC.aiStyle = NPCAIStyleID.KingSlime;
			AIType = NPCID.KingSlime;
			AnimationType = NPCID.KingSlime;

			if (!Main.dedServ) {
				Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/jorgeboss");
			}
		}

        public override void AI()
        {
			float obesidad = 10f;
			float grasa = 1f;

			grasa -= 1f - NPC.life / NPC.lifeMax;
			obesidad *= grasa;

			float factor = NPC.life / NPC.lifeMax;
			factor = factor * 0.5f + 0.75f;
			factor *= obesidad;
			if (factor != NPC.scale)
			{
				NPC.position.X += NPC.width / 2;
				NPC.position.Y += NPC.height;
				NPC.scale = factor;
				NPC.width = (int)(98f * NPC.scale);
				NPC.height = (int)(92f * NPC.scale);
				NPC.position.X -= NPC.width / 2;
				NPC.position.Y -= NPC.height;
			}

			foreach (var player in Main.ActivePlayers)
				player.GetModPlayer<SlimeShaderPlayer>().IsActive = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// Sets the description of this NPC that is listed in the bestiary
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("es el..... oh no...................")
			});
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 1, 9999 * 2));
			npcLoot.Add(ItemDropRule.Common(ItemID.Toilet, 1, 1, 9999));
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
			cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
			return true;
		}

		public override void HitEffect(NPC.HitInfo hit) {

			// sound and camera modifier goes on client
			if (Main.netMode == NetmodeID.Server)
				return;

			if (NPC.life <= 0) {
				SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

				PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 6f, 20, 1000f, FullName);
				Main.instance.CameraModifiers.Add(modifier);
			}
		}

		// will not despawn naturally
		public override bool CheckActive() => false;

		// doesnt drop potions
		public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.None;
    }
}
