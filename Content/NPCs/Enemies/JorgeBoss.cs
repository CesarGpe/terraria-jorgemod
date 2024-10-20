using System;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;

namespace eslamio.Content.NPCs.Enemies;

internal class JorgeBossBiome : ModBiome
{
    public override bool IsBiomeActive(Player player)
    {
        return NPC.AnyNPCs(ModContent.NPCType<JorgeBoss>());
    }

    public override void SpecialVisuals(Player player, bool isActive)
    {
        player.ManageSpecialBiomeVisuals("eslamio:SlimeShader", isActive, player.Center);
    }
}

[AutoloadBossHead]
public class JorgeBoss : ModNPC
{
    public override string Texture => $"Terraria/Images/NPC_{NPCID.KingSlime}";
    public override string BossHeadTexture => "eslamio/Content/NPCs/Enemies/JorgeBoss_Head_Boss";

    public override void Load()
    {
        // Register head icon manually
        Mod.AddBossHeadTexture(BossHeadTexture);
    }

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 6;

        NPCID.Sets.MPAllowedEnemies[Type] = true;
        NPCID.Sets.BossBestiaryPriority.Add(Type);

        NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
        {
            PortraitScale = 2f,
            PortraitPositionYOverride = 32f
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
    }

    public override void SetDefaults()
    {
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
        NPC.npcSlots = 3.2f; // Take up open spawn slots, preventing random NPCs from spawning during the fight
        NPC.netAlways = true;

        NPC.aiStyle = NPCAIStyleID.KingSlime;
        AIType = NPCID.KingSlime;
        AnimationType = NPCID.KingSlime;

        if (!Main.dedServ)
        {
            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/jorgeboss");
        }
    }

