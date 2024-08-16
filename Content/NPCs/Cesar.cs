using eslamio.Content.Items.Consumables;
using eslamio.Content.Items.Pets;
using eslamio.Content.Items.Weapons;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
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
	public class Cesar : ModNPC
	{
		public const string ShopName1 = "Shop";

		private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;

		public override void Load() {
			// Adds our Shimmer Head to the NPCHeadLoader.
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
		}

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
			NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 0; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
			NPCID.Sets.AttackTime[Type] = 70; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 15; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
			NPCID.Sets.HatOffsetY[Type] = -2; // For when a party is active, the party hat spawns at a Y offset.
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

			NPCID.Sets.ShimmerTownTransform[Type] = true; // Allows for this NPC to have a different texture after touching the Shimmer liquid.

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
				// Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
				// If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			// Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
			// NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
			NPC.Happiness
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Like)
				.SetBiomeAffection<HallowBiome>(AffectionLevel.Love)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(ModContent.NPCType<Antonio>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Isaac>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Tsuyar>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Cesar>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Jorge>(), AffectionLevel.Hate)
			; // < Mind the semicolon!

			// This creates a "profile" for ExamplePerson, which allows for different textures during a party and/or while the NPC is shimmered.
			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture)),
				new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex)
			);
		}

		public override void SetDefaults() {
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 5;
			NPC.defense = 0;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			NPC.rarity = 10000;

			AnimationType = NPCID.SantaClaus;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("HOLA SOY EL CESAR YO ESCRIBO MI PROPIO BESTIARIO JAJA PUTO EL QUE LO LEA PINCHES PENDEJOS LOS ODIO")
			});
		}

		// The PreDraw hook is useful for drawing things before our sprite is drawn or running code before the sprite is drawn
		// Returning false will allow you to manually draw your NPC
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			// This code slowly rotates the NPC in the bestiary
			// (simply checking NPC.IsABestiaryIconDummy and incrementing NPC.Rotation won't work here as it gets overridden by drawModifiers.Rotation each tick)
			if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(Type, out NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers)) {
				drawModifiers.Rotation += 0.01f;

				// Replace the existing NPCBestiaryDrawModifiers with our new one with an adjusted rotation
				NPCID.Sets.NPCBestiaryDrawOffset.Remove(Type);
				NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			}

			return true;
		}

		public override void HitEffect(NPC.HitInfo hit) {
			NPC.life = 250;
		}

		public override bool UsesPartyHat() {
			return false;
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) { // Requirements for the town NPC to spawn.
			// no llega otro npc si lo tienes en el inventario
			for (int k = 0; k < Main.maxPlayers; k++) {
				Player player = Main.player[k];
				if (!player.active) {
					continue;
				}

				if (player.inventory.Any(item => item.type == ModContent.ItemType<CesarSpawner>())) {
					return false;
				}
			}

			// llega despues del cerebro / tragamundos
			if (NPC.downedBoss2)
                return true;
			else
            	return false;
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"cesar"
			};
		}

		// esto es lo que me hace invencible porque yo soy el cesar y no me importa nada los odio a todos a la verga
		public override bool? CanBeHitByProjectile(Projectile projectile) { return false; }
        public override bool? CanBeHitByItem(Player player, Item item) { return false; }
        public override bool CanBeHitByNPC(NPC attacker) { return false; }
		//

        public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();
			
			if (Main.LocalPlayer.name.Contains("cesar") || Main.LocalPlayer.name.Contains("Cesar"))
			{
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.cesartxt1"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.cesartxt2"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.cesartxt3"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.cesartxt4"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.cesartxt5"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.cesartxt6"));
			}
			else
			{
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.youtxt1", Main.LocalPlayer.name));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.youtxt2", Main.LocalPlayer.name));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.youtxt3", Main.LocalPlayer.name));
				if (Main.LocalPlayer.Male) 
					chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.youtxt4", Main.LocalPlayer.name));
				else
					chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.youtxt5", Main.LocalPlayer.name));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.youtxt6", Main.LocalPlayer.name));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.youtxt7", Main.LocalPlayer.name));
			}

			if (Main.LocalPlayer.Male)
			{
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.mantxt1"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.mantxt2"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.mantxt3"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.mantxt4"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.mantxt5"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.mantxt6"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.mantxt7"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.mantxt8"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.mantxt9"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.mantxt10"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.mantxt11"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.mantxt12"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.mantxt13"));
			}
			else
			{
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.femtxt1"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.femtxt2"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.femtxt3"));
				chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.femtxt4"));
			}

			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text0"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text1"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text2"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text3"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text4"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text5"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text6"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text7"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text8"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text9"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text10"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text11"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text12"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text13"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text14"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text15"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text16"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text17"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text18"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text19"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text20"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text21"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text22"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text23"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text24"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text25"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text26"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text27"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text28"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text29"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text30"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text31"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text32"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text33"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text34"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text35"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text36"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text37"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text38"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text39"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text40"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text41"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text42"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text43"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text44"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text45"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text46"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text47"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text48"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text49"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text50"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text51"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text52"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text53"));
			chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Cesar.text54"));

			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.

			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = Language.GetTextValue("LegacyInterface.28");
			button2 = "Ganar";
			/*if (!NPC.IsShimmerVariant)
				button2 = "Ganar";
			else
				button2 = "puto";*/
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton)
				shop = ShopName1;
			else
			{
				Main.LocalPlayer.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason("el cesar le gano a " + Main.LocalPlayer.name + " jaja"), 69420420f, 1);

				if (Main.LocalPlayer.Male)
					ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("<cesar> no mames que pendejo jajaj"), Colors.RarityNormal);
				else
					ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("<cesar> no mames que pendeja jajaj"), Colors.RarityNormal);
			}
		}

		// Not completely finished, but below is what the NPC will sell
		public override void AddShops() {
			var npcShop = new NPCShop(Type, ShopName1)
				.Add<Transmutator>()
				.Add(ItemID.WireKite)
				.Add(new Item(ItemID.Wire) {shopCustomPrice = Item.buyPrice(copper: 0)})
				.Add(ItemID.ActuationRod)
				.Add(new Item(ItemID.Actuator) {shopCustomPrice = Item.buyPrice(copper: 0)})
				.Add(ItemID.TimerOneFourthSecond)
				.Add(ItemID.TimerOneHalfSecond)
				.Add(ItemID.Timer1Second)
				.Add(ItemID.Timer3Second)
				.Add(ItemID.Timer5Second)

				.Add(ItemID.Lever)
				.Add(ItemID.Switch)
				.Add(ItemID.Teleporter)
				.Add(ItemID.RedPressurePlate)
				.Add(ItemID.YellowPressurePlate)
				.Add(ItemID.ProjectilePressurePad)
				.Add(ItemID.DartTrap)
				.Add(ItemID.SlimeStatue)
				.Add(ItemID.BoulderStatue)
				.Add(ItemID.HeartStatue)

				.Add(ItemID.CombatWrench)
				.Add<Screwdriver>()
				.Add<CesarPetItem>()
				.Add(ItemID.LogicGate_AND)
				.Add(ItemID.LogicGate_NAND)
				.Add(ItemID.LogicGate_OR)
				.Add(ItemID.LogicGate_NOR)
				.Add(ItemID.LogicGate_XOR)
				.Add(ItemID.LogicGateLamp_On)
				.Add(ItemID.LogicGateLamp_Off)
				
				.Add(ItemID.MrsClauseHat)
				.Add(ItemID.WhoopieCushion)
				.Add(ItemID.PinkDye)
				.Add(ItemID.VioletDye)
				.Add(ItemID.PurpleDye)
				.Add(ItemID.BoringBow)
				.Add(ItemID.DD2EnergyCrystal)
				.Add(ItemID.Fake_newchest2);

			npcShop.Register(); // Name of this shop tab
		}

		public override void ModifyActiveShop(string shopName, Item[] items) {
			foreach (Item item in items) {
				// Skip 'air' items and null items.
				if (item == null || item.type == ItemID.None) {
					continue;
				}
			}
			//ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral("CALLATE TMODLOADER"), Colors.RarityNormal, Main.LocalPlayer.whoAmI);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ItemID.MrsClauseHat));
		}

		// Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
		public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

		public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 50;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 20;
			randExtraCooldown = 20;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
			projType = ProjectileID.BoulderStaffOfEarth;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
			multiplier = 12f;
			randomOffset = 2f;
		}
	}
}