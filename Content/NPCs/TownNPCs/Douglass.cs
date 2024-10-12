using eslamio.Content.Items.Consumables;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Utilities;

namespace eslamio.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    public class Douglass : ModNPC
    {
        public const string ShopName = "Shop";

        private static Profiles.StackedNPCProfile NPCProfile;

        /*public override void Load() {
			// Adds our Shimmer Head to the NPCHeadLoader.
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
		}*/

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

            NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
            NPCID.Sets.AttackFrameCount[Type] = 5; // The amount of frames in the attacking animation.

            NPCID.Sets.DangerDetectRange[Type] = 100; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
            NPCID.Sets.AttackType[Type] = 3; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
            NPCID.Sets.AttackTime[Type] = 25; // The amount of time it takes for the NPC's attack animation to be over once it starts.
            NPCID.Sets.AttackAverageChance[Type] = 10; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
            
            NPCID.Sets.HatOffsetY[Type] = -2; // For when a party is active, the party hat spawns at a Y offset.
            
            NPCID.Sets.ShimmerTownTransform[NPC.type] = false; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Velocity = 1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                //.SetBiomeAffection<JungleBiome>(AffectionLevel.Like)
                //.SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(ModContent.NPCType<Antonio>(), AffectionLevel.Like)
                .SetNPCAffection(ModContent.NPCType<Tsuyar>(), AffectionLevel.Like)
                .SetNPCAffection(ModContent.NPCType<Isaac>(), AffectionLevel.Like)
                .SetNPCAffection(ModContent.NPCType<Cesar>(), AffectionLevel.Like)
                .SetNPCAffection(ModContent.NPCType<Jorge>(), AffectionLevel.Hate)
            ;

            // This creates a "profile", which allows for different textures during a party and/or while the NPC is shimmered.
            NPCProfile = new Profiles.StackedNPCProfile(
                new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture))
            );
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
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

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("Era un día como cualquier otro solo que llovía sangre, no le tome importancia asi que fui al tianguis de mi tío. Mi tío vende productos audiovisuales de gente haciendo el amor. Yo fui para buscar lo nuevo de Alexis Texas y Abella Danger, estaba todo agotado para mí sorpresa. Entonces mi tío se me acerca y me dice que uno de sus clientes le regreso un DVD. Así que me lo regala, en eso me percató que el DVD tiene un nombre muy extraño, decía: \"el pepe ete sech\". Se me hizo extraño, pero pensé que era un producto audiovisual de femboys haciendo el amor así que me lo lleve de todas formas hacia mi casa para reproducirlo en mi televisor sacado de coppel a 12 meses. Cuando le doy al play me percato que solo son unas palabras en pantalla que decían: \"yo soy el pepe, yo soy el ete sech, yo soy Dios\". Justo cuando termine de leer ese texto que estaba reproduciendo mi televisor, surge un cortocircuito y en eso una figura grande y oscura sale de mi televisor, se me acerca poco a poco con una mirada muy penetrante, tenía los ojos como de un demonio, no lo podía creer me quedé en shock, no podía moverme. Con una voz muy seductora, varonil y erótica me dice: \"yo soy el pepe ete sech\". En eso mi dopamina aumenta de forma exagerada, tanto que mi erección se vuelve mortal y después me dió diabetes tipo 2, tuve que cortar mis piernas para regular mi sangre. En eso el pepe ete sech me dice: \"para que te deje en paz tienes que darme tu alma\". No me quedo de otra, tuve que dar mi alma. Actualmente vago en las calles de ciudad Juárez sin alma, como un fantasma... Pero no le tome importancia."),
            ]);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // Create gore when the NPC is killed.
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 2), NPC.velocity, 11);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 2), NPC.velocity, 12);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 3), NPC.velocity, 13);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 3), NPC.velocity, 11);
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        { // Requirements for the town NPC to spawn.
            if (NPC.AnyNPCs(NPCID.BestiaryGirl))
                return true;
            else
                return false;
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return NPCProfile;
        }

        public override List<string> SetNPCNameList()
        {
            return [
                "Douglass",
                "Douglasss",
                "Douglassss",
                "Douglasssss",
                "Douglaaasssss",
                "BrownishSea",
                "yeyotomate11",
                "yeyopapa12",
                "yeyozanahoria13",
                "yeyosandia14",
                "Diegol",
                "Diegardo",
                "Diego",
                "Diego Rulz, ok?",
                "BNB Chain",
                "brela",
                "pepe",
                "etesech",
                "elpepetesech",
                "etepepe",
                "tilin",
                "tilinmaster",
                "elreytilin88",
            ];
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new();
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Douglass.StandardDialogue1"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Douglass.StandardDialogue2"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Douglass.StandardDialogue3"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Douglass.StandardDialogue4"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Douglass.StandardDialogue5"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Douglass.StandardDialogue6"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Douglass.StandardDialogue7"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Douglass.StandardDialogue8"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Douglass.StandardDialogue9"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Douglass.StandardDialogue10"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Douglass.StandardDialogue11"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Douglass.StandardDialogue12"));

            string chosenChat = chat;
            return chosenChat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        { // What the chat buttons are when you open up the chat UI
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
                shop = ShopName;
        }

        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName)
                .Add(ItemID.SleepingIcon)
                //.Add<Whey>()
            ;

            npcShop.Register(); // Name of this shop tab
        }

        public override void ModifyActiveShop(string shopName, Item[] items)
        {
            foreach (Item item in items)
            {
                // Skip 'air' items and null items.
                if (item == null || item.type == ItemID.None)
                {
                    continue;
                }
            }
        }

        // Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
        public override bool CanGoToStatue(bool toKingStatue) => true;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 40;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 10;
            randExtraCooldown = 10;
        }

        public override void DrawTownAttackSwing(ref Texture2D item, ref Rectangle itemFrame, ref int itemSize, ref float scale, ref Vector2 offset)//Allows you to customize how this town NPC's weapon is drawn when this NPC is swinging it (this NPC must have an attack type of 3). ItemType is the Texture2D instance of the item to be drawn (use Main.PopupTexture[id of item]), itemSize is the width and height of the item's hitbox
        {
            item = TextureAssets.Item[ItemID.BatBat].Value; //this defines the item that this npc will use
            itemFrame.Width = 52;
            itemFrame.Height = 52;
            itemSize = 52;
            scale = 1f;
        }

        public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight) //  Allows you to determine the width and height of the item this town NPC swings when it attacks, which controls the range of this NPC's swung weapon.
        {
            itemWidth = 38;
            itemHeight = 38;
        }
    }
}