    public override void AI()
    {
        float num759 = 1f;
        float num760 = 1f;
        bool flag68 = false;
        bool flag79 = false;
        bool flag90 = false;

        float num761 = 10f; // gordo
        num761 -= 1f - (float)NPC.life / (float)NPC.lifeMax;
        num760 *= num761;

        NPC.aiAction = 0;
        if (NPC.ai[3] == 0f && NPC.life > 0)
        {
            NPC.ai[3] = NPC.lifeMax;
        }
        if (NPC.localAI[3] == 0f)
        {
            NPC.localAI[3] = 1f;
            flag68 = true;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[0] = -100f;
                NPC.TargetClosest();
                NPC.netUpdate = true;
            }
        }
        int num762 = 3000;
        if (Main.player[NPC.target].dead || Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > (float)num762)
        {
            NPC.TargetClosest();
            if (Main.player[NPC.target].dead || Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > (float)num762)
            {
                NPC.EncourageDespawn(10);
                if (Main.player[NPC.target].Center.X < NPC.Center.X)
                {
                    NPC.direction = 1;
                }
                else
                {
                    NPC.direction = -1;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[1] != 5f)
                {
                    NPC.netUpdate = true;
                    NPC.ai[2] = 0f;
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 5f;
                    NPC.localAI[1] = Main.maxTilesX * 16;
                    NPC.localAI[2] = Main.maxTilesY * 16;
                }
            }
        }
        if (!Main.player[NPC.target].dead && NPC.timeLeft > 10 && NPC.ai[2] >= 300f && NPC.ai[1] < 5f && NPC.velocity.Y == 0f)
        {
            NPC.ai[2] = 0f;
            NPC.ai[0] = 0f;
            NPC.ai[1] = 5f;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.TargetClosest(faceTarget: false);
                Point point10 = NPC.Center.ToTileCoordinates();
                Point point11 = Main.player[NPC.target].Center.ToTileCoordinates();
                Vector2 vector224 = Main.player[NPC.target].Center - NPC.Center;
                int num764 = 10;
                int num765 = 0;
                int num766 = 7;
                int num767 = 0;
                bool flag101 = false;
                if (NPC.localAI[0] >= 360f || vector224.Length() > 2000f)
                {
                    if (NPC.localAI[0] >= 360f)
                    {
                        NPC.localAI[0] = 360f;
                    }
                    flag101 = true;
                    num767 = 100;
                }
                while (!flag101 && num767 < 100)
                {
                    num767++;
                    int num768 = Main.rand.Next(point11.X - num764, point11.X + num764 + 1);
                    int num769 = Main.rand.Next(point11.Y - num764, point11.Y + 1);
                    if ((num769 >= point11.Y - num766 && num769 <= point11.Y + num766 && num768 >= point11.X - num766 && num768 <= point11.X + num766) || (num769 >= point10.Y - num765 && num769 <= point10.Y + num765 && num768 >= point10.X - num765 && num768 <= point10.X + num765) || Main.tile[num768, num769].HasUnactuatedTile)
                    {
                        continue;
                    }
                    int num770 = num769;
                    int num771 = 0;
                    if (Main.tile[num768, num770].HasUnactuatedTile && Main.tileSolid[Main.tile[num768, num770].TileType] && !Main.tileSolidTop[Main.tile[num768, num770].TileType])
                    {
                        num771 = 1;
                    }
                    else
                    {
                        for (; num771 < 150 && num770 + num771 < Main.maxTilesY; num771++)
                        {
                            int num772 = num770 + num771;
                            if (Main.tile[num768, num772].HasUnactuatedTile && Main.tileSolid[Main.tile[num768, num772].TileType] && !Main.tileSolidTop[Main.tile[num768, num772].TileType])
                            {
                                num771--;
                                break;
                            }
                        }
                    }
                    num769 += num771;
                    bool flag2 = true;
                    if (flag2 && Main.tile[num768, num769].LiquidType == LiquidID.Lava)
                    {
                        flag2 = false;
                    }
                    if (flag2 && !Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
                    {
                        flag2 = false;
                    }
                    if (flag2)
                    {
                        NPC.localAI[1] = num768 * 16 + 8;
                        NPC.localAI[2] = num769 * 16 + 16;
                        flag101 = true;
                        break;
                    }
                }
                if (num767 >= 100)
                {
                    Vector2 bottom = Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].Bottom;
                    NPC.localAI[1] = bottom.X;
                    NPC.localAI[2] = bottom.Y;
                }
            }
        }
        if (!Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0) || Math.Abs(NPC.Top.Y - Main.player[NPC.target].Bottom.Y) > 160f)
        {
            NPC.ai[2]++;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.localAI[0]++;
            }
        }
        else if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            NPC.localAI[0]--;
            if (NPC.localAI[0] < 0f)
            {
                NPC.localAI[0] = 0f;
            }
        }
        if (NPC.timeLeft < 10 && (NPC.ai[0] != 0f || NPC.ai[1] != 0f))
        {
            NPC.ai[0] = 0f;
            NPC.ai[1] = 0f;
            NPC.netUpdate = true;
            flag79 = false;
        }
        Dust dust34;
        Dust dust87;
        if (NPC.ai[1] == 5f)
        {
            flag79 = true;
            NPC.aiAction = 1;
            NPC.ai[0]++;
            num759 = MathHelper.Clamp((60f - NPC.ai[0]) / 60f, 0f, 1f);
            num759 = 0.5f + num759 * 0.5f;
            if (NPC.ai[0] >= 60f)
            {
                flag90 = true;
            }
            if (NPC.ai[0] == 60f)
            {
                //Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(-40f, -NPC.height / 2), NPC.velocity, 734);
            }
            if (NPC.ai[0] >= 60f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.Bottom = new Vector2(NPC.localAI[1], NPC.localAI[2]);
                NPC.ai[1] = 6f;
                NPC.ai[0] = 0f;
                NPC.netUpdate = true;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient && NPC.ai[0] >= 120f)
            {
                NPC.ai[1] = 6f;
                NPC.ai[0] = 0f;
            }
            if (!flag90)
            {
                for (int num773 = 0; num773 < 10; num773++)
                {
                    int num775 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, DustID.TintableDust, NPC.velocity.X, NPC.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                    Main.dust[num775].noGravity = true;
                    dust34 = Main.dust[num775];
                    dust87 = dust34;
                    dust87.velocity *= 0.5f;
                }
            }
        }
        else if (NPC.ai[1] == 6f)
        {
            flag79 = true;
            NPC.aiAction = 0;
            NPC.ai[0]++;
            num759 = MathHelper.Clamp(NPC.ai[0] / 30f, 0f, 1f);
            num759 = 0.5f + num759 * 0.5f;
            if (NPC.ai[0] >= 30f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[1] = 0f;
                NPC.ai[0] = 0f;
                NPC.netUpdate = true;
                NPC.TargetClosest();
            }
            if (Main.netMode == NetmodeID.MultiplayerClient && NPC.ai[0] >= 60f)
            {
                NPC.ai[1] = 0f;
                NPC.ai[0] = 0f;
                NPC.TargetClosest();
            }
            for (int num776 = 0; num776 < 10; num776++)
            {
                int num777 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, DustID.TintableDust, NPC.velocity.X, NPC.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                Main.dust[num777].noGravity = true;
                dust34 = Main.dust[num777];
                dust87 = dust34;
                dust87.velocity *= 2f;
            }
        }
        NPC.dontTakeDamage = (NPC.hide = flag90);
        if (NPC.velocity.Y == 0f)
        {
            NPC.velocity.X *= 0.8f;
            if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
            {
                NPC.velocity.X = 0f;
            }
            if (!flag79)
            {
                NPC.ai[0] += 2f;
                if ((double)NPC.life < (double)NPC.lifeMax * 0.8)
                {
                    NPC.ai[0] += 1f;
                }
                if ((double)NPC.life < (double)NPC.lifeMax * 0.6)
                {
                    NPC.ai[0] += 1f;
                }
                if ((double)NPC.life < (double)NPC.lifeMax * 0.4)
                {
                    NPC.ai[0] += 2f;
                }
                if ((double)NPC.life < (double)NPC.lifeMax * 0.2)
                {
                    NPC.ai[0] += 3f;
                }
                if ((double)NPC.life < (double)NPC.lifeMax * 0.1)
                {
                    NPC.ai[0] += 4f;
                }
                if (NPC.ai[0] >= 0f)
                {
                    NPC.netUpdate = true;
                    NPC.TargetClosest();
                    if (NPC.ai[1] == 3f)
                    {
                        NPC.velocity.Y = -13f;
                        NPC.velocity.X += 3.5f * (float)NPC.direction;
                        NPC.ai[0] = -200f;
                        NPC.ai[1] = 0f;
                    }
                    else if (NPC.ai[1] == 2f)
                    {
                        NPC.velocity.Y = -6f;
                        NPC.velocity.X += 4.5f * (float)NPC.direction;
                        NPC.ai[0] = -120f;
                        NPC.ai[1] += 1f;
                    }
                    else
                    {
                        NPC.velocity.Y = -8f;
                        NPC.velocity.X += 4f * (float)NPC.direction;
                        NPC.ai[0] = -120f;
                        NPC.ai[1] += 1f;
                    }
                }
                else if (NPC.ai[0] >= -30f)
                {
                    NPC.aiAction = 1;
                }
            }
        }
        else if (NPC.target < 255)
        {
            float num778 = 4f; // gordo
            if ((NPC.direction == 1 && NPC.velocity.X < num778) || (NPC.direction == -1 && NPC.velocity.X > 0f - num778))
            {
                if ((NPC.direction == -1 && (double)NPC.velocity.X < 0.1) || (NPC.direction == 1 && (double)NPC.velocity.X > -0.1))
                {
                    NPC.velocity.X += 0.2f * (float)NPC.direction;
                }
                else
                {
                    NPC.velocity.X *= 0.93f;
                }
            }
        }
        int num779 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.TintableDust, NPC.velocity.X, NPC.velocity.Y, 255, new Color(0, 80, 255, 80), NPC.scale * 1.2f);
        Main.dust[num779].noGravity = true;
        dust34 = Main.dust[num779];
        dust87 = dust34;
        dust87.velocity *= 0.5f;
        if (NPC.life <= 0)
        {
            return;
        }
        float num780 = (float)NPC.life / (float)NPC.lifeMax;
        num780 = num780 * 0.5f + 0.75f;
        num780 *= num759;
        num780 *= num760;
        if (num780 != NPC.scale || flag68)
        {
            NPC.position.X += NPC.width / 2;
            NPC.position.Y += NPC.height;
            NPC.scale = num780;
            NPC.width = (int)(98f * NPC.scale);
            NPC.height = (int)(92f * NPC.scale);
            NPC.position.X -= NPC.width / 2;
            NPC.position.Y -= NPC.height;
        }
        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            return;
        }
        int num781 = (int)((double)NPC.lifeMax * 0.05);
        if (!((float)(NPC.life + num781) < NPC.ai[3]))
        {
            return;
        }
        NPC.ai[3] = NPC.life;
        int num782 = Main.rand.Next(1, 4);
        for (int num783 = 0; num783 < num782; num783++)
        {
            int x = (int)(NPC.position.X + (float)Main.rand.Next(NPC.width - 32));
            int y = (int)(NPC.position.Y + (float)Main.rand.Next(NPC.height - 32));
            int num784 = 1;
            if (Main.expertMode && Main.rand.NextBool(4) == true)
            {
                num784 = 535;
            }
            int num786 = NPC.NewNPC(NPC.GetSource_ReleaseEntity(), x, y, num784);
            Main.npc[num786].SetDefaults(num784);
            Main.npc[num786].velocity.X = (float)Main.rand.Next(-15, 16) * 0.1f;
            Main.npc[num786].velocity.Y = (float)Main.rand.Next(-30, 1) * 0.1f;
            Main.npc[num786].ai[0] = -1000 * Main.rand.Next(3);
            Main.npc[num786].ai[1] = 0f;
            if (Main.netMode == NetmodeID.Server && num786 < 200)
            {
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num786);
            }
        }
    }

    public override void SetBestiary(BestiaryDatabase dataNPC, BestiaryEntry bestiaryEntry)
    {
        // Sets the description of NPC NPC that is listed in the bestiary
        bestiaryEntry.Info.AddRange([
            new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("es el..... oh no...................")
        ]);
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 1, 9999 * 2));
        npcLoot.Add(ItemDropRule.Common(ItemID.Toilet, 1, 1, 9999));
    }

    public override bool CanHitPlayer(Player target, ref int cooldownSlot)
    {
        cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
        return true;
    }

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
    {
        modifiers.ScalingArmorPenetration += 32f;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {

        // sound and camera modifier goes on client
        if (Main.netMode == NetmodeID.Server)
            return;

        if (NPC.life <= 0)
        {
            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

            PunchCameraModifier modifier = new(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 6f, 20, 1000f, FullName);
            Main.instance.CameraModifiers.Add(modifier);
        }
    }

    // will not despawn naturally
    public override bool CheckActive() => false;

    // doesnt drop potions
    public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.None;
}
