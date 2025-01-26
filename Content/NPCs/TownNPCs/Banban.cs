using eslamio.Content.Items.Consumables;
using eslamio.Content.Items.Pets;
using eslamio.Content.Items.Pets.Drone;
using eslamio.Content.Items.Pets.Familiar;
using eslamio.Content.Projectiles;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Utilities;

namespace eslamio.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    public class Banban : ModNPC
    {
        public const string ShopName = "Shop";

        private static Profiles.StackedNPCProfile NPCProfile;

        /*public override void Load() {
			// Adds our Shimmer Head to the NPCHeadLoader.
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
		}*/

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 26; // The total amount of frames the NPC has

            NPCID.Sets.ExtraFramesCount[Type] = 10; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
            NPCID.Sets.AttackFrameCount[Type] = 5; // The amount of frames in the attacking animation.

            NPCID.Sets.DangerDetectRange[Type] = 105; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
            NPCID.Sets.AttackType[Type] = 0; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
            NPCID.Sets.AttackTime[Type] = 40; // The amount of time it takes for the NPC's attack animation to be over once it starts.
            NPCID.Sets.AttackAverageChance[Type] = 2; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.

            NPCID.Sets.HatOffsetY[Type] = -4; // For when a party is active, the party hat spawns at a Y offset.

            NPCID.Sets.ShimmerTownTransform[NPC.type] = false; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Velocity = 1f,
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                .SetBiomeAffection<OceanBiome>(AffectionLevel.Like)
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate)
                .SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Angler, AffectionLevel.Hate)
            ;

            // This creates a "profile", which allows for different textures during a party and/or while the NPC is shimmered.
            NPCProfile = new Profiles.StackedNPCProfile(
                new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture))
            //new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex)
            );
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 40;
            NPC.defense = 50;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Guide;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("Banban is the titular deuteragonist of the 2023 indie-horror game Garten of Banban. The lead mascot of the titular kindergarten. He was created by Uthman Adam to take care of the children, along with the other mascots. After the events of Bring a Friend Day, he aids the protagonist to help find their child, who are said to be somewhere in the abyss."),
            ]);
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<BanbanProjSpawner>();
            attackDelay = 28;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            gravityCorrection *= 15f;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 40;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 60;
            randExtraCooldown = 60;
        }

        public override int? PickEmote(Player closestPlayer, List<int> emoteList, WorldUIAnchor otherAnchor)
        {
            // banban will not use emote bubbles
            var number = Main.rand.Next(1, 5);
            SoundEngine.PlaySound(new("eslamio/Assets/Sounds/Banban/Emote" + number), NPC.position);
            return -1;
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
        {
            return true;
        }

        public override List<string> SetNPCNameList()
        {
            return [
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Banban",
                "Evil Banban",
                "Banban 2",
                "Batman",
                "Uthman Adam",
                "Dr. Uthman",
            ];
        }

        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName)
                .Add(ItemID.SleepingIcon)
                .Add<DronePetItem>()
                .Add<FamiliarPetItem>()
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

        public override string GetChat()
        {
            var number = Main.rand.Next(1, 13);
            SoundEngine.PlaySound(new("eslamio/Assets/Sounds/Banban/Line" + number), NPC.position);

            WeightedRandom<string> chat = new();
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Banban.StandardDialogue1"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Banban.StandardDialogue2"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Banban.StandardDialogue3"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Banban.StandardDialogue4"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Banban.StandardDialogue5"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Banban.StandardDialogue6"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Banban.StandardDialogue7"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Banban.StandardDialogue8"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Banban.StandardDialogue9"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Banban.StandardDialogue10"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Banban.StandardDialogue11"));
            chat.Add(Language.GetTextValue("Mods.eslamio.Dialogue.Banban.StandardDialogue12"));

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

        public override ITownNPCProfile TownNPCProfile()
        {
            return NPCProfile;
        }

        // Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

        /*
        public override void AI()
        {

            NPC.ShimmeredTownNPCs[NPC.type] = NPC.IsShimmerVariant;
            if (NPC.type == 441 && NPC.GivenName == "Andrew")
            {
                NPC.defDefense = 200;
            }
            int num = 300;
            if (NPC.type == 638 || NPC.type == 656 || NPCID.Sets.IsTownSlime[NPC.type])
            {
                num = 0;
            }
            bool flag = Main.raining;
            if (!Main.dayTime)
            {
                flag = true;
            }
            if (Main.eclipse)
            {
                flag = true;
            }
            if (Main.slimeRain)
            {
                flag = true;
            }
            float num42 = 1f;
            if (Main.masterMode)
            {
                NPC.defense = (NPC.dryadWard ? (NPC.defDefense + 14) : NPC.defDefense);
            }
            else if (Main.expertMode)
            {
                NPC.defense = (NPC.dryadWard ? (NPC.defDefense + 10) : NPC.defDefense);
            }
            else
            {
                NPC.defense = (NPC.dryadWard ? (NPC.defDefense + 6) : NPC.defDefense);
            }
            if (NPC.isLikeATownNPC)
            {
                if (NPC.combatBookWasUsed)
                {
                    num42 += 0.2f;
                    NPC.defense += 6;
                }
                if (NPC.combatBookVolumeTwoWasUsed)
                {
                    num42 += 0.2f;
                    NPC.defense += 6;
                }
                if (NPC.downedBoss1)
                {
                    num42 += 0.1f;
                    NPC.defense += 3;
                }
                if (NPC.downedBoss2)
                {
                    num42 += 0.1f;
                    NPC.defense += 3;
                }
                if (NPC.downedBoss3)
                {
                    num42 += 0.1f;
                    NPC.defense += 3;
                }
                if (NPC.downedQueenBee)
                {
                    num42 += 0.1f;
                    NPC.defense += 3;
                }
                if (Main.hardMode)
                {
                    num42 += 0.4f;
                    NPC.defense += 12;
                }
                if (NPC.downedQueenSlime)
                {
                    num42 += 0.15f;
                    NPC.defense += 6;
                }
                if (NPC.downedMechBoss1)
                {
                    num42 += 0.15f;
                    NPC.defense += 6;
                }
                if (NPC.downedMechBoss2)
                {
                    num42 += 0.15f;
                    NPC.defense += 6;
                }
                if (NPC.downedMechBoss3)
                {
                    num42 += 0.15f;
                    NPC.defense += 6;
                }
                if (NPC.downedPlantBoss)
                {
                    num42 += 0.15f;
                    NPC.defense += 8;
                }
                if (NPC.downedEmpressOfLight)
                {
                    num42 += 0.15f;
                    NPC.defense += 8;
                }
                if (NPC.downedGolemBoss)
                {
                    num42 += 0.15f;
                    NPC.defense += 8;
                }
                if (NPC.downedAncientCultist)
                {
                    num42 += 0.15f;
                    NPC.defense += 8;
                }
                NPCLoader.BuffTownNPC(ref num42, ref NPC.defense);
            }
            if (NPC.type == 142 && Main.netMode != 1 && !Main.xMas)
            {
                NPC.StrikeNPCNoInteraction(9999, 0f, 0);
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(28, -1, -1, null, NPC.whoAmI, 9999f);
                }
            }
            if ((NPC.type == 148 || NPC.type == 149) && NPC.localAI[0] == 0f)
            {
                NPC.localAI[0] = Main.rand.Next(1, 5);
            }
            if (NPC.type == 124)
            {
                int num53 = NPC.lazyNPCOwnedProjectileSearchArray[NPC.whoAmI];
                bool flag12 = false;
                if (Main.projectile.IndexInRange(num53))
                {
                    Projectile projectile = Main.projectile[num53];
                    if (projectile.active && projectile.type == 582 && projectile.ai[1] == (float)NPC.whoAmI)
                    {
                        flag12 = true;
                    }
                }
                NPC.localAI[0] = flag12.ToInt();
            }
            if ((NPC.type == 362 || NPC.type == 364 || NPC.type == 602 || NPC.type == 608) && Main.netMode != 1 && (NPC.velocity.Y > 4f || NPC.velocity.Y < -4f || NPC.wet))
            {
                int num64 = NPC.direction;
                NPC.Transform(NPC.type + 1);
                NPC.TargetClosest();
                NPC.direction = num64;
                NPC.netUpdate = true;
                return;
            }
            switch (NPC.type)
            {
                case 588:
                    NPC.savedGolfer = true;
                    break;
                case 441:
                    NPC.savedTaxCollector = true;
                    break;
                case 107:
                    NPC.savedGoblin = true;
                    break;
                case 108:
                    NPC.savedWizard = true;
                    break;
                case 124:
                    NPC.savedMech = true;
                    break;
                case 353:
                    NPC.savedStylist = true;
                    break;
                case 369:
                    NPC.savedAngler = true;
                    break;
                case 550:
                    NPC.savedBartender = true;
                    break;
            }
            NPC.dontTakeDamage = false;
            if (NPC.ai[0] == 25f)
            {
                NPC.dontTakeDamage = true;
                if (NPC.ai[1] == 0f)
                {
                    NPC.velocity.X = 0f;
                }
                NPC.shimmerWet = false;
                NPC.wet = false;
                NPC.lavaWet = false;
                NPC.honeyWet = false;
                if (NPC.ai[1] == 0f && Main.netMode == 1)
                {
                    return;
                }
                if (NPC.ai[1] == 0f && NPC.ai[2] < 1f)
                {
                    NPC.AI_007_TownEntities_Shimmer_TeleportToLandingSpot();
                }
                if (NPC.ai[2] > 0f)
                {
                    NPC.ai[2] -= 1f;
                    if (NPC.ai[2] <= 0f)
                    {
                        NPC.ai[1] = 1f;
                    }
                    return;
                }
                NPC.ai[1] += 1f;
                if (NPC.ai[1] >= 30f)
                {
                    if (!Collision.WetCollision(NPC.position, NPC.width, NPC.height))
                    {
                        NPC.shimmerTransparency = MathHelper.Clamp(NPC.shimmerTransparency - 1f / 60f, 0f, 1f);
                    }
                    else
                    {
                        NPC.ai[1] = 30f;
                    }
                    NPC.velocity = new Vector2(0f, -4f * NPC.shimmerTransparency);
                }
                Rectangle hitbox = NPC.Hitbox;
                hitbox.Y += 20;
                hitbox.Height -= 20;
                float num75 = Main.rand.NextFloatDirection();
                Lighting.AddLight(NPC.Center, Main.hslToRgb((float)Main.timeForVisualEffects / 360f % 1f, 0.6f, 0.65f).ToVector3() * Utils.Remap(NPC.ai[1], 30f, 90f, 0f, 0.7f));
                if (Main.rand.NextFloat() > Utils.Remap(NPC.ai[1], 30f, 60f, 1f, 0.5f))
                {
                    Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(hitbox) + Main.rand.NextVector2Circular(8f, 0f) + new Vector2(0f, 4f), 309, new Vector2(0f, -2f).RotatedBy(num75 * ((float)Math.PI * 2f) * 0.11f), 0, default(Color), 1.7f - Math.Abs(num75) * 1.3f);
                }
                if (NPC.ai[1] > 60f && Main.rand.Next(15) == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vector = Main.rand.NextVector2FromRectangle(NPC.Hitbox);
                        ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerBlock, new ParticleOrchestraSettings
                        {
                            PositionInWorld = vector,
                            MovementVector = NPC.DirectionTo(vector).RotatedBy((float)Math.PI * 9f / 20f * (float)(Main.rand.Next(2) * 2 - 1)) * Main.rand.NextFloat()
                        });
                    }
                }
                NPC.TargetClosest();
                NPCAimedTarget targetData = NPC.GetTargetData();
                if (NPC.ai[1] >= 75f && NPC.shimmerTransparency <= 0f && Main.netMode != 1)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                    Math.Sign(targetData.Center.X - NPC.Center.X);
                    NPC.velocity = new Vector2(0f, -4f);
                    NPC.localAI[0] = 0f;
                    NPC.localAI[1] = 0f;
                    NPC.localAI[2] = 0f;
                    NPC.localAI[3] = 0f;
                    NPC.netUpdate = true;
                    NPC.townNpcVariationIndex = ((NPC.townNpcVariationIndex != 1) ? 1 : 0);
                    NetMessage.SendData(56, -1, -1, null, NPC.whoAmI);
                    NPC.Teleport(NPC.position, 12);
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPC, new ParticleOrchestraSettings
                    {
                        PositionInWorld = NPC.Center
                    });
                }
                return;
            }
            if (NPC.type >= 0 && NPCID.Sets.TownCritter[NPC.type] && NPC.target == 255)
            {
                NPC.TargetClosest();
                if (NPC.position.X < Main.player[NPC.target].position.X)
                {
                    NPC.direction = 1;
                    NPC.spriteDirection = NPC.direction;
                }
                if (NPC.position.X > Main.player[NPC.target].position.X)
                {
                    NPC.direction = -1;
                    NPC.spriteDirection = NPC.direction;
                }
                if (NPC.homeTileX == -1)
                {
                    NPC.UpdateHomeTileState(NPC.homeless, (int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), NPC.homeTileY);
                }
            }
            else if (NPC.homeTileX == -1 && NPC.homeTileY == -1 && NPC.velocity.Y == 0f && !NPC.shimmering)
            {
                NPC.UpdateHomeTileState(NPC.homeless, (int)NPC.Center.X / 16, (int)(NPC.position.Y + (float)NPC.height + 4f) / 16);
            }
            bool flag23 = false;
            int num86 = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
            int num97 = (int)(NPC.position.Y + (float)NPC.height + 1f) / 16;
            NPC.AI_007_FindGoodRestingSpot(num86, num97, out var floorX, out var floorY);
            if (NPC.type == 441)
            {
                NPC.taxCollector = true;
            }
            NPC.directionY = -1;
            if (NPC.direction == 0)
            {
                NPC.direction = 1;
            }
            if (NPC.ai[0] != 24f)
            {
                for (int j = 0; j < 255; j++)
                {
                    if (Main.player[j].active && Main.player[j].talkNPC == NPC.whoAmI)
                    {
                        flag23 = true;
                        if (NPC.ai[0] != 0f)
                        {
                            NPC.netUpdate = true;
                        }
                        NPC.ai[0] = 0f;
                        NPC.ai[1] = 300f;
                        NPC.localAI[3] = 100f;
                        if (Main.player[j].position.X + (float)(Main.player[j].width / 2) < NPC.position.X + (float)(NPC.width / 2))
                        {
                            NPC.direction = -1;
                        }
                        else
                        {
                            NPC.direction = 1;
                        }
                    }
                }
            }
            if (NPC.ai[3] == 1f)
            {
                NPC.life = -1;
                NPC.HitEffect();
                NPC.active = false;
                NPC.netUpdate = true;
                if (NPC.type == 37)
                {
                    SoundEngine.PlaySound(15, (int)NPC.position.X, (int)NPC.position.Y, 0);
                }
                return;
            }
            if (NPC.type == 37 && Main.netMode != 1)
            {
                NPC.UpdateHomeTileState(homeless: false, Main.dungeonX, Main.dungeonY);
                if (NPC.downedBoss3)
                {
                    NPC.ai[3] = 1f;
                    NPC.netUpdate = true;
                }
            }
            if (NPC.type == 368)
            {
                NPC.homeless = true;
                if (!Main.dayTime)
                {
                    if (!NPC.shimmering)
                    {
                        NPC.UpdateHomeTileState(NPC.homeless, (int)(NPC.Center.X / 16f), (int)(NPC.position.Y + (float)NPC.height + 2f) / 16);
                    }
                    if (!flag23 && NPC.ai[0] == 0f)
                    {
                        NPC.ai[0] = 1f;
                        NPC.ai[1] = 200f;
                    }
                    flag = false;
                }
            }
            if (NPC.type == 369 && NPC.homeless && NPC.wet)
            {
                if (NPC.Center.X / 16f < 380f || NPC.Center.X / 16f > (float)(Main.maxTilesX - 380))
                {
                    NPC.UpdateHomeTileState(NPC.homeless, Main.spawnTileX, Main.spawnTileY);
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 200f;
                }
                if (NPC.position.X / 16f < 300f)
                {
                    NPC.direction = 1;
                }
                else if (NPC.position.X / 16f > (float)(Main.maxTilesX - 300))
                {
                    NPC.direction = -1;
                }
            }
            if (!WorldGen.InWorld(num86, num97) || (Main.netMode == 1 && !Main.sectionManager.TileLoaded(num86, num97)))
            {
                return;
            }
            if (!NPC.homeless && Main.netMode != 1 && NPC.townNPC && (flag || (NPC.type == 37 && Main.tileDungeon[Main.tile[num86, num97].type])) && !NPC.AI_007_TownEntities_IsInAGoodRestingSpot(num86, num97, floorX, floorY))
            {
                bool flag27 = true;
                for (int k = 0; k < 2; k++)
                {
                    if (!flag27)
                    {
                        break;
                    }
                    Rectangle rectangle = new Rectangle((int)(NPC.position.X + (float)(NPC.width / 2) - (float)(NPC.sWidth / 2) - (float)NPC.safeRangeX), (int)(NPC.position.Y + (float)(NPC.height / 2) - (float)(NPC.sHeight / 2) - (float)NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                    if (k == 1)
                    {
                        rectangle = new Rectangle(floorX * 16 + 8 - NPC.sWidth / 2 - NPC.safeRangeX, floorY * 16 + 8 - NPC.sHeight / 2 - NPC.safeRangeY, NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                    }
                    for (int l = 0; l < 255; l++)
                    {
                        if (Main.player[l].active && new Rectangle((int)Main.player[l].position.X, (int)Main.player[l].position.Y, Main.player[l].width, Main.player[l].height).Intersects(rectangle))
                        {
                            flag27 = false;
                            break;
                        }
                    }
                }
                if (flag27)
                {
                    NPC.AI_007_TownEntities_TeleportToHome(floorX, floorY);
                }
            }
            bool flag28 = NPC.type == 300 || NPC.type == 447 || NPC.type == 610;
            bool flag29 = NPC.type == 616 || NPC.type == 617 || NPC.type == 625;
            bool flag30 = NPC.type == 361 || NPC.type == 445 || NPC.type == 687;
            bool flag31 = NPCID.Sets.IsTownSlime[NPC.type];
            _ = NPCID.Sets.IsTownPet[NPC.type];
            bool flag32 = flag29 || flag30;
            bool flag2 = flag29 || flag30;
            bool flag3 = flag31;
            bool flag4 = flag31;
            float num108 = 200f;
            if (NPCID.Sets.DangerDetectRange[NPC.type] != -1)
            {
                num108 = NPCID.Sets.DangerDetectRange[NPC.type];
            }
            bool flag5 = false;
            bool flag6 = false;
            float num119 = -1f;
            float num2 = -1f;
            int num13 = 0;
            int num24 = -1;
            int num35 = -1;
            bool keepwalking4;
            if (!flag29 && Main.netMode != 1 && !flag23)
            {
                for (int m = 0; m < 200; m++)
                {
                    if (!Main.npc[m].active || Main.npc[m].friendly || Main.npc[m].damage <= 0 || !(Main.npc[m].Distance(NPC.Center) < num108) || (NPC.type == 453 && NPCID.Sets.Skeletons[Main.npc[m].type]) || (!Main.npc[m].noTileCollide && !Collision.CanHit(NPC.Center, 0, 0, Main.npc[m].Center, 0, 0)) || !NPCLoader.CanHitNPC(Main.npc[m], NPC))
                    {
                        continue;
                    }
                    bool flag7 = Main.npc[m].CanBeChasedBy(NPC);
                    flag5 = true;
                    float num36 = Main.npc[m].Center.X - NPC.Center.X;
                    if (NPC.type == 614)
                    {
                        if (num36 < 0f && (num119 == -1f || num36 > num119))
                        {
                            num2 = num36;
                            num35 = m;
                        }
                        if (num36 > 0f && (num2 == -1f || num36 < num2))
                        {
                            num119 = num36;
                            num24 = m;
                        }
                        continue;
                    }
                    if (num36 < 0f && (num119 == -1f || num36 > num119))
                    {
                        num119 = num36;
                        if (flag7)
                        {
                            num24 = m;
                        }
                    }
                    if (num36 > 0f && (num2 == -1f || num36 < num2))
                    {
                        num2 = num36;
                        if (flag7)
                        {
                            num35 = m;
                        }
                    }
                }
                if (flag5)
                {
                    num13 = ((num119 == -1f) ? 1 : ((num2 != -1f) ? (num2 < 0f - num119).ToDirectionInt() : (-1)));
                    float num37 = 0f;
                    if (num119 != -1f)
                    {
                        num37 = 0f - num119;
                    }
                    if (num37 == 0f || (num2 < num37 && num2 > 0f))
                    {
                        num37 = num2;
                    }
                    if (NPC.ai[0] == 8f)
                    {
                        if (NPC.direction == -num13)
                        {
                            NPC.ai[0] = 1f;
                            NPC.ai[1] = 300 + Main.rand.Next(300);
                            NPC.ai[2] = 0f;
                            NPC.localAI[3] = 0f;
                            NPC.netUpdate = true;
                        }
                    }
                    else if (NPC.ai[0] != 10f && NPC.ai[0] != 12f && NPC.ai[0] != 13f && NPC.ai[0] != 14f && NPC.ai[0] != 15f)
                    {
                        if (NPCID.Sets.PrettySafe[NPC.type] != -1 && (float)NPCID.Sets.PrettySafe[NPC.type] < num37)
                        {
                            flag5 = false;
                            flag6 = NPCID.Sets.AttackType[NPC.type] > -1;
                        }
                        else if (NPC.ai[0] != 1f)
                        {
                            int tileX = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)(15 * NPC.direction)) / 16f);
                            int tileY = (int)((NPC.position.Y + (float)NPC.height - 16f) / 16f);
                            bool currentlyDrowning = NPC.wet && !flag32;
                            NPC.AI_007_TownEntities_GetWalkPrediction(num86, floorX, flag32, currentlyDrowning, tileX, tileY, out keepwalking4, out var avoidFalling);
                            if (!avoidFalling)
                            {
                                if (NPC.ai[0] == 3f || NPC.ai[0] == 4f || NPC.ai[0] == 16f || NPC.ai[0] == 17f)
                                {
                                    NPC nPC = Main.npc[(int)NPC.ai[2]];
                                    if (nPC.active)
                                    {
                                        nPC.ai[0] = 1f;
                                        nPC.ai[1] = 120 + Main.rand.Next(120);
                                        nPC.ai[2] = 0f;
                                        nPC.localAI[3] = 0f;
                                        nPC.direction = -num13;
                                        nPC.netUpdate = true;
                                    }
                                }
                                NPC.ai[0] = 1f;
                                NPC.ai[1] = 120 + Main.rand.Next(120);
                                NPC.ai[2] = 0f;
                                NPC.localAI[3] = 0f;
                                NPC.direction = -num13;
                                NPC.netUpdate = true;
                            }
                        }
                        else if (NPC.ai[0] == 1f && NPC.direction != -num13)
                        {
                            NPC.direction = -num13;
                            NPC.netUpdate = true;
                        }
                    }
                }
            }
            if (NPC.ai[0] == 0f)
            {
                if (NPC.localAI[3] > 0f)
                {
                    NPC.localAI[3] -= 1f;
                }
                int num38 = 120;
                if (NPC.type == 638)
                {
                    num38 = 60;
                }
                if ((flag30 || flag31) && NPC.wet)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 200 + Main.rand.Next(500, 700);
                    NPC.ai[2] = 0f;
                    NPC.localAI[3] = 0f;
                    NPC.netUpdate = true;
                }
                else if (flag && !flag23 && !NPCID.Sets.TownCritter[NPC.type])
                {
                    if (Main.netMode != 1)
                    {
                        if (num86 == floorX && num97 == floorY)
                        {
                            if (NPC.velocity.X != 0f)
                            {
                                NPC.netUpdate = true;
                            }
                            if (NPC.velocity.X > 0.1f)
                            {
                                NPC.velocity.X -= 0.1f;
                            }
                            else if (NPC.velocity.X < -0.1f)
                            {
                                NPC.velocity.X += 0.1f;
                            }
                            else
                            {
                                NPC.velocity.X = 0f;
                                NPC.AI_007_TryForcingSitting(floorX, floorY);
                            }
                            if (NPCID.Sets.IsTownPet[NPC.type])
                            {
                                NPC.AI_007_AttemptToPlayIdleAnimationsForPets(num38 * 4);
                            }
                        }
                        else
                        {
                            if (num86 > floorX)
                            {
                                NPC.direction = -1;
                            }
                            else
                            {
                                NPC.direction = 1;
                            }
                            NPC.ai[0] = 1f;
                            NPC.ai[1] = 200 + Main.rand.Next(200);
                            NPC.ai[2] = 0f;
                            NPC.localAI[3] = 0f;
                            NPC.netUpdate = true;
                        }
                    }
                }
                else
                {
                    if (flag28)
                    {
                        NPC.velocity.X *= 0.5f;
                    }
                    if (NPC.velocity.X > 0.1f)
                    {
                        NPC.velocity.X -= 0.1f;
                    }
                    else if (NPC.velocity.X < -0.1f)
                    {
                        NPC.velocity.X += 0.1f;
                    }
                    else
                    {
                        NPC.velocity.X = 0f;
                    }
                    if (Main.netMode != 1)
                    {
                        if (!flag23 && NPCID.Sets.IsTownPet[NPC.type] && NPC.ai[1] >= 100f && NPC.ai[1] <= 150f)
                        {
                            NPC.AI_007_AttemptToPlayIdleAnimationsForPets(num38);
                        }
                        if (NPC.ai[1] > 0f)
                        {
                            NPC.ai[1] -= 1f;
                        }
                        bool flag8 = true;
                        int tileX2 = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)(15 * NPC.direction)) / 16f);
                        int tileY2 = (int)((NPC.position.Y + (float)NPC.height - 16f) / 16f);
                        bool currentlyDrowning2 = NPC.wet && !flag32;
                        NPC.AI_007_TownEntities_GetWalkPrediction(num86, floorX, flag32, currentlyDrowning2, tileX2, tileY2, out keepwalking4, out var avoidFalling2);
                        if (NPC.wet && !flag32)
                        {
                            bool currentlyDrowning3 = Collision.DrownCollision(NPC.position, NPC.width, NPC.height, 1f, includeSlopes: true);
                            if (NPC.AI_007_TownEntities_CheckIfWillDrown(currentlyDrowning3))
                            {
                                NPC.ai[0] = 1f;
                                NPC.ai[1] = 200 + Main.rand.Next(300);
                                NPC.ai[2] = 0f;
                                if (NPCID.Sets.TownCritter[NPC.type])
                                {
                                    NPC.ai[1] += Main.rand.Next(200, 400);
                                }
                                NPC.localAI[3] = 0f;
                                NPC.netUpdate = true;
                            }
                        }
                        if (avoidFalling2)
                        {
                            flag8 = false;
                        }
                        if (NPC.ai[1] <= 0f)
                        {
                            if (flag8 && !avoidFalling2)
                            {
                                NPC.ai[0] = 1f;
                                NPC.ai[1] = 200 + Main.rand.Next(300);
                                NPC.ai[2] = 0f;
                                if (NPCID.Sets.TownCritter[NPC.type])
                                {
                                    NPC.ai[1] += Main.rand.Next(200, 400);
                                }
                                NPC.localAI[3] = 0f;
                                NPC.netUpdate = true;
                            }
                            else
                            {
                                NPC.direction *= -1;
                                NPC.ai[1] = 60 + Main.rand.Next(120);
                                NPC.netUpdate = true;
                            }
                        }
                    }
                }
                if (Main.netMode != 1 && (!flag || NPC.AI_007_TownEntities_IsInAGoodRestingSpot(num86, num97, floorX, floorY)))
                {
                    if (num86 < floorX - 25 || num86 > floorX + 25)
                    {
                        if (NPC.localAI[3] == 0f)
                        {
                            if (num86 < floorX - 50 && NPC.direction == -1)
                            {
                                NPC.direction = 1;
                                NPC.netUpdate = true;
                            }
                            else if (num86 > floorX + 50 && NPC.direction == 1)
                            {
                                NPC.direction = -1;
                                NPC.netUpdate = true;
                            }
                        }
                    }
                    else if (Main.rand.Next(80) == 0 && NPC.localAI[3] == 0f)
                    {
                        NPC.localAI[3] = 200f;
                        NPC.direction *= -1;
                        NPC.netUpdate = true;
                    }
                }
            }
            else if (NPC.ai[0] == 1f)
            {
                if (Main.netMode != 1 && flag && NPC.AI_007_TownEntities_IsInAGoodRestingSpot(num86, num97, floorX, floorY) && !NPCID.Sets.TownCritter[NPC.type])
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 200 + Main.rand.Next(200);
                    NPC.localAI[3] = 60f;
                    NPC.netUpdate = true;
                }
                else
                {
                    bool flag9 = !flag32 && Collision.DrownCollision(NPC.position, NPC.width, NPC.height, 1f, includeSlopes: true);
                    if (!flag9)
                    {
                        if (Main.netMode != 1 && !NPC.homeless && !Main.tileDungeon[Main.tile[num86, num97].type] && (num86 < floorX - 35 || num86 > floorX + 35))
                        {
                            if (NPC.position.X < (float)(floorX * 16) && NPC.direction == -1)
                            {
                                NPC.ai[1] -= 5f;
                            }
                            else if (NPC.position.X > (float)(floorX * 16) && NPC.direction == 1)
                            {
                                NPC.ai[1] -= 5f;
                            }
                        }
                        NPC.ai[1] -= 1f;
                    }
                    if (NPC.ai[1] <= 0f)
                    {
                        NPC.ai[0] = 0f;
                        NPC.ai[1] = 300 + Main.rand.Next(300);
                        NPC.ai[2] = 0f;
                        if (NPCID.Sets.TownCritter[NPC.type])
                        {
                            NPC.ai[1] -= Main.rand.Next(100);
                        }
                        else
                        {
                            NPC.ai[1] += Main.rand.Next(900);
                        }
                        NPC.localAI[3] = 60f;
                        NPC.netUpdate = true;
                    }
                    if (NPC.closeDoor && ((NPC.position.X + (float)(NPC.width / 2)) / 16f > (float)(NPC.doorX + 2) || (NPC.position.X + (float)(NPC.width / 2)) / 16f < (float)(NPC.doorX - 2)))
                    {
                        Tile tileSafely = Framing.GetTileSafely(NPC.doorX, NPC.doorY);
                        if (TileLoader.CloseDoorID(tileSafely) >= 0)
                        {
                            if (WorldGen.CloseDoor(NPC.doorX, NPC.doorY))
                            {
                                NPC.closeDoor = false;
                                NetMessage.SendData(19, -1, -1, null, 1, NPC.doorX, NPC.doorY, NPC.direction);
                            }
                            if ((NPC.position.X + (float)(NPC.width / 2)) / 16f > (float)(NPC.doorX + 4) || (NPC.position.X + (float)(NPC.width / 2)) / 16f < (float)(NPC.doorX - 4) || (NPC.position.Y + (float)(NPC.height / 2)) / 16f > (float)(NPC.doorY + 4) || (NPC.position.Y + (float)(NPC.height / 2)) / 16f < (float)(NPC.doorY - 4))
                            {
                                NPC.closeDoor = false;
                            }
                        }
                        else if (tileSafely.type == 389)
                        {
                            if (WorldGen.ShiftTallGate(NPC.doorX, NPC.doorY, closing: true))
                            {
                                NPC.closeDoor = false;
                                NetMessage.SendData(19, -1, -1, null, 5, NPC.doorX, NPC.doorY);
                            }
                            if ((NPC.position.X + (float)(NPC.width / 2)) / 16f > (float)(NPC.doorX + 4) || (NPC.position.X + (float)(NPC.width / 2)) / 16f < (float)(NPC.doorX - 4) || (NPC.position.Y + (float)(NPC.height / 2)) / 16f > (float)(NPC.doorY + 4) || (NPC.position.Y + (float)(NPC.height / 2)) / 16f < (float)(NPC.doorY - 4))
                            {
                                NPC.closeDoor = false;
                            }
                        }
                        else
                        {
                            NPC.closeDoor = false;
                        }
                    }
                    float num39 = 1f;
                    float num40 = 0.07f;
                    if (NPC.type == 614 && flag5)
                    {
                        num39 = 1.5f;
                        num40 = 0.1f;
                    }
                    else if (NPC.type == 299 || NPC.type == 539 || NPC.type == 538 || (NPC.type >= 639 && NPC.type <= 645))
                    {
                        num39 = 1.5f;
                    }
                    else if (flag29)
                    {
                        if (NPC.wet)
                        {
                            num40 = 1f;
                            num39 = 2f;
                        }
                        else
                        {
                            num40 = 0.07f;
                            num39 = 0.5f;
                        }
                    }
                    if (NPC.type == 625)
                    {
                        if (NPC.wet)
                        {
                            num40 = 1f;
                            num39 = 2.5f;
                        }
                        else
                        {
                            num40 = 0.07f;
                            num39 = 0.2f;
                        }
                    }
                    if (flag28)
                    {
                        num39 = 2f;
                        num40 = 1f;
                    }
                    if (NPC.friendly && (flag5 || flag9))
                    {
                        num39 = 1.5f;
                        float num41 = 1f - (float)NPC.life / (float)NPC.lifeMax;
                        num39 += num41 * 0.9f;
                        num40 = 0.1f;
                    }
                    if (flag3 && NPC.wet)
                    {
                        num39 = 2f;
                        num40 = 0.2f;
                    }
                    if (flag30 && NPC.wet)
                    {
                        if (Math.Abs(NPC.velocity.X) < 0.05f && Math.Abs(NPC.velocity.Y) < 0.05f)
                        {
                            NPC.velocity.X += num39 * 10f * (float)NPC.direction;
                        }
                        else
                        {
                            NPC.velocity.X *= 0.9f;
                        }
                    }
                    else if (NPC.velocity.X < 0f - num39 || NPC.velocity.X > num39)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.8f;
                        }
                    }
                    else if (NPC.velocity.X < num39 && NPC.direction == 1)
                    {
                        NPC.velocity.X += num40;
                        if (NPC.velocity.X > num39)
                        {
                            NPC.velocity.X = num39;
                        }
                    }
                    else if (NPC.velocity.X > 0f - num39 && NPC.direction == -1)
                    {
                        NPC.velocity.X -= num40;
                        if (NPC.velocity.X > num39)
                        {
                            NPC.velocity.X = num39;
                        }
                    }
                    bool flag10 = true;
                    if ((float)(NPC.homeTileY * 16 - 32) > NPC.position.Y)
                    {
                        flag10 = false;
                    }
                    if (!flag10 && NPC.velocity.Y == 0f)
                    {
                        Collision.StepDown(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
                    }
                    if (NPC.velocity.Y >= 0f)
                    {
                        Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY, 1, flag10, 1);
                    }
                    if (NPC.velocity.Y == 0f)
                    {
                        int num43 = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)(15 * NPC.direction)) / 16f);
                        int num44 = (int)((NPC.position.Y + (float)NPC.height - 16f) / 16f);
                        int num45 = 180;
                        NPC.AI_007_TownEntities_GetWalkPrediction(num86, floorX, flag32, flag9, num43, num44, out var keepwalking3, out var avoidFalling3);
                        bool flag11 = false;
                        bool flag13 = false;
                        if (NPC.wet && !flag32 && NPC.townNPC && (flag13 = NPC.AI_007_TownEntities_CheckIfWillDrown(flag9)) && NPC.localAI[3] <= 0f)
                        {
                            avoidFalling3 = true;
                            NPC.localAI[3] = num45;
                            int num46 = 0;
                            for (int n = 0; n <= 10 && Framing.GetTileSafely(num43 - NPC.direction, num44 - n).liquid != 0; n++)
                            {
                                num46++;
                            }
                            float num47 = 0.3f;
                            float num48 = (float)Math.Sqrt((float)(num46 * 16 + 16) * 2f * num47);
                            if (num48 > 26f)
                            {
                                num48 = 26f;
                            }
                            NPC.velocity.Y = 0f - num48;
                            NPC.localAI[3] = NPC.position.X;
                            flag11 = true;
                        }
                        if (avoidFalling3 && !flag11)
                        {
                            int num49 = (int)((NPC.position.X + (float)(NPC.width / 2)) / 16f);
                            int num50 = 0;
                            for (int num51 = -1; num51 <= 1; num51++)
                            {
                                Tile tileSafely2 = Framing.GetTileSafely(num49 + num51, num44 + 1);
                                if (tileSafely2.nactive() && Main.tileSolid[tileSafely2.type])
                                {
                                    num50++;
                                }
                            }
                            if (num50 <= 2)
                            {
                                if (NPC.velocity.X != 0f)
                                {
                                    NPC.netUpdate = true;
                                }
                                keepwalking3 = (avoidFalling3 = false);
                                NPC.ai[0] = 0f;
                                NPC.ai[1] = 50 + Main.rand.Next(50);
                                NPC.ai[2] = 0f;
                                NPC.localAI[3] = 40f;
                            }
                        }
                        if (NPC.position.X == NPC.localAI[3] && !flag11)
                        {
                            NPC.direction *= -1;
                            NPC.netUpdate = true;
                            NPC.localAI[3] = num45;
                        }
                        if (flag9 && !flag11)
                        {
                            if (NPC.localAI[3] > (float)num45)
                            {
                                NPC.localAI[3] = num45;
                            }
                            if (NPC.localAI[3] > 0f)
                            {
                                NPC.localAI[3] -= 1f;
                            }
                        }
                        else
                        {
                            NPC.localAI[3] = -1f;
                        }
                        Tile tileSafely3 = Framing.GetTileSafely(num43, num44);
                        Tile tileSafely4 = Framing.GetTileSafely(num43, num44 - 1);
                        Tile tileSafely5 = Framing.GetTileSafely(num43, num44 - 2);
                        bool flag14 = NPC.height / 16 < 3;
                        if ((NPC.townNPC || NPCID.Sets.AllowDoorInteraction[NPC.type]) && tileSafely5.nactive() && (TileLoader.IsClosedDoor(tileSafely5) || tileSafely5.type == 388) && (Main.rand.Next(10) == 0 || flag))
                        {
                            if (Main.netMode != 1)
                            {
                                if (WorldGen.OpenDoor(num43, num44 - 2, NPC.direction))
                                {
                                    NPC.closeDoor = true;
                                    NPC.doorX = num43;
                                    NPC.doorY = num44 - 2;
                                    NetMessage.SendData(19, -1, -1, null, 0, num43, num44 - 2, NPC.direction);
                                    NPC.netUpdate = true;
                                    NPC.ai[1] += 80f;
                                }
                                else if (WorldGen.OpenDoor(num43, num44 - 2, -NPC.direction))
                                {
                                    NPC.closeDoor = true;
                                    NPC.doorX = num43;
                                    NPC.doorY = num44 - 2;
                                    NetMessage.SendData(19, -1, -1, null, 0, num43, num44 - 2, -NPC.direction);
                                    NPC.netUpdate = true;
                                    NPC.ai[1] += 80f;
                                }
                                else if (WorldGen.ShiftTallGate(num43, num44 - 2, closing: false))
                                {
                                    NPC.closeDoor = true;
                                    NPC.doorX = num43;
                                    NPC.doorY = num44 - 2;
                                    NetMessage.SendData(19, -1, -1, null, 4, num43, num44 - 2);
                                    NPC.netUpdate = true;
                                    NPC.ai[1] += 80f;
                                }
                                else
                                {
                                    NPC.direction *= -1;
                                    NPC.netUpdate = true;
                                }
                            }
                        }
                        else
                        {
                            if ((NPC.velocity.X < 0f && NPC.direction == -1) || (NPC.velocity.X > 0f && NPC.direction == 1))
                            {
                                bool flag15 = false;
                                bool flag16 = false;
                                if (tileSafely5.nactive() && Main.tileSolid[tileSafely5.type] && !Main.tileSolidTop[tileSafely5.type] && (!flag14 || (tileSafely4.nactive() && Main.tileSolid[tileSafely4.type] && !Main.tileSolidTop[tileSafely4.type])))
                                {
                                    if (!Collision.SolidTilesVersatile(num43 - NPC.direction * 2, num43 - NPC.direction, num44 - 5, num44 - 1) && !Collision.SolidTiles(num43, num43, num44 - 5, num44 - 3))
                                    {
                                        NPC.velocity.Y = -6f;
                                        NPC.netUpdate = true;
                                    }
                                    else if (flag28)
                                    {
                                        if (WorldGen.SolidTile((int)(NPC.Center.X / 16f) + NPC.direction, (int)(NPC.Center.Y / 16f)))
                                        {
                                            NPC.direction *= -1;
                                            NPC.velocity.X *= 0f;
                                            NPC.netUpdate = true;
                                        }
                                    }
                                    else if (flag5)
                                    {
                                        flag16 = true;
                                        flag15 = true;
                                    }
                                    else if (!flag13)
                                    {
                                        flag15 = true;
                                    }
                                }
                                else if (tileSafely4.nactive() && Main.tileSolid[tileSafely4.type] && !Main.tileSolidTop[tileSafely4.type])
                                {
                                    if (!Collision.SolidTilesVersatile(num43 - NPC.direction * 2, num43 - NPC.direction, num44 - 4, num44 - 1) && !Collision.SolidTiles(num43, num43, num44 - 4, num44 - 2))
                                    {
                                        NPC.velocity.Y = -5f;
                                        NPC.netUpdate = true;
                                    }
                                    else if (flag5)
                                    {
                                        flag16 = true;
                                        flag15 = true;
                                    }
                                    else
                                    {
                                        flag15 = true;
                                    }
                                }
                                else if (NPC.position.Y + (float)NPC.height - (float)(num44 * 16) > 20f && tileSafely3.nactive() && Main.tileSolid[tileSafely3.type] && !tileSafely3.topSlope())
                                {
                                    if (!Collision.SolidTilesVersatile(num43 - NPC.direction * 2, num43, num44 - 3, num44 - 1))
                                    {
                                        NPC.velocity.Y = -4.4f;
                                        NPC.netUpdate = true;
                                    }
                                    else if (flag5)
                                    {
                                        flag16 = true;
                                        flag15 = true;
                                    }
                                    else
                                    {
                                        flag15 = true;
                                    }
                                }
                                else if (avoidFalling3)
                                {
                                    if (!flag13)
                                    {
                                        flag15 = true;
                                    }
                                    if (flag5)
                                    {
                                        flag16 = true;
                                    }
                                }
                                else if (flag4 && !Collision.SolidTilesVersatile(num43 - NPC.direction * 2, num43 - NPC.direction, num44 - 2, num44 - 1))
                                {
                                    NPC.velocity.Y = -5f;
                                    NPC.netUpdate = true;
                                }
                                if (flag16)
                                {
                                    keepwalking3 = false;
                                    NPC.velocity.X = 0f;
                                    NPC.ai[0] = 8f;
                                    NPC.ai[1] = 240f;
                                    NPC.netUpdate = true;
                                }
                                if (flag15)
                                {
                                    NPC.direction *= -1;
                                    NPC.velocity.X *= -1f;
                                    NPC.netUpdate = true;
                                }
                                if (keepwalking3)
                                {
                                    NPC.ai[1] = 90f;
                                    NPC.netUpdate = true;
                                }
                                if (NPC.velocity.Y < 0f)
                                {
                                    NPC.localAI[3] = NPC.position.X;
                                }
                            }
                            if (NPC.velocity.Y < 0f && NPC.wet)
                            {
                                NPC.velocity.Y *= 1.2f;
                            }
                            if (NPC.velocity.Y < 0f && NPCID.Sets.TownCritter[NPC.type] && !flag28)
                            {
                                NPC.velocity.Y *= 1.2f;
                            }
                        }
                    }
                    else if (flag4 && !NPC.wet)
                    {
                        int num52 = (int)(NPC.Center.X / 16f);
                        int num54 = (int)((NPC.position.Y + (float)NPC.height - 16f) / 16f);
                        int num55 = 0;
                        for (int num56 = -1; num56 <= 1; num56++)
                        {
                            for (int num57 = 1; num57 <= 6; num57++)
                            {
                                Tile tileSafely6 = Framing.GetTileSafely(num52 + num56, num54 + num57);
                                if (tileSafely6.liquid > 0 || (tileSafely6.nactive() && Main.tileSolid[tileSafely6.type]))
                                {
                                    num55++;
                                }
                            }
                        }
                        if (num55 <= 2)
                        {
                            if (NPC.velocity.X != 0f)
                            {
                                NPC.netUpdate = true;
                            }
                            NPC.velocity.X *= 0.2f;
                            NPC.ai[0] = 0f;
                            NPC.ai[1] = 50 + Main.rand.Next(50);
                            NPC.ai[2] = 0f;
                            NPC.localAI[3] = 40f;
                        }
                    }
                }
            }
            else if (NPC.ai[0] == 2f || NPC.ai[0] == 11f)
            {
                if (Main.netMode != 1)
                {
                    NPC.localAI[3] -= 1f;
                    if (Main.rand.Next(60) == 0 && NPC.localAI[3] == 0f)
                    {
                        NPC.localAI[3] = 60f;
                        NPC.direction *= -1;
                        NPC.netUpdate = true;
                    }
                }
                NPC.ai[1] -= 1f;
                NPC.velocity.X *= 0.8f;
                if (NPC.ai[1] <= 0f)
                {
                    NPC.localAI[3] = 40f;
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 60 + Main.rand.Next(60);
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 3f || NPC.ai[0] == 4f || NPC.ai[0] == 5f || NPC.ai[0] == 8f || NPC.ai[0] == 9f || NPC.ai[0] == 16f || NPC.ai[0] == 17f || NPC.ai[0] == 20f || NPC.ai[0] == 21f || NPC.ai[0] == 22f || NPC.ai[0] == 23f)
            {
                NPC.velocity.X *= 0.8f;
                NPC.ai[1] -= 1f;
                if (NPC.ai[0] == 8f && NPC.ai[1] < 60f && flag5)
                {
                    NPC.ai[1] = 180f;
                    NPC.netUpdate = true;
                }
                if (NPC.ai[0] == 5f)
                {
                    Point coords = (NPC.Bottom + Vector2.UnitY * -2f).ToTileCoordinates();
                    Tile tile = Main.tile[coords.X, coords.Y];
                    if (!TileID.Sets.CanBeSatOnForNPCs[tile.type])
                    {
                        NPC.ai[1] = 0f;
                    }
                    else
                    {
                        Main.sittingManager.AddNPC(NPC.whoAmI, coords);
                    }
                }
                if (NPC.ai[1] <= 0f)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 60 + Main.rand.Next(60);
                    NPC.ai[2] = 0f;
                    NPC.localAI[3] = 30 + Main.rand.Next(60);
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 6f || NPC.ai[0] == 7f || NPC.ai[0] == 18f || NPC.ai[0] == 19f)
            {
                if (NPC.ai[0] == 18f && (NPC.localAI[3] < 1f || NPC.localAI[3] > 2f))
                {
                    NPC.localAI[3] = 2f;
                }
                NPC.velocity.X *= 0.8f;
                NPC.ai[1] -= 1f;
                int num58 = (int)NPC.ai[2];
                if (num58 < 0 || num58 > 255 || !Main.player[num58].CanBeTalkedTo || Main.player[num58].Distance(NPC.Center) > 200f || !Collision.CanHitLine(NPC.Top, 0, 0, Main.player[num58].Top, 0, 0))
                {
                    NPC.ai[1] = 0f;
                }
                if (NPC.ai[1] > 0f)
                {
                    int num59 = ((NPC.Center.X < Main.player[num58].Center.X) ? 1 : (-1));
                    if (num59 != NPC.direction)
                    {
                        NPC.netUpdate = true;
                    }
                    NPC.direction = num59;
                }
                else
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 60 + Main.rand.Next(60);
                    NPC.ai[2] = 0f;
                    NPC.localAI[3] = 30 + Main.rand.Next(60);
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 10f)
            {
                int num60 = 0;
                int num61 = 0;
                float knockBack = 0f;
                float num62 = 0f;
                int num63 = 0;
                int num65 = 0;
                int maxValue = 0;
                float num66 = 0f;
                float num67 = NPCID.Sets.DangerDetectRange[NPC.type];
                float num68 = 0f;
                if ((float)NPCID.Sets.AttackTime[NPC.type] == NPC.ai[1])
                {
                    NPC.frameCounter = 0.0;
                    NPC.localAI[3] = 0f;
                }
                if (NPC.type == 38)
                {
                    num60 = 30;
                    num62 = 6f;
                    num61 = 20;
                    num63 = 10;
                    num65 = 180;
                    maxValue = 120;
                    num66 = 16f;
                    knockBack = 7f;
                }
                else if (NPC.type == 633)
                {
                    num60 = 880;
                    num62 = 24f;
                    num61 = 15;
                    num63 = 1;
                    num66 = 0f;
                    knockBack = 7f;
                    num65 = 15;
                    maxValue = 10;
                    if (NPC.ShouldBestiaryGirlBeLycantrope())
                    {
                        num60 = 929;
                        num61 = (int)((float)num61 * 1.5f);
                    }
                }
                else if (NPC.type == 550)
                {
                    num60 = 669;
                    num62 = 6f;
                    num61 = 24;
                    num63 = 10;
                    num65 = 120;
                    maxValue = 60;
                    num66 = 16f;
                    knockBack = 9f;
                }
                else if (NPC.type == 588)
                {
                    num60 = 721;
                    num62 = 8f;
                    num61 = 15;
                    num63 = 5;
                    num65 = 20;
                    maxValue = 10;
                    num66 = 16f;
                    knockBack = 9f;
                }
                else if (NPC.type == 208)
                {
                    num60 = 588;
                    num62 = 6f;
                    num61 = 30;
                    num63 = 10;
                    num65 = 60;
                    maxValue = 120;
                    num66 = 16f;
                    knockBack = 6f;
                }
                else if (NPC.type == 17)
                {
                    num60 = 48;
                    num62 = 9f;
                    num61 = 12;
                    num63 = 10;
                    num65 = 60;
                    maxValue = 60;
                    num66 = 16f;
                    knockBack = 1.5f;
                }
                else if (NPC.type == 369)
                {
                    num60 = 520;
                    num62 = 12f;
                    num61 = 10;
                    num63 = 10;
                    num65 = 0;
                    maxValue = 1;
                    num66 = 16f;
                    knockBack = 3f;
                }
                else if (NPC.type == 453)
                {
                    num60 = 21;
                    num62 = 14f;
                    num61 = 14;
                    num63 = 10;
                    num65 = 0;
                    maxValue = 1;
                    num66 = 16f;
                    knockBack = 3f;
                }
                else if (NPC.type == 107)
                {
                    num60 = 24;
                    num62 = 5f;
                    num61 = 15;
                    num63 = 10;
                    num65 = 60;
                    maxValue = 60;
                    num66 = 16f;
                    knockBack = 1f;
                }
                else if (NPC.type == 124)
                {
                    num60 = 582;
                    num62 = 10f;
                    num61 = 11;
                    num63 = 1;
                    num65 = 30;
                    maxValue = 30;
                    knockBack = 3.5f;
                }
                else if (NPC.type == 18)
                {
                    num60 = 583;
                    num62 = 8f;
                    num61 = 8;
                    num63 = 1;
                    num65 = 15;
                    maxValue = 10;
                    knockBack = 2f;
                    num66 = 10f;
                }
                else if (NPC.type == 142)
                {
                    num60 = 589;
                    num62 = 7f;
                    num61 = 22;
                    num63 = 1;
                    num65 = 10;
                    maxValue = 1;
                    knockBack = 2f;
                    num66 = 10f;
                }
                NPCLoader.TownNPCAttackStrength(NPC, ref num61, ref knockBack);
                NPCLoader.TownNPCAttackCooldown(NPC, ref num65, ref maxValue);
                NPCLoader.TownNPCAttackProj(NPC, ref num60, ref num63);
                NPCLoader.TownNPCAttackProjSpeed(NPC, ref num62, ref num66, ref num68);
                if (Main.expertMode)
                {
                    num61 = (int)((float)num61 * Main.GameModeInfo.TownNPCDamageMultiplier);
                }
                num61 = (int)((float)num61 * num42);
                NPC.velocity.X *= 0.8f;
                NPC.ai[1] -= 1f;
                NPC.localAI[3] += 1f;
                if (NPC.localAI[3] == (float)num63 && Main.netMode != 1)
                {
                    Vector2 vec = -Vector2.UnitY;
                    if (num13 == 1 && NPC.spriteDirection == 1 && num35 != -1)
                    {
                        vec = NPC.DirectionTo(Main.npc[num35].Center + new Vector2(0f, (0f - num66) * MathHelper.Clamp(NPC.Distance(Main.npc[num35].Center) / num67, 0f, 1f)));
                    }
                    if (num13 == -1 && NPC.spriteDirection == -1 && num24 != -1)
                    {
                        vec = NPC.DirectionTo(Main.npc[num24].Center + new Vector2(0f, (0f - num66) * MathHelper.Clamp(NPC.Distance(Main.npc[num24].Center) / num67, 0f, 1f)));
                    }
                    if (vec.HasNaNs() || Math.Sign(vec.X) != NPC.spriteDirection)
                    {
                        vec = new Vector2(NPC.spriteDirection, -1f);
                    }
                    vec *= num62;
                    vec += Utils.RandomVector2(Main.rand, 0f - num68, num68);
                    int num69 = 1000;
                    num69 = ((NPC.type == 124) ? Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center.X + (float)(NPC.spriteDirection * 16), NPC.Center.Y - 2f, vec.X, vec.Y, num60, num61, knockBack, Main.myPlayer, 0f, NPC.whoAmI, NPC.townNpcVariationIndex) : ((NPC.type != 142) ? Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center.X + (float)(NPC.spriteDirection * 16), NPC.Center.Y - 2f, vec.X, vec.Y, num60, num61, knockBack, Main.myPlayer) : Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center.X + (float)(NPC.spriteDirection * 16), NPC.Center.Y - 2f, vec.X, vec.Y, num60, num61, knockBack, Main.myPlayer, 0f, Main.rand.Next(5))));
                    Main.projectile[num69].npcProj = true;
                    Main.projectile[num69].noDropItem = true;
                    if (NPC.type == 588)
                    {
                        Main.projectile[num69].timeLeft = 480;
                    }
                }
                if (NPC.ai[1] <= 0f)
                {
                    NPC.ai[0] = ((NPC.localAI[2] == 8f && flag5) ? 8 : 0);
                    NPC.ai[1] = num65 + Main.rand.Next(maxValue);
                    NPC.ai[2] = 0f;
                    NPC.localAI[1] = (NPC.localAI[3] = num65 / 2 + Main.rand.Next(maxValue));
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 12f)
            {
                int num70 = 0;
                int num71 = 0;
                float num72 = 0f;
                int num73 = 0;
                int num74 = 0;
                int maxValue2 = 0;
                float knockBack2 = 0f;
                float num76 = 0f;
                bool flag17 = false;
                float num77 = 0f;
                if ((float)NPCID.Sets.AttackTime[NPC.type] == NPC.ai[1])
                {
                    NPC.frameCounter = 0.0;
                    NPC.localAI[3] = 0f;
                }
                int num78 = -1;
                if (num13 == 1 && NPC.spriteDirection == 1)
                {
                    num78 = num35;
                }
                if (num13 == -1 && NPC.spriteDirection == -1)
                {
                    num78 = num24;
                }
                if (NPC.type == 19)
                {
                    num70 = 14;
                    num72 = 13f;
                    num71 = 24;
                    num74 = 14;
                    maxValue2 = 4;
                    knockBack2 = 3f;
                    num73 = 1;
                    num77 = 0.5f;
                    if ((float)NPCID.Sets.AttackTime[NPC.type] == NPC.ai[1])
                    {
                        NPC.frameCounter = 0.0;
                        NPC.localAI[3] = 0f;
                    }
                    if (Main.hardMode)
                    {
                        num71 = 15;
                        if (NPC.localAI[3] > (float)num73)
                        {
                            num73 = 10;
                            flag17 = true;
                        }
                        if (NPC.localAI[3] > (float)num73)
                        {
                            num73 = 20;
                            flag17 = true;
                        }
                        if (NPC.localAI[3] > (float)num73)
                        {
                            num73 = 30;
                            flag17 = true;
                        }
                    }
                }
                else if (NPC.type == 227)
                {
                    num70 = 587;
                    num72 = 10f;
                    num71 = 8;
                    num74 = 10;
                    maxValue2 = 1;
                    knockBack2 = 1.75f;
                    num73 = 1;
                    num77 = 0.5f;
                    if (NPC.localAI[3] > (float)num73)
                    {
                        num73 = 12;
                        flag17 = true;
                    }
                    if (NPC.localAI[3] > (float)num73)
                    {
                        num73 = 24;
                        flag17 = true;
                    }
                    if (Main.hardMode)
                    {
                        num71 += 2;
                    }
                }
                else if (NPC.type == 368)
                {
                    num70 = 14;
                    num72 = 13f;
                    num71 = 24;
                    num74 = 12;
                    maxValue2 = 5;
                    knockBack2 = 2f;
                    num73 = 1;
                    num77 = 0.2f;
                    if (Main.hardMode)
                    {
                        num71 = 30;
                        num70 = 357;
                    }
                }
                else if (NPC.type == 22)
                {
                    num72 = 10f;
                    num71 = 8;
                    num73 = 1;
                    if (Main.hardMode)
                    {
                        num70 = 2;
                        num74 = 15;
                        maxValue2 = 10;
                        num71 += 6;
                    }
                    else
                    {
                        num70 = 1;
                        num74 = 30;
                        maxValue2 = 20;
                    }
                    knockBack2 = 2.75f;
                    num76 = 4f;
                    num77 = 0.7f;
                }
                else if (NPC.type == 228)
                {
                    num70 = 267;
                    num72 = 14f;
                    num71 = 20;
                    num73 = 1;
                    num74 = 10;
                    maxValue2 = 1;
                    knockBack2 = 3f;
                    num76 = 6f;
                    num77 = 0.4f;
                }
                else if (NPC.type == 178)
                {
                    num70 = 242;
                    num72 = 13f;
                    num71 = ((!Main.hardMode) ? 11 : 15);
                    num74 = 10;
                    maxValue2 = 1;
                    knockBack2 = 2f;
                    num73 = 1;
                    if (NPC.localAI[3] > (float)num73)
                    {
                        num73 = 8;
                        flag17 = true;
                    }
                    if (NPC.localAI[3] > (float)num73)
                    {
                        num73 = 16;
                        flag17 = true;
                    }
                    num77 = 0.3f;
                }
                else if (NPC.type == 229)
                {
                    num70 = 14;
                    num72 = 14f;
                    num71 = 24;
                    num74 = 10;
                    maxValue2 = 1;
                    knockBack2 = 2f;
                    num73 = 1;
                    num77 = 0.7f;
                    if (NPC.localAI[3] > (float)num73)
                    {
                        num73 = 16;
                        flag17 = true;
                    }
                    if (NPC.localAI[3] > (float)num73)
                    {
                        num73 = 24;
                        flag17 = true;
                    }
                    if (NPC.localAI[3] > (float)num73)
                    {
                        num73 = 32;
                        flag17 = true;
                    }
                    if (NPC.localAI[3] > (float)num73)
                    {
                        num73 = 40;
                        flag17 = true;
                    }
                    if (NPC.localAI[3] > (float)num73)
                    {
                        num73 = 48;
                        flag17 = true;
                    }
                    if (NPC.localAI[3] == 0f && num78 != -1 && NPC.Distance(Main.npc[num78].Center) < (float)NPCID.Sets.PrettySafe[NPC.type])
                    {
                        num77 = 0.1f;
                        num70 = 162;
                        num71 = 50;
                        knockBack2 = 10f;
                        num72 = 24f;
                    }
                }
                else if (NPC.type == 209)
                {
                    num70 = Utils.SelectRandom<int>(Main.rand, 134, 133, 135);
                    num73 = 1;
                    switch (num70)
                    {
                        case 135:
                            num72 = 12f;
                            num71 = 30;
                            num74 = 30;
                            maxValue2 = 10;
                            knockBack2 = 7f;
                            num77 = 0.2f;
                            break;
                        case 133:
                            num72 = 10f;
                            num71 = 25;
                            num74 = 10;
                            maxValue2 = 1;
                            knockBack2 = 6f;
                            num77 = 0.2f;
                            break;
                        case 134:
                            num72 = 13f;
                            num71 = 20;
                            num74 = 20;
                            maxValue2 = 10;
                            knockBack2 = 4f;
                            num77 = 0.1f;
                            break;
                    }
                }
                NPCLoader.TownNPCAttackStrength(NPC, ref num71, ref knockBack2);
                NPCLoader.TownNPCAttackCooldown(NPC, ref num74, ref maxValue2);
                NPCLoader.TownNPCAttackProj(NPC, ref num70, ref num73);
                NPCLoader.TownNPCAttackProjSpeed(NPC, ref num72, ref num76, ref num77);
                NPCLoader.TownNPCAttackShoot(NPC, ref flag17);
                if (Main.expertMode)
                {
                    num71 = (int)((float)num71 * Main.GameModeInfo.TownNPCDamageMultiplier);
                }
                num71 = (int)((float)num71 * num42);
                NPC.velocity.X *= 0.8f;
                NPC.ai[1] -= 1f;
                NPC.localAI[3] += 1f;
                if (NPC.localAI[3] == (float)num73 && Main.netMode != 1)
                {
                    Vector2 vec2 = Vector2.Zero;
                    if (num78 != -1)
                    {
                        vec2 = NPC.DirectionTo(Main.npc[num78].Center + new Vector2(0f, 0f - num76));
                    }
                    if (vec2.HasNaNs() || Math.Sign(vec2.X) != NPC.spriteDirection)
                    {
                        vec2 = new Vector2(NPC.spriteDirection, 0f);
                    }
                    vec2 *= num72;
                    vec2 += Utils.RandomVector2(Main.rand, 0f - num77, num77);
                    int num79 = 1000;
                    num79 = ((NPC.type != 227) ? Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center.X + (float)(NPC.spriteDirection * 16), NPC.Center.Y - 2f, vec2.X, vec2.Y, num70, num71, knockBack2, Main.myPlayer) : Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center.X + (float)(NPC.spriteDirection * 16), NPC.Center.Y - 2f, vec2.X, vec2.Y, num70, num71, knockBack2, Main.myPlayer, 0f, (float)Main.rand.Next(12) / 6f));
                    Main.projectile[num79].npcProj = true;
                    Main.projectile[num79].noDropItem = true;
                }
                if (NPC.localAI[3] == (float)num73 && flag17 && num78 != -1)
                {
                    Vector2 vector2 = NPC.DirectionTo(Main.npc[num78].Center);
                    if (vector2.Y <= 0.5f && vector2.Y >= -0.5f)
                    {
                        NPC.ai[2] = vector2.Y;
                    }
                }
                if (NPC.ai[1] <= 0f)
                {
                    NPC.ai[0] = ((NPC.localAI[2] == 8f && flag5) ? 8 : 0);
                    NPC.ai[1] = num74 + Main.rand.Next(maxValue2);
                    NPC.ai[2] = 0f;
                    NPC.localAI[1] = (NPC.localAI[3] = num74 / 2 + Main.rand.Next(maxValue2));
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 13f)
            {
                NPC.velocity.X *= 0.8f;
                if ((float)NPCID.Sets.AttackTime[NPC.type] == NPC.ai[1])
                {
                    NPC.frameCounter = 0.0;
                }
                NPC.ai[1] -= 1f;
                NPC.localAI[3] += 1f;
                if (NPC.localAI[3] == 1f && Main.netMode != 1)
                {
                    Vector2 vec3 = NPC.DirectionTo(Main.npc[(int)NPC.ai[2]].Center + new Vector2(0f, -20f));
                    if (vec3.HasNaNs() || Math.Sign(vec3.X) == -NPC.spriteDirection)
                    {
                        vec3 = new Vector2(NPC.spriteDirection, -1f);
                    }
                    vec3 *= 8f;
                    int num80 = Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center.X + (float)(NPC.spriteDirection * 16), NPC.Center.Y - 2f, vec3.X, vec3.Y, 584, 0, 0f, Main.myPlayer, NPC.ai[2]);
                    Main.projectile[num80].npcProj = true;
                    Main.projectile[num80].noDropItem = true;
                }
                if (NPC.ai[1] <= 0f)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 10 + Main.rand.Next(10);
                    NPC.ai[2] = 0f;
                    NPC.localAI[3] = 5 + Main.rand.Next(10);
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 14f)
            {
                int num81 = 0;
                int num82 = 0;
                float num83 = 0f;
                int num84 = 0;
                int num85 = 0;
                int maxValue3 = 0;
                float knockBack3 = 0f;
                float num87 = 0f;
                float num88 = NPCID.Sets.DangerDetectRange[NPC.type];
                float num89 = 1f;
                float num90 = 0f;
                if ((float)NPCID.Sets.AttackTime[NPC.type] == NPC.ai[1])
                {
                    NPC.frameCounter = 0.0;
                    NPC.localAI[3] = 0f;
                }
                int num91 = -1;
                if (num13 == 1 && NPC.spriteDirection == 1)
                {
                    num91 = num35;
                }
                if (num13 == -1 && NPC.spriteDirection == -1)
                {
                    num91 = num24;
                }
                if (NPC.type == 54)
                {
                    num81 = 585;
                    num83 = 10f;
                    num82 = 16;
                    num84 = 30;
                    num85 = 20;
                    maxValue3 = 15;
                    knockBack3 = 2f;
                    num90 = 1f;
                }
                else if (NPC.type == 108)
                {
                    num81 = 15;
                    num83 = 6f;
                    num82 = 18;
                    num84 = 15;
                    num85 = 15;
                    maxValue3 = 5;
                    knockBack3 = 3f;
                    num87 = 20f;
                }
                else if (NPC.type == 160)
                {
                    num81 = 590;
                    num82 = 40;
                    num84 = 15;
                    num85 = 10;
                    maxValue3 = 1;
                    knockBack3 = 3f;
                    for (; NPC.localAI[3] > (float)num84; num84 += 15)
                    {
                    }
                }
                else if (NPC.type == 663)
                {
                    num81 = 950;
                    num82 = ((!Main.hardMode) ? 15 : 20);
                    num84 = 15;
                    num85 = 0;
                    maxValue3 = 0;
                    knockBack3 = 3f;
                    for (; NPC.localAI[3] > (float)num84; num84 += 10)
                    {
                    }
                }
                else if (NPC.type == 20)
                {
                    num81 = 586;
                    num84 = 24;
                    num85 = 10;
                    maxValue3 = 1;
                    knockBack3 = 3f;
                }
                NPCLoader.TownNPCAttackStrength(NPC, ref num82, ref knockBack3);
                NPCLoader.TownNPCAttackCooldown(NPC, ref num85, ref maxValue3);
                NPCLoader.TownNPCAttackProj(NPC, ref num81, ref num84);
                NPCLoader.TownNPCAttackProjSpeed(NPC, ref num83, ref num87, ref num90);
                NPCLoader.TownNPCAttackMagic(NPC, ref num89);
                if (Main.expertMode)
                {
                    num82 = (int)((float)num82 * Main.GameModeInfo.TownNPCDamageMultiplier);
                }
                num82 = (int)((float)num82 * num42);
                NPC.velocity.X *= 0.8f;
                NPC.ai[1] -= 1f;
                NPC.localAI[3] += 1f;
                if (NPC.localAI[3] == (float)num84 && Main.netMode != 1)
                {
                    Vector2 vec4 = Vector2.Zero;
                    if (num91 != -1)
                    {
                        vec4 = NPC.DirectionTo(Main.npc[num91].Center + new Vector2(0f, (0f - num87) * MathHelper.Clamp(NPC.Distance(Main.npc[num91].Center) / num88, 0f, 1f)));
                    }
                    if (vec4.HasNaNs() || Math.Sign(vec4.X) != NPC.spriteDirection)
                    {
                        vec4 = new Vector2(NPC.spriteDirection, 0f);
                    }
                    vec4 *= num83;
                    vec4 += Utils.RandomVector2(Main.rand, 0f - num90, num90);
                    if (NPC.type == 108)
                    {
                        int num92 = Utils.SelectRandom<int>(Main.rand, 1, 1, 1, 1, 2, 2, 3);
                        for (int num93 = 0; num93 < num92; num93++)
                        {
                            Vector2 vector3 = Utils.RandomVector2(Main.rand, -3.4f, 3.4f);
                            int num94 = Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center.X + (float)(NPC.spriteDirection * 16), NPC.Center.Y - 2f, vec4.X + vector3.X, vec4.Y + vector3.Y, num81, num82, knockBack3, Main.myPlayer, 0f, 0f, NPC.townNpcVariationIndex);
                            Main.projectile[num94].npcProj = true;
                            Main.projectile[num94].noDropItem = true;
                        }
                    }
                    else if (NPC.type == 160)
                    {
                        if (num91 != -1)
                        {
                            Vector2 vector4 = Main.npc[num91].position - Main.npc[num91].Size * 2f + Main.npc[num91].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 5f;
                            int num95 = 10;
                            while (num95 > 0 && WorldGen.SolidTile(Framing.GetTileSafely((int)vector4.X / 16, (int)vector4.Y / 16)))
                            {
                                num95--;
                                vector4 = Main.npc[num91].position - Main.npc[num91].Size * 2f + Main.npc[num91].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 5f;
                            }
                            int num96 = Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), vector4.X, vector4.Y, 0f, 0f, num81, num82, knockBack3, Main.myPlayer, 0f, 0f, NPC.townNpcVariationIndex);
                            Main.projectile[num96].npcProj = true;
                            Main.projectile[num96].noDropItem = true;
                        }
                    }
                    else if (NPC.type == 663)
                    {
                        if (num91 != -1)
                        {
                            Vector2 vector5 = Main.npc[num91].position + Main.npc[num91].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 1f;
                            int num98 = 5;
                            while (num98 > 0 && WorldGen.SolidTile(Framing.GetTileSafely((int)vector5.X / 16, (int)vector5.Y / 16)))
                            {
                                num98--;
                                vector5 = Main.npc[num91].position + Main.npc[num91].Size * Utils.RandomVector2(Main.rand, 0f, 1f) * 1f;
                            }
                            int num99 = Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), vector5.X, vector5.Y, 0f, 0f, num81, num82, knockBack3, Main.myPlayer, 0f, 0f, NPC.townNpcVariationIndex);
                            Main.projectile[num99].npcProj = true;
                            Main.projectile[num99].noDropItem = true;
                        }
                    }
                    else if (NPC.type == 20)
                    {
                        int num100 = Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center.X + (float)(NPC.spriteDirection * 16), NPC.Center.Y - 2f, vec4.X, vec4.Y, num81, num82, knockBack3, Main.myPlayer, 0f, NPC.whoAmI, NPC.townNpcVariationIndex);
                        Main.projectile[num100].npcProj = true;
                        Main.projectile[num100].noDropItem = true;
                    }
                    else
                    {
                        int num101 = Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center.X + (float)(NPC.spriteDirection * 16), NPC.Center.Y - 2f, vec4.X, vec4.Y, num81, num82, knockBack3, Main.myPlayer);
                        Main.projectile[num101].npcProj = true;
                        Main.projectile[num101].noDropItem = true;
                    }
                }
                if (num89 > 0f)
                {
                    Vector3 vector6 = NPC.GetMagicAuraColor().ToVector3() * num89;
                    Lighting.AddLight(NPC.Center, vector6.X, vector6.Y, vector6.Z);
                }
                if (NPC.ai[1] <= 0f)
                {
                    NPC.ai[0] = ((NPC.localAI[2] == 8f && flag5) ? 8 : 0);
                    NPC.ai[1] = num85 + Main.rand.Next(maxValue3);
                    NPC.ai[2] = 0f;
                    NPC.localAI[1] = (NPC.localAI[3] = num85 / 2 + Main.rand.Next(maxValue3));
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[0] == 15f)
            {
                int num102 = 0;
                int maxValue4 = 0;
                if ((float)NPCID.Sets.AttackTime[NPC.type] == NPC.ai[1])
                {
                    NPC.frameCounter = 0.0;
                    NPC.localAI[3] = 0f;
                }
                int num103 = 0;
                float num104 = 0f;
                int num105 = 0;
                int num106 = 0;
                if (num13 == 1)
                {
                    _ = NPC.spriteDirection;
                }
                if (num13 == -1)
                {
                    _ = NPC.spriteDirection;
                }
                if (NPC.type == 207)
                {
                    num103 = 11;
                    num105 = (num106 = 32);
                    num102 = 12;
                    maxValue4 = 6;
                    num104 = 4.25f;
                }
                else if (NPC.type == 441)
                {
                    num103 = 9;
                    num105 = (num106 = 28);
                    num102 = 9;
                    maxValue4 = 3;
                    num104 = 3.5f;
                    if (NPC.GivenName == "Andrew")
                    {
                        num103 *= 2;
                        num104 *= 2f;
                    }
                }
                else if (NPC.type == 353)
                {
                    num103 = 10;
                    num105 = (num106 = 32);
                    num102 = 15;
                    maxValue4 = 8;
                    num104 = 5f;
                }
                else if (NPCID.Sets.IsTownPet[NPC.type])
                {
                    num103 = 10;
                    num105 = (num106 = 32);
                    num102 = 15;
                    maxValue4 = 8;
                    num104 = 3f;
                }
                NPCLoader.TownNPCAttackStrength(NPC, ref num103, ref num104);
                NPCLoader.TownNPCAttackCooldown(NPC, ref num102, ref maxValue4);
                NPCLoader.TownNPCAttackSwing(NPC, ref num105, ref num106);
                if (Main.expertMode)
                {
                    num103 = (int)((float)num103 * Main.GameModeInfo.TownNPCDamageMultiplier);
                }
                num103 = (int)((float)num103 * num42);
                NPC.velocity.X *= 0.8f;
                NPC.ai[1] -= 1f;
                if (Main.netMode != 1)
                {
                    Tuple<Vector2, float> swingStats = NPC.GetSwingStats(NPCID.Sets.AttackTime[NPC.type] * 2, (int)NPC.ai[1], NPC.spriteDirection, num105, num106);
                    Rectangle itemRectangle = new Rectangle((int)swingStats.Item1.X, (int)swingStats.Item1.Y, num105, num106);
                    if (NPC.spriteDirection == -1)
                    {
                        itemRectangle.X -= num105;
                    }
                    itemRectangle.Y -= num106;
                    NPC.TweakSwingStats(NPCID.Sets.AttackTime[NPC.type] * 2, (int)NPC.ai[1], NPC.spriteDirection, ref itemRectangle);
                    int myPlayer = Main.myPlayer;
                    for (int num107 = 0; num107 < 200; num107++)
                    {
                        NPC nPC2 = Main.npc[num107];
                        if (nPC2.active && nPC2.immune[myPlayer] == 0 && !nPC2.dontTakeDamage && !nPC2.friendly && nPC2.damage > 0 && itemRectangle.Intersects(nPC2.Hitbox) && (nPC2.noTileCollide || Collision.CanHit(NPC.position, NPC.width, NPC.height, nPC2.position, nPC2.width, nPC2.height)))
                        {
                            nPC2.StrikeNPCNoInteraction(num103, num104, NPC.spriteDirection);
                            if (Main.netMode != 0)
                            {
                                NetMessage.SendData(28, -1, -1, null, num107, num103, num104, NPC.spriteDirection);
                            }
                            nPC2.netUpdate = true;
                            nPC2.immune[myPlayer] = (int)NPC.ai[1] + 2;
                        }
                    }
                }
                if (NPC.ai[1] <= 0f)
                {
                    bool flag18 = false;
                    if (flag5)
                    {
                        int num109 = -num13;
                        if (!Collision.CanHit(NPC.Center, 0, 0, NPC.Center + Vector2.UnitX * num109 * 32f, 0, 0) || NPC.localAI[2] == 8f)
                        {
                            flag18 = true;
                        }
                        if (flag18)
                        {
                            int num110 = NPCID.Sets.AttackTime[NPC.type];
                            int num111 = ((num13 == 1) ? num35 : num24);
                            int num112 = ((num13 == 1) ? num24 : num35);
                            if (num111 != -1 && !Collision.CanHit(NPC.Center, 0, 0, Main.npc[num111].Center, 0, 0))
                            {
                                num111 = ((num112 == -1 || !Collision.CanHit(NPC.Center, 0, 0, Main.npc[num112].Center, 0, 0)) ? (-1) : num112);
                            }
                            if (num111 != -1)
                            {
                                NPC.ai[0] = 15f;
                                NPC.ai[1] = num110;
                                NPC.ai[2] = 0f;
                                NPC.localAI[3] = 0f;
                                NPC.direction = ((NPC.position.X < Main.npc[num111].position.X) ? 1 : (-1));
                                NPC.netUpdate = true;
                            }
                            else
                            {
                                flag18 = false;
                            }
                        }
                    }
                    if (!flag18)
                    {
                        NPC.ai[0] = ((NPC.localAI[2] == 8f && flag5) ? 8 : 0);
                        NPC.ai[1] = num102 + Main.rand.Next(maxValue4);
                        NPC.ai[2] = 0f;
                        NPC.localAI[1] = (NPC.localAI[3] = num102 / 2 + Main.rand.Next(maxValue4));
                        NPC.netUpdate = true;
                    }
                }
            }
            else if (NPC.ai[0] == 24f)
            {
                NPC.velocity.X *= 0.8f;
                NPC.ai[1] -= 1f;
                NPC.localAI[3] += 1f;
                NPC.direction = 1;
                NPC.spriteDirection = 1;
                Vector3 vector7 = NPC.GetMagicAuraColor().ToVector3();
                Lighting.AddLight(NPC.Center, vector7.X, vector7.Y, vector7.Z);
                if (NPC.ai[1] <= 0f)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 480f;
                    NPC.ai[2] = 0f;
                    NPC.localAI[1] = 480f;
                    NPC.netUpdate = true;
                }
            }
            if (flag3 && NPC.wet)
            {
                int num113 = (int)(NPC.Center.X / 16f);
                int num114 = 5;
                if (NPC.collideX || (num113 < num114 && NPC.direction == -1) || (num113 > Main.maxTilesX - num114 && NPC.direction == 1))
                {
                    NPC.direction *= -1;
                    NPC.velocity.X *= -0.25f;
                    NPC.netUpdate = true;
                }
                NPC.velocity.Y *= 0.9f;
                NPC.velocity.Y -= 0.5f;
                if (NPC.velocity.Y < -15f)
                {
                    NPC.velocity.Y = -15f;
                }
            }
            if (flag2 && NPC.wet)
            {
                if (flag30)
                {
                    NPC.ai[1] = 50f;
                }
                int num115 = (int)(NPC.Center.X / 16f);
                int num116 = 5;
                if (NPC.collideX || (num115 < num116 && NPC.direction == -1) || (num115 > Main.maxTilesX - num116 && NPC.direction == 1))
                {
                    NPC.direction *= -1;
                    NPC.velocity.X *= -0.25f;
                    NPC.netUpdate = true;
                }
                if (Collision.GetWaterLine(NPC.Center.ToTileCoordinates(), out var waterLineHeight))
                {
                    float num117 = NPC.Center.Y + 1f;
                    if (NPC.Center.Y > waterLineHeight)
                    {
                        NPC.velocity.Y -= 0.8f;
                        if (NPC.velocity.Y < -4f)
                        {
                            NPC.velocity.Y = -4f;
                        }
                        if (num117 + NPC.velocity.Y < waterLineHeight)
                        {
                            NPC.velocity.Y = waterLineHeight - num117;
                        }
                    }
                    else
                    {
                        NPC.velocity.Y = MathHelper.Min(NPC.velocity.Y, waterLineHeight - num117);
                    }
                }
                else
                {
                    NPC.velocity.Y -= 0.2f;
                }
            }
            if (Main.netMode != 1 && NPC.isLikeATownNPC && !flag23)
            {
                bool flag19 = NPC.ai[0] < 2f && !flag5 && !NPC.wet;
                bool flag20 = (NPC.ai[0] < 2f || NPC.ai[0] == 8f) && (flag5 || flag6);
                if (NPC.localAI[1] > 0f)
                {
                    NPC.localAI[1] -= 1f;
                }
                if (NPC.localAI[1] > 0f)
                {
                    flag20 = false;
                }
                if (flag20 && NPC.type == 124 && NPC.localAI[0] == 1f)
                {
                    flag20 = false;
                }
                if (flag20 && NPC.type == 20)
                {
                    flag20 = false;
                    for (int num118 = 0; num118 < 200; num118++)
                    {
                        NPC nPC3 = Main.npc[num118];
                        if (nPC3.active && nPC3.townNPC && !(NPC.Distance(nPC3.Center) > 1200f) && nPC3.FindBuffIndex(165) == -1)
                        {
                            flag20 = true;
                            break;
                        }
                    }
                }
                if (NPC.CanTalk && flag19 && NPC.ai[0] == 0f && NPC.velocity.Y == 0f && Main.rand.Next(300) == 0)
                {
                    int num120 = 420;
                    num120 = ((Main.rand.Next(2) != 0) ? (num120 * Main.rand.Next(1, 3)) : (num120 * Main.rand.Next(1, 4)));
                    int num121 = 100;
                    int num122 = 20;
                    for (int num123 = 0; num123 < 200; num123++)
                    {
                        NPC nPC4 = Main.npc[num123];
                        bool flag21 = (nPC4.ai[0] == 1f && nPC4.closeDoor) || (nPC4.ai[0] == 1f && nPC4.ai[1] > 200f) || nPC4.ai[0] > 1f || nPC4.wet;
                        if (nPC4 != NPC && nPC4.active && nPC4.CanBeTalkedTo && !flag21 && nPC4.Distance(NPC.Center) < (float)num121 && nPC4.Distance(NPC.Center) > (float)num122 && Collision.CanHit(NPC.Center, 0, 0, nPC4.Center, 0, 0))
                        {
                            int num124 = (NPC.position.X < nPC4.position.X).ToDirectionInt();
                            NPC.ai[0] = 3f;
                            NPC.ai[1] = num120;
                            NPC.ai[2] = num123;
                            NPC.direction = num124;
                            NPC.netUpdate = true;
                            nPC4.ai[0] = 4f;
                            nPC4.ai[1] = num120;
                            nPC4.ai[2] = NPC.whoAmI;
                            nPC4.direction = -num124;
                            nPC4.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (NPC.CanTalk && flag19 && NPC.ai[0] == 0f && NPC.velocity.Y == 0f && Main.rand.Next(1800) == 0)
                {
                    int num125 = 420;
                    num125 = ((Main.rand.Next(2) != 0) ? (num125 * Main.rand.Next(1, 3)) : (num125 * Main.rand.Next(1, 4)));
                    int num126 = 100;
                    int num127 = 20;
                    for (int num128 = 0; num128 < 200; num128++)
                    {
                        NPC nPC5 = Main.npc[num128];
                        bool flag22 = (nPC5.ai[0] == 1f && nPC5.closeDoor) || (nPC5.ai[0] == 1f && nPC5.ai[1] > 200f) || nPC5.ai[0] > 1f || nPC5.wet;
                        if (nPC5 != NPC && nPC5.active && nPC5.CanBeTalkedTo && !NPCID.Sets.IsTownPet[nPC5.type] && !flag22 && nPC5.Distance(NPC.Center) < (float)num126 && nPC5.Distance(NPC.Center) > (float)num127 && Collision.CanHit(NPC.Center, 0, 0, nPC5.Center, 0, 0))
                        {
                            int num129 = (NPC.position.X < nPC5.position.X).ToDirectionInt();
                            NPC.ai[0] = 16f;
                            NPC.ai[1] = num125;
                            NPC.ai[2] = num128;
                            NPC.localAI[2] = Main.rand.Next(4);
                            NPC.localAI[3] = Main.rand.Next(3 - (int)NPC.localAI[2]);
                            NPC.direction = num129;
                            NPC.netUpdate = true;
                            nPC5.ai[0] = 17f;
                            nPC5.ai[1] = num125;
                            nPC5.ai[2] = NPC.whoAmI;
                            nPC5.localAI[2] = 0f;
                            nPC5.localAI[3] = 0f;
                            nPC5.direction = -num129;
                            nPC5.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (!NPCID.Sets.IsTownPet[NPC.type] && flag19 && NPC.ai[0] == 0f && NPC.velocity.Y == 0f && Main.rand.Next(1200) == 0 && (NPC.type == 208 || (BirthdayParty.PartyIsUp && NPCID.Sets.AttackType[NPC.type] == NPCID.Sets.AttackType[208])))
                {
                    int num3 = 300;
                    int num4 = 150;
                    for (int num5 = 0; num5 < 255; num5++)
                    {
                        Player player = Main.player[num5];
                        if (player.active && !player.dead && player.Distance(NPC.Center) < (float)num4 && Collision.CanHitLine(NPC.Top, 0, 0, player.Top, 0, 0))
                        {
                            int num6 = (NPC.position.X < player.position.X).ToDirectionInt();
                            NPC.ai[0] = 6f;
                            NPC.ai[1] = num3;
                            NPC.ai[2] = num5;
                            NPC.direction = num6;
                            NPC.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (flag19 && NPC.ai[0] == 0f && NPC.velocity.Y == 0f && Main.rand.Next(600) == 0 && NPC.type == 550)
                {
                    int num7 = 300;
                    int num8 = 150;
                    for (int num9 = 0; num9 < 255; num9++)
                    {
                        Player player2 = Main.player[num9];
                        if (player2.active && !player2.dead && player2.Distance(NPC.Center) < (float)num8 && Collision.CanHitLine(NPC.Top, 0, 0, player2.Top, 0, 0))
                        {
                            int num10 = (NPC.position.X < player2.position.X).ToDirectionInt();
                            NPC.ai[0] = 18f;
                            NPC.ai[1] = num7;
                            NPC.ai[2] = num9;
                            NPC.direction = num10;
                            NPC.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (!NPCID.Sets.IsTownPet[NPC.type] && flag19 && NPC.ai[0] == 0f && NPC.velocity.Y == 0f && Main.rand.Next(1800) == 0)
                {
                    NPC.ai[0] = 2f;
                    NPC.ai[1] = 45 * Main.rand.Next(1, 2);
                    NPC.netUpdate = true;
                }
                else if (flag19 && NPC.ai[0] == 0f && NPC.velocity.Y == 0f && Main.rand.Next(600) == 0 && NPC.type == 229 && !flag6)
                {
                    NPC.ai[0] = 11f;
                    NPC.ai[1] = 30 * Main.rand.Next(1, 4);
                    NPC.netUpdate = true;
                }
                else if (flag19 && NPC.ai[0] == 0f && NPC.velocity.Y == 0f && Main.rand.Next(1200) == 0)
                {
                    int num11 = 220;
                    int num12 = 150;
                    for (int num14 = 0; num14 < 255; num14++)
                    {
                        Player player3 = Main.player[num14];
                        if (player3.CanBeTalkedTo && player3.Distance(NPC.Center) < (float)num12 && Collision.CanHitLine(NPC.Top, 0, 0, player3.Top, 0, 0))
                        {
                            int num15 = (NPC.position.X < player3.position.X).ToDirectionInt();
                            NPC.ai[0] = 7f;
                            NPC.ai[1] = num11;
                            NPC.ai[2] = num14;
                            NPC.direction = num15;
                            NPC.netUpdate = true;
                            break;
                        }
                    }
                }
                else if (flag19 && NPC.ai[0] == 1f && NPC.velocity.Y == 0f && num > 0 && Main.rand.Next(num) == 0)
                {
                    Point point = (NPC.Bottom + Vector2.UnitY * -2f).ToTileCoordinates();
                    bool flag24 = WorldGen.InWorld(point.X, point.Y, 1);
                    if (flag24)
                    {
                        for (int num16 = 0; num16 < 200; num16++)
                        {
                            if (Main.npc[num16].active && Main.npc[num16].aiStyle == 7 && Main.npc[num16].townNPC && Main.npc[num16].ai[0] == 5f && (Main.npc[num16].Bottom + Vector2.UnitY * -2f).ToTileCoordinates() == point)
                            {
                                flag24 = false;
                                break;
                            }
                        }
                        for (int num17 = 0; num17 < 255; num17++)
                        {
                            if (Main.player[num17].active && Main.player[num17].sitting.isSitting && Main.player[num17].Center.ToTileCoordinates() == point)
                            {
                                flag24 = false;
                                break;
                            }
                        }
                    }
                    if (flag24)
                    {
                        Tile tile2 = Main.tile[point.X, point.Y];
                        flag24 = TileID.Sets.CanBeSatOnForNPCs[tile2.type];
                        if (flag24 && tile2.type == 15 && tile2.frameY >= 1080 && tile2.frameY <= 1098)
                        {
                            flag24 = false;
                        }
                        if (flag24)
                        {
                            NPC.ai[0] = 5f;
                            NPC.ai[1] = 900 + Main.rand.Next(10800);
                            NPC.SitDown(point, out var targetDirection, out var bottom);
                            NPC.direction = targetDirection;
                            NPC.Bottom = bottom;
                            NPC.velocity = Vector2.Zero;
                            NPC.localAI[3] = 0f;
                            NPC.netUpdate = true;
                        }
                    }
                }
                else if (flag19 && NPC.ai[0] == 1f && NPC.velocity.Y == 0f && Main.rand.Next(600) == 0 && Utils.PlotTileLine(NPC.Top, NPC.Bottom, NPC.width, DelegateMethods.SearchAvoidedByNPCs))
                {
                    Point point2 = (NPC.Center + new Vector2(NPC.direction * 10, 0f)).ToTileCoordinates();
                    bool flag25 = WorldGen.InWorld(point2.X, point2.Y, 1);
                    if (flag25)
                    {
                        Tile tileSafely7 = Framing.GetTileSafely(point2.X, point2.Y);
                        if (!tileSafely7.nactive() || !TileID.Sets.InteractibleByNPCs[tileSafely7.type])
                        {
                            flag25 = false;
                        }
                    }
                    if (flag25)
                    {
                        NPC.ai[0] = 9f;
                        NPC.ai[1] = 40 + Main.rand.Next(90);
                        NPC.velocity = Vector2.Zero;
                        NPC.localAI[3] = 0f;
                        NPC.netUpdate = true;
                    }
                }
                if (Main.netMode != 1 && NPC.ai[0] < 2f && NPC.velocity.Y == 0f && NPC.type == 18 && NPC.breath > 0)
                {
                    int num18 = -1;
                    for (int num19 = 0; num19 < 200; num19++)
                    {
                        NPC nPC6 = Main.npc[num19];
                        if (nPC6.active && nPC6.townNPC && nPC6.life != nPC6.lifeMax && (num18 == -1 || nPC6.lifeMax - nPC6.life > Main.npc[num18].lifeMax - Main.npc[num18].life) && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, nPC6.position, nPC6.width, nPC6.height) && NPC.Distance(nPC6.Center) < 500f)
                        {
                            num18 = num19;
                        }
                    }
                    if (num18 != -1)
                    {
                        NPC.ai[0] = 13f;
                        NPC.ai[1] = 34f;
                        NPC.ai[2] = num18;
                        NPC.localAI[3] = 0f;
                        NPC.direction = ((NPC.position.X < Main.npc[num18].position.X) ? 1 : (-1));
                        NPC.netUpdate = true;
                    }
                }
                if (flag20 && NPC.velocity.Y == 0f && NPCID.Sets.AttackType[NPC.type] == 0 && NPCID.Sets.AttackAverageChance[NPC.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[NPC.type] * 2) == 0)
                {
                    int num20 = NPCID.Sets.AttackTime[NPC.type];
                    int num21 = ((num13 == 1) ? num35 : num24);
                    int num22 = ((num13 == 1) ? num24 : num35);
                    if (num21 != -1 && !Collision.CanHit(NPC.Center, 0, 0, Main.npc[num21].Center, 0, 0))
                    {
                        num21 = ((num22 == -1 || !Collision.CanHit(NPC.Center, 0, 0, Main.npc[num22].Center, 0, 0)) ? (-1) : num22);
                    }
                    bool flag26 = num21 != -1;
                    if (flag26 && NPC.type == 633)
                    {
                        flag26 = Vector2.Distance(NPC.Center, Main.npc[num21].Center) <= 50f;
                    }
                    if (flag26)
                    {
                        NPC.localAI[2] = NPC.ai[0];
                        NPC.ai[0] = 10f;
                        NPC.ai[1] = num20;
                        NPC.ai[2] = 0f;
                        NPC.localAI[3] = 0f;
                        NPC.direction = ((NPC.position.X < Main.npc[num21].position.X) ? 1 : (-1));
                        NPC.netUpdate = true;
                    }
                }
                else if (flag20 && NPC.velocity.Y == 0f && NPCID.Sets.AttackType[NPC.type] == 1 && NPCID.Sets.AttackAverageChance[NPC.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[NPC.type] * 2) == 0)
                {
                    int num23 = NPCID.Sets.AttackTime[NPC.type];
                    int num25 = ((num13 == 1) ? num35 : num24);
                    int num26 = ((num13 == 1) ? num24 : num35);
                    if (num25 != -1 && !Collision.CanHitLine(NPC.Center, 0, 0, Main.npc[num25].Center, 0, 0))
                    {
                        num25 = ((num26 == -1 || !Collision.CanHitLine(NPC.Center, 0, 0, Main.npc[num26].Center, 0, 0)) ? (-1) : num26);
                    }
                    if (num25 != -1)
                    {
                        Vector2 vector8 = NPC.DirectionTo(Main.npc[num25].Center);
                        if (vector8.Y <= 0.5f && vector8.Y >= -0.5f)
                        {
                            NPC.localAI[2] = NPC.ai[0];
                            NPC.ai[0] = 12f;
                            NPC.ai[1] = num23;
                            NPC.ai[2] = vector8.Y;
                            NPC.localAI[3] = 0f;
                            NPC.direction = ((NPC.position.X < Main.npc[num25].position.X) ? 1 : (-1));
                            NPC.netUpdate = true;
                        }
                    }
                }
                if (flag20 && NPC.velocity.Y == 0f && NPCID.Sets.AttackType[NPC.type] == 2 && NPCID.Sets.AttackAverageChance[NPC.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[NPC.type] * 2) == 0)
                {
                    int num27 = NPCID.Sets.AttackTime[NPC.type];
                    int num28 = ((num13 == 1) ? num35 : num24);
                    int num29 = ((num13 == 1) ? num24 : num35);
                    if (num28 != -1 && !Collision.CanHitLine(NPC.Center, 0, 0, Main.npc[num28].Center, 0, 0))
                    {
                        num28 = ((num29 == -1 || !Collision.CanHitLine(NPC.Center, 0, 0, Main.npc[num29].Center, 0, 0)) ? (-1) : num29);
                    }
                    if (num28 != -1)
                    {
                        NPC.localAI[2] = NPC.ai[0];
                        NPC.ai[0] = 14f;
                        NPC.ai[1] = num27;
                        NPC.ai[2] = 0f;
                        NPC.localAI[3] = 0f;
                        NPC.direction = ((NPC.position.X < Main.npc[num28].position.X) ? 1 : (-1));
                        NPC.netUpdate = true;
                    }
                    else if (NPC.type == 20)
                    {
                        NPC.localAI[2] = NPC.ai[0];
                        NPC.ai[0] = 14f;
                        NPC.ai[1] = num27;
                        NPC.ai[2] = 0f;
                        NPC.localAI[3] = 0f;
                        NPC.netUpdate = true;
                    }
                }
                if (flag20 && NPC.velocity.Y == 0f && NPCID.Sets.AttackType[NPC.type] == 3 && NPCID.Sets.AttackAverageChance[NPC.type] > 0 && Main.rand.Next(NPCID.Sets.AttackAverageChance[NPC.type] * 2) == 0)
                {
                    int num30 = NPCID.Sets.AttackTime[NPC.type];
                    int num31 = ((num13 == 1) ? num35 : num24);
                    int num32 = ((num13 == 1) ? num24 : num35);
                    if (num31 != -1 && !Collision.CanHit(NPC.Center, 0, 0, Main.npc[num31].Center, 0, 0))
                    {
                        num31 = ((num32 == -1 || !Collision.CanHit(NPC.Center, 0, 0, Main.npc[num32].Center, 0, 0)) ? (-1) : num32);
                    }
                    if (num31 != -1)
                    {
                        NPC.localAI[2] = NPC.ai[0];
                        NPC.ai[0] = 15f;
                        NPC.ai[1] = num30;
                        NPC.ai[2] = 0f;
                        NPC.localAI[3] = 0f;
                        NPC.direction = ((NPC.position.X < Main.npc[num31].position.X) ? 1 : (-1));
                        NPC.netUpdate = true;
                    }
                }
            }
            if (NPC.type == 681)
            {
                float R = 0f;
                float G = 0f;
                float B = 0f;
                TorchID.TorchColor(23, out R, out G, out B);
                float num33 = 0.35f;
                R *= num33;
                G *= num33;
                B *= num33;
                Lighting.AddLight(NPC.Center, R, G, B);
            }
            if (NPC.type == 683 || NPC.type == 687)
            {
                float num34 = Utils.WrappedLerp(0.75f, 1f, (float)Main.timeForVisualEffects % 120f / 120f);
                Lighting.AddLight(NPC.Center, 0.25f * num34, 0.25f * num34, 0.1f * num34);
            }
        }
        */

    }
}