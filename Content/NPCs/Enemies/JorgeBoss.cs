using eslamio.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

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

			// Add this in for bosses that have a summon item, requires corresponding code in the item (See MinionBossSummonItem.cs)
			NPCID.Sets.MPAllowedEnemies[Type] = true;

			// Automatically group with other bosses
			NPCID.Sets.BossBestiaryPriority.Add(Type);
			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				//CustomTexturePath = $"Terraria/Images/NPC_{NPCID.KingSlime}",
				PortraitScale = 2f, // Portrait refers to the full picture when clicking on the icon in the bestiary
				//PortraitPositionYOverride = 0f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() {
			NPC.width = 162 * 10;
			NPC.height = 108 * 10;
			NPC.scale = 10f;
			NPC.damage = 32;
			NPC.defense = 32;
			NPC.lifeMax = 32000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			NPC.value = Item.buyPrice(gold: 32, silver: 32, copper: 32);
			NPC.rarity = 32;
			NPC.boss = true;
			NPC.npcSlots = 10f; // Take up open spawn slots, preventing random NPCs from spawning during the fight

			NPC.aiStyle = NPCAIStyleID.KingSlime;
			AnimationType = NPCID.KingSlime;

			// Default buff immunities should be set in SetStaticDefaults through the NPCID.Sets.ImmuneTo{X} arrays.
			// To dynamically adjust immunities of an active NPC, NPC.buffImmune[] can be changed in AI: NPC.buffImmune[BuffID.OnFire] = true;
			// This approach, however, will not preserve buff immunities. To preserve buff immunities, use the NPC.BecomeImmuneTo and NPC.ClearImmuneToBuffs methods instead, as shown in the ApplySecondStageBuffImmunities method below.

			// The following code assigns a music track to the boss in a simple way.
			if (!Main.dedServ) {
				Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/jorgeboss");
			}
		}

        public override bool PreAI()
        {
			NPC.position.X += NPC.width / 2;
			NPC.position.Y += NPC.height;
			NPC.scale = 10f;
			//NPC.color = Color.SpringGreen;
			NPC.width = (int)(98f * NPC.scale);
			NPC.height = (int)(92f * NPC.scale);
			NPC.position.X -= NPC.width / 2;
			NPC.position.Y -= NPC.height;

            return base.PreAI();
        }

        public override void AI()
        {
            base.AI();

			NPC.position.X += NPC.width / 2;
			NPC.position.Y += NPC.height;
			NPC.scale = 10f;
			//NPC.color = Color.SpringGreen;
			NPC.width = (int)(98f * NPC.scale);
			NPC.height = (int)(92f * NPC.scale);
			NPC.position.X -= NPC.width / 2;
			NPC.position.Y -= NPC.height;

			foreach (var player in Main.ActivePlayers)
				player.GetModPlayer<SlimeShaderPlayer>().IsActive = true;
        }

        /*public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			// Retrieve reference to shader
			var shader = GameShaders.Misc["EmpressBlade"];
			shader.Apply(null);

            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
        }*/

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

		public override void OnKill() {
			// This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
			//NPC.SetEventFlagCleared(ref DownedBossSystem.downedMinionBoss, -1);

			// Since this hook is only ran in singleplayer and serverside, we would have to sync it manually.
			// Thankfully, vanilla sends the MessageID.WorldData packet if a BOSS was killed automatically, shortly after this hook is ran

			// If your NPC is not a boss and you need to sync the world (which includes ModSystem, check DownedBossSystem), use this code:
			/*
			if (Main.netMode == NetmodeID.Server) {
				NetMessage.SendData(MessageID.WorldData);
			}
			*/
		}

		public override void BossLoot(ref string name, ref int potionType) {
			// Here you'd want to change the potion type that drops when the boss is defeated. Because this boss is early pre-hardmode, we keep it unchanged
			// (Lesser Healing Potion). If you wanted to change it, simply write "potionType = ItemID.HealingPotion;" or any other potion type
			potionType = ItemID.None;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
			cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
			return true;
		}

		/*public override void FindFrame(int frameHeight) {
			// This NPC animates with a simple "go from start frame to final frame, and loop back to start frame" rule
			// In this case: First stage: 0-1-2-0-1-2, Second stage: 3-4-5-3-4-5, 5 being "total frame count - 1"
			int startFrame = 0;
			int finalFrame = 2;

			int frameSpeed = 5;
			NPC.frameCounter += 0.5f;
			NPC.frameCounter += NPC.velocity.Length() / 10f; // Make the counter go faster with more movement speed
			if (NPC.frameCounter > frameSpeed) {
				NPC.frameCounter = 0;
				NPC.frame.Y += frameHeight;

				if (NPC.frame.Y > finalFrame * frameHeight) {
					NPC.frame.Y = startFrame * frameHeight;
				}
			}
		}*/

		public override void HitEffect(NPC.HitInfo hit) {
			// If the NPC dies, spawn gore and play a sound
			if (Main.netMode == NetmodeID.Server) {
				// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
				return;
			}

			if (NPC.life <= 0) {
				// These gores work by simply existing as a texture inside any folder which path contains "Gores/"
				/*int backGoreType = Mod.Find<ModGore>("MinionBossBody_Back").Type;
				int frontGoreType = Mod.Find<ModGore>("MinionBossBody_Front").Type;
				var entitySource = NPC.GetSource_Death();

				for (int i = 0; i < 2; i++) {
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
				}*/

				SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

				// This adds a screen shake (screenshake) similar to Deerclops
				PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 6f, 20, 1000f, FullName);
				Main.instance.CameraModifiers.Add(modifier);
			}
		}
    }
}
