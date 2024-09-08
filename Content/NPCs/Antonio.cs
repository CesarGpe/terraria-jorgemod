using eslamio.Content.Items.Consumables;
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
	public class Antonio : ModNPC
	{
		public const string ShopName = "Shop";

		private static Profiles.StackedNPCProfile NPCProfile;

		/*public override void Load() {
			// Adds our Shimmer Head to the NPCHeadLoader.
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
		}*/

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.AttackFrameCount[Type] = 5; // The amount of frames in the attacking animation.
			NPCID.Sets.DangerDetectRange[Type] = 100; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 3; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
			NPCID.Sets.AttackTime[Type] = 35; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 10; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
			NPCID.Sets.HatOffsetY[Type] = -2; // For when a party is active, the party hat spawns at a Y offset.
			NPCID.Sets.ShimmerTownTransform[NPC.type] = false; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

			NPCID.Sets.ShimmerTownTransform[Type] = false; // Allows for this NPC to have a different texture after touching the Shimmer liquid.

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
				//.SetBiomeAffection<JungleBiome>(AffectionLevel.Like) // Example Person prefers the forest.
				//.SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike) // Example Person dislikes the snow.
				.SetNPCAffection(ModContent.NPCType<Antonio>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Tsuyar>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Isaac>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Cesar>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Jorge>(), AffectionLevel.Hate)
			; // < Mind the semicolon!

			// This creates a "profile" for ExamplePerson, which allows for different textures during a party and/or while the NPC is shimmered.
			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture))
			);
		}

		public override void SetDefaults() {
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("No dejes que se acerque a tu thinner."),
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
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 2), NPC.velocity, 11);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 2), NPC.velocity, 12);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 3), NPC.velocity, 13);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 3), NPC.velocity, 11);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) { // Requirements for the town NPC to spawn.
			if (NPC.downedBoss1)
                return true;
			else
            	return false;
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Toño",
				"Toño",
				"Antonio",
				"Antonio",
				"Ancoito",
				"Ancoño",
				"Ñoño",
				"Antojo"
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

			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Antonio.StandardDialogue1"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Antonio.StandardDialogue2"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Antonio.StandardDialogue3"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Antonio.StandardDialogue4"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Antonio.StandardDialogue5"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Antonio.StandardDialogue6"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Antonio.StandardDialogue7"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Antonio.StandardDialogue8"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Antonio.StandardDialogue9"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Antonio.StandardDialogue10"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Antonio.StandardDialogue11"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Antonio.StandardDialogue12"));

			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.

			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton)
				shop = ShopName;
		}

		public override void AddShops() {
			var npcShop = new NPCShop(Type, ShopName)
				.Add<Whey>()
				.Add<Fentanilo>()
				.Add(ItemID.BuilderPotion)
				.Add(ItemID.CalmingPotion)
				.Add(ItemID.SpectrePaintbrush)
				.Add(ItemID.SpectrePaintRoller)
				.Add(ItemID.SpectrePaintScraper)
				.Add(ItemID.AncientChisel)
				.Add(ItemID.TreasureMagnet)
				.Add(ItemID.PortableStool)
				.Add(ItemID.BrickLayer)
				.Add(ItemID.ExtendoGrip)
				.Add(ItemID.PaintSprayer)
				.Add(ItemID.PortableCementMixer);

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

		// Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
		public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

		public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 40;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 10;
			randExtraCooldown = 10;
		}

		public override void DrawTownAttackSwing(ref Texture2D item, ref Rectangle itemFrame, ref int itemSize, ref float scale, ref Vector2 offset)//Allows you to customize how this town NPC's weapon is drawn when this NPC is swinging it (this NPC must have an attack type of 3). ItemType is the Texture2D instance of the item to be drawn (use Main.PopupTexture[id of item]), itemSize is the width and height of the item's hitbox
        {
			item = TextureAssets.Item[ItemID.IronHammer].Value; //this defines the item that this npc will use
			itemFrame.Width = 38;
			itemFrame.Height = 38;
			itemSize = 38;
            scale = 1f;
        }

		public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight) //  Allows you to determine the width and height of the item this town NPC swings when it attacks, which controls the range of this NPC's swung weapon.
        {
            itemWidth = 38;
            itemHeight = 38;
        }
	}
}