using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace eslamio.Content.NPCs
{
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class Jorge : ModNPC
	{
		public const string ShopName = "Shop";

		/*public override void Load() {
			// Adds our Shimmer Head to the NPCHeadLoader.
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
		}*/

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.AttackFrameCount[Type] = 323232; // The amount of frames in the attacking animation.
			NPCID.Sets.DangerDetectRange[Type] = 0; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 3; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
			NPCID.Sets.AttackTime[Type] = 323232; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 323232; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
			NPCID.Sets.HatOffsetY[Type] = 20; // For when a party is active, the party hat spawns at a Y offset.
			NPCID.Sets.ShimmerTownTransform[NPC.type] = false; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

			NPCID.Sets.ShimmerTownTransform[Type] = false; // Allows for this NPC to have a different texture after touching the Shimmer liquid.

			// Connects this NPC with a custom emote.
			// This makes it when the NPC is in the world, other NPCs will "talk about him".
			// By setting this you don't have to override the PickEmote method for the emote to appear.
			//NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<JorgeEmote>();

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
				// Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
				// If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			// Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
			// NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
			NPC.Happiness
				//.SetBiomeAffection<TheUnderworld>(AffectionLevel.Love) // Example Person prefers the forest.

				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Nurse, AffectionLevel.Love)
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Princess, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Stylist, AffectionLevel.Love)
				.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Love)

				.SetNPCAffection(NPCID.TownSlimeBlue, AffectionLevel.Like)
				.SetNPCAffection(NPCID.TownSlimeCopper, AffectionLevel.Like)
				.SetNPCAffection(NPCID.TownSlimeGreen, AffectionLevel.Like)
				.SetNPCAffection(NPCID.TownSlimeOld, AffectionLevel.Like)
				.SetNPCAffection(NPCID.TownSlimePurple, AffectionLevel.Like)
				.SetNPCAffection(NPCID.TownSlimeRainbow, AffectionLevel.Like)
				.SetNPCAffection(NPCID.TownSlimeRed, AffectionLevel.Like)
				.SetNPCAffection(NPCID.TownSlimeYellow, AffectionLevel.Like)

				.SetNPCAffection(NPCID.TownBunny, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.TownCat, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.TownDog, AffectionLevel.Dislike)

				.SetNPCAffection(NPCID.Angler, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Clothier, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Cyborg, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Golfer, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Painter, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Pirate, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.SantaClaus, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Truffle, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Wizard, AffectionLevel.Hate)
				.SetNPCAffection(ModContent.NPCType<Jorge>(), AffectionLevel.Hate)

			; // < Mind the semicolon!
		}

		public override void SetDefaults() {
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 20;
			NPC.height = 22;
			NPC.aiStyle = 7;
			NPC.damage = 32;
			NPC.defense = 32;
			NPC.lifeMax = 32;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			NPC.rarity = -10000;

			AnimationType = NPCID.Guide;
		}

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("EL LLER LLER...")
			});
		}

		// The PreDraw hook is useful for drawing things before our sprite is drawn or running code before the sprite is drawn
		// Returning false will allow you to manually draw your NPC
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			// This code slowly rotates the NPC in the bestiary
			// (simply checking NPC.IsABestiaryIconDummy and incrementing NPC.Rotation won't work here as it gets overridden by drawModifiers.Rotation each tick)
			/*if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(Type, out NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers)) {
				drawModifiers.Rotation += 0.001f;

				// Replace the existing NPCBestiaryDrawModifiers with our new one with an adjusted rotation
				NPCID.Sets.NPCBestiaryDrawOffset.Remove(Type);
				NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			}*/

			return true;
		}

		public override void HitEffect(NPC.HitInfo hit) {
			// Create gore when the NPC is killed.
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0) {
				for (int i = 0; i < 32; i++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Poop);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) { // Requirements for the town NPC to spawn.
			if (NPC.downedSlimeKing)
                return true;
			else
            	return false;

			/*for (int i = 0; i < 255; i++)
			{
				Player player = Main.player[i];
				foreach (Item item in player.inventory)
				{
					if (item.type == ItemID.PoopBlock)
						return true;
				}
			}
			return false;*/
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Jorge",
				"Martin",
				"Fragoso",
				"Velazquez",
				"Llorlly",
				"llarllar",
				"llerller",
				"llirllir",
				"llorllor",
				"llurllur",
				"Eslamio",
				"Jorge Martin Fragoso Velazquez",
				"Jotorge",
				"Fartin",
				"Faggotso",
				"Cagoso",
				"Putazquez",
				"Llorllazquez",
				"Llorlloso"
			};
		}

		public override void FindFrame(int frameHeight) {
			/*npc.frame.Width = 40;
			if (((int)Main.time / 10) % 2 == 0)
			{
				npc.frame.X = 40;
			}
			else
			{
				npc.frame.X = 0;
			}*/
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

			int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
			if (partyGirl >= 0 && Main.rand.NextBool(4)) {
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Jorge.PartyGirlDialogue", Main.npc[partyGirl].GivenName));
			}
			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Jorge.StandardDialogue1"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Jorge.StandardDialogue2"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Jorge.StandardDialogue3"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Jorge.StandardDialogue4"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Jorge.CommonDialogue"), 8.0);

			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.

			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				shop = ShopName;
			}
		}

		// Not completely finished, but below is what the NPC will sell
		public override void AddShops() {
			var npcShop = new NPCShop(Type, ShopName)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)

				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)

				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock)
				
				.Add(ItemID.PoopBlock)
				.Add(ItemID.PoopBlock);

			npcShop.Register(); // Name of this shop tab
		}

		public override void ModifyActiveShop(string shopName, Item[] items) {
			foreach (Item item in items) {
				// Skip 'air' items and null items.
				if (item == null || item.type == ItemID.None) {
					continue;
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ItemID.PoopBlock));
		}

		// Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
		public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

		public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 0;
			knockback = 0f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 323232;
			randExtraCooldown = 323232;
		}
	}
}