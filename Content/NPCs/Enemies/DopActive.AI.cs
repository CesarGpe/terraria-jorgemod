using System;
using Terraria.Audio;
using Terraria.ID;

namespace eslamio.Content.NPCs.Enemies;
public partial class DopActive : ModNPC
{
    SoundStyle chaseSound = new(SoundID.Zombie94.SoundPath.Remove(SoundID.Zombie94.SoundPath.Length - 1))
        { PitchVariance = 0.5f, Volume = 0.8f, Variants = [4, 5, 6, 7, 8, 9] };

    // adapted from vanilla's fighter AI
    // with AIType = NPCID.ZombieMerman
    public override void AI()
    {
        if (Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height == NPC.position.Y + (float)NPC.height)
        {
            NPC.directionY = -1;
        }

        bool flag = false;

        if (NPC.alpha == 255)
        {
            NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction;
            NPC.velocity.Y = -6f;
            NPC.netUpdate = true;
            for (int j = 0; j < 35; j++)
            {
                Dust dust6 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Blood);
                dust6.velocity *= 1f;
                dust6.scale = 1f + Main.rand.NextFloat() * 0.5f;
                dust6.fadeIn = 1.5f + Main.rand.NextFloat() * 0.5f;
                dust6.velocity += NPC.velocity * 0.5f;
            }
        }
        NPC.alpha -= 15;
        if (NPC.alpha < 0)
        {
            NPC.alpha = 0;
        }
        NPC.position += NPC.netOffset;
        if (NPC.alpha != 0)
        {
            for (int k = 0; k < 2; k++)
            {
                Dust dust7 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Blood);
                dust7.velocity *= 1f;
                dust7.scale = 1f + Main.rand.NextFloat() * 0.5f;
                dust7.fadeIn = 1.5f + Main.rand.NextFloat() * 0.5f;
                dust7.velocity += NPC.velocity * 0.3f;
            }
        }
        if (Main.rand.NextBool(3))
        {
            Dust dust8 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Blood);
            dust8.velocity *= 0f;
            dust8.alpha = 120;
            dust8.scale = 0.7f + Main.rand.NextFloat() * 0.5f;
            dust8.velocity += NPC.velocity * 0.3f;
        }
        NPC.position -= NPC.netOffset;

        // wet stuff of zombie merman
        /*
        if (NPC.wet)
        {
            NPC.knockBackResist = 0f;
            NPC.ai[3] = -0.10101f;
            NPC.noGravity = true;
            Vector2 center3 = NPC.Center;
            NPC.position.X = center3.X - (float)(NPC.width / 2);
            NPC.position.Y = center3.Y - (float)(NPC.height / 2);
            NPC.TargetClosest();
            if (NPC.collideX)
            {
                NPC.velocity.X = 0f - NPC.oldVelocity.X;
            }
            if (NPC.velocity.X < 0f)
            {
                NPC.direction = -1;
            }
            if (NPC.velocity.X > 0f)
            {
                NPC.direction = 1;
            }
            if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].Center, 1, 1))
            {
                Vector2 vector21 = Main.player[NPC.target].Center - NPC.Center;
                vector21.Normalize();
                float num180 = 1f;
                num180 += Math.Abs(NPC.Center.Y - Main.player[NPC.target].Center.Y) / 40f;
                num180 = MathHelper.Clamp(num180, 5f, 20f);
                vector21 *= num180;
                if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity = (NPC.velocity * 29f + vector21) / 30f;
                }
                else
                {
                    NPC.velocity = (NPC.velocity * 4f + vector21) / 5f;
                }
                return;
            }
            float num191 = 5f;
            if (NPC.velocity.Y > 0f)
            {
                num191 = 3f;
            }
            if (NPC.velocity.Y < 0f)
            {
                num191 = 8f;
            }
            Vector2 vector31 = new(NPC.direction, -1f);
            vector31.Normalize();
            vector31 *= num191;
            if (num191 < 5f)
            {
                NPC.velocity = (NPC.velocity * 24f + vector31) / 25f;
            }
            else
            {
                NPC.velocity = (NPC.velocity * 9f + vector31) / 10f;
            }
            return;
        }
        */

        NPC.noGravity = false;
        Vector2 center4 = NPC.Center;
        NPC.position.X = center4.X - (float)(NPC.width / 2);
        NPC.position.Y = center4.Y - (float)(NPC.height / 2);
        if (NPC.ai[3] == -0.10101f)
        {
            NPC.ai[3] = 0f;
            float num2 = NPC.velocity.Length();
            num2 *= 2f;
            if (num2 > 15f)
            {
                num2 = 15f;
            }
            NPC.velocity.Normalize();
            NPC.velocity *= num2;
            if (NPC.velocity.X < 0f)
            {
                NPC.direction = -1;
            }
            if (NPC.velocity.X > 0f)
            {
                NPC.direction = 1;
            }
            NPC.spriteDirection = NPC.direction;
        }

        bool flag23 = false;
        bool flag24 = false;
        if (NPC.velocity.X == 0f)
        {
            flag24 = true;
        }
        if (NPC.justHit)
        {
            flag24 = false;
        }
        int num154 = 60;
        bool flag25 = false;
        bool flag26 = true;
        bool flag27 = false;
        bool flag2 = true;
        if (!flag27 && flag2)
        {
            if (NPC.velocity.Y == 0f && ((NPC.velocity.X > 0f && NPC.direction < 0) || (NPC.velocity.X < 0f && NPC.direction > 0)))
            {
                flag25 = true;
            }
            if (NPC.position.X == NPC.oldPosition.X || NPC.ai[3] >= (float)num154 || flag25)
            {
                NPC.ai[3] += 1f;
            }
            else if ((double)Math.Abs(NPC.velocity.X) > 0.9 && NPC.ai[3] > 0f)
            {
                NPC.ai[3] -= 1f;
            }
            if (NPC.ai[3] > (float)(num154 * 10))
            {
                NPC.ai[3] = 0f;
            }
            if (NPC.justHit)
            {
                NPC.ai[3] = 0f;
            }
            if (NPC.ai[3] == num154)
            {
                NPC.netUpdate = true;
            }
            if (Main.player[NPC.target].Hitbox.Intersects(NPC.Hitbox))
            {
                NPC.ai[3] = 0f;
            }
        }

        if (NPC.ai[3] < num154)
        {
            if (NPC.shimmerTransparency < 1f)
            {
                if (Main.rand.NextBool(150))
                {
                    if (!Main.dedServ) SoundEngine.PlaySound(chaseSound, NPC.position);
                }
            }
            NPC.TargetClosest();
            if (NPC.directionY > 0 && Main.player[NPC.target].Center.Y <= NPC.Bottom.Y)
            {
                NPC.directionY = -1;
            }
        }
        else if (!(NPC.ai[2] > 0f))
        {
            if (NPC.velocity.X == 0f)
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.ai[0] += 1f;
                    if (NPC.ai[0] >= 2f)
                    {
                        NPC.direction *= -1;
                        NPC.spriteDirection = NPC.direction;
                        NPC.ai[0] = 0f;
                    }
                }
            }
            else
            {
                NPC.ai[0] = 0f;
            }
            if (NPC.direction == 0)
            {
                NPC.direction = 1;
            }
        }

        float num181 = 1.5f + (1f - NPC.life / NPC.lifeMax) * 3.5f;

        if (NPC.velocity.X < 0f - num181 || NPC.velocity.X > num181)
        {
            if (NPC.velocity.Y == 0f)
            {
                NPC.velocity *= 0.8f;
            }
        }
        else if (NPC.velocity.X < num181 && NPC.direction == 1)
        {
            if (NPC.velocity.Y == 0f && NPC.velocity.X < -1f)
            {
                NPC.velocity.X *= 0.9f;
            }
            NPC.velocity.X += 0.07f;
            if (NPC.velocity.X > num181)
            {
                NPC.velocity.X = num181;
            }
        }
        else if (NPC.velocity.X > 0f - num181 && NPC.direction == -1)
        {
            if (NPC.velocity.Y == 0f && NPC.velocity.X > 1f)
            {
                NPC.velocity.X *= 0.9f;
            }
            NPC.velocity.X -= 0.07f;
            if (NPC.velocity.X < 0f - num181)
            {
                NPC.velocity.X = 0f - num181;
            }
        }

        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            if (Main.expertMode && NPC.target >= 0 && Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
            {
                NPC.localAI[0] += 1f;
                if (NPC.justHit)
                {
                    NPC.localAI[0] -= Main.rand.Next(20, 60);
                    if (NPC.localAI[0] < 0f)
                    {
                        NPC.localAI[0] = 0f;
                    }
                }
                if (NPC.localAI[0] > (float)Main.rand.Next(180, 900))
                {
                    NPC.localAI[0] = 0f;
                    Vector2 vector24 = Main.player[NPC.target].Center - NPC.Center;
                    vector24.Normalize();
                    vector24 *= 8f;
                    int attackDamage_ForProjectiles2 = NPC.GetAttackDamage_ForProjectiles(18f, 18f);
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center.X, NPC.Center.Y, vector24.X, vector24.Y, 472, attackDamage_ForProjectiles2, 0f, Main.myPlayer);
                }
            }
        }
        if (NPC.velocity.Y == 0f || flag)
        {
            int num91 = (int)(NPC.position.Y + (float)NPC.height + 7f) / 16;
            int num92 = (int)(NPC.position.Y - 9f) / 16;
            int num93 = (int)NPC.position.X / 16;
            int num94 = (int)(NPC.position.X + (float)NPC.width) / 16;
            int num205 = (int)(NPC.position.X + 8f) / 16;
            int num95 = (int)(NPC.position.X + (float)NPC.width - 8f) / 16;
            bool flag15 = false;
            for (int num96 = num205; num96 <= num95; num96++)
            {
                if (num96 >= num93 && num96 <= num94 && Main.tile[num96, num91] == null)
                {
                    flag15 = true;
                    continue;
                }
                if (Main.tile[num96, num92] != null && Main.tile[num96, num92].HasUnactuatedTile && Main.tileSolid[Main.tile[num96, num92].TileType])
                {
                    flag23 = false;
                    break;
                }
                if (!flag15 && num96 >= num93 && num96 <= num94 && Main.tile[num96, num91].HasUnactuatedTile && Main.tileSolid[Main.tile[num96, num91].TileType])
                {
                    flag23 = true;
                }
            }
            if (!flag23 && NPC.velocity.Y < 0f)
            {
                NPC.velocity.Y = 0f;
            }
            if (flag15)
            {
                return;
            }
        }
        if (NPC.velocity.Y >= 0f && NPC.directionY != 1)
        {
            int num97 = 0;
            if (NPC.velocity.X < 0f)
            {
                num97 = -1;
            }
            if (NPC.velocity.X > 0f)
            {
                num97 = 1;
            }
            Vector2 vector30 = NPC.position;
            vector30.X += NPC.velocity.X;
            int num98 = (int)((vector30.X + (float)(NPC.width / 2) + (float)((NPC.width / 2 + 1) * num97)) / 16f);
            int num100 = (int)((vector30.Y + (float)NPC.height - 1f) / 16f);
            if (WorldGen.InWorld(num98, num100, 4))
            {
                if ((float)(num98 * 16) < vector30.X + (float)NPC.width && (float)(num98 * 16 + 16) > vector30.X && ((Main.tile[num98, num100].HasUnactuatedTile && !Main.tile[num98, num100].TopSlope && !Main.tile[num98, num100 - 1].TopSlope && Main.tileSolid[Main.tile[num98, num100].TileType] && !Main.tileSolidTop[Main.tile[num98, num100].TileType]) || (Main.tile[num98, num100 - 1].IsHalfBlock && Main.tile[num98, num100 - 1].HasUnactuatedTile)) && (!Main.tile[num98, num100 - 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num98, num100 - 1].TileType] || Main.tileSolidTop[Main.tile[num98, num100 - 1].TileType] || (Main.tile[num98, num100 - 1].IsHalfBlock && (!Main.tile[num98, num100 - 4].HasUnactuatedTile || !Main.tileSolid[Main.tile[num98, num100 - 4].TileType] || Main.tileSolidTop[Main.tile[num98, num100 - 4].TileType]))) && (!Main.tile[num98, num100 - 2].HasUnactuatedTile || !Main.tileSolid[Main.tile[num98, num100 - 2].TileType] || Main.tileSolidTop[Main.tile[num98, num100 - 2].TileType]) && (!Main.tile[num98, num100 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num98, num100 - 3].TileType] || Main.tileSolidTop[Main.tile[num98, num100 - 3].TileType]) && (!Main.tile[num98 - num97, num100 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num98 - num97, num100 - 3].TileType]))
                {
                    float num101 = num100 * 16;
                    if (Main.tile[num98, num100].IsHalfBlock)
                    {
                        num101 += 8f;
                    }
                    if (Main.tile[num98, num100 - 1].IsHalfBlock)
                    {
                        num101 -= 8f;
                    }
                    if (num101 < vector30.Y + (float)NPC.height)
                    {
                        float num102 = vector30.Y + (float)NPC.height - num101;
                        float num103 = 16.1f;
                        if (num102 <= num103)
                        {
                            NPC.gfxOffY += NPC.position.Y + (float)NPC.height - num101;
                            NPC.position.Y = num101 - (float)NPC.height;
                            if (num102 < 9f)
                            {
                                NPC.stepSpeed = 1f;
                            }
                            else
                            {
                                NPC.stepSpeed = 2f;
                            }
                        }
                    }
                }
            }
        }
        if (flag23)
        {
            int num104 = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)(15 * NPC.direction)) / 16f);
            int num105 = (int)((NPC.position.Y + (float)NPC.height - 15f) / 16f);
            if (Main.tile[num104, num105 - 1].HasUnactuatedTile && (TileLoader.IsClosedDoor(Main.tile[num104, num105 - 1]) || Main.tile[num104, num105 - 1].TileType == 388) && flag26)
            {
                NPC.ai[2] += 1f;
                NPC.ai[3] = 0f;
                if (NPC.ai[2] >= 60f)
                {
                    NPC.velocity.X = 0.5f * -NPC.direction;
                    int num106 = 5;
                    if (Main.tile[num104, num105 - 1].TileType == 388)
                    {
                        num106 = 2;
                    }
                    NPC.ai[1] += num106;
                    NPC.ai[2] = 0f;
                    bool flag18 = false;
                    if (NPC.ai[1] >= 10f)
                    {
                        flag18 = true;
                        NPC.ai[1] = 10f;
                    }
                    WorldGen.KillTile(num104, num105 - 1, fail: true);
                    if ((Main.netMode != NetmodeID.MultiplayerClient || !flag18) && flag18 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (TileLoader.IsClosedDoor(Main.tile[num104, num105 - 1]))
                        {
                            bool flag19 = WorldGen.OpenDoor(num104, num105 - 1, NPC.direction);
                            if (!flag19)
                            {
                                NPC.ai[3] = num154;
                                NPC.netUpdate = true;
                            }
                            if (Main.netMode == NetmodeID.Server && flag19)
                            {
                                NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, num104, num105 - 1, NPC.direction);
                            }
                        }
                        if (Main.tile[num104, num105 - 1].TileType == 388)
                        {
                            bool flag20 = WorldGen.ShiftTallGate(num104, num105 - 1, closing: false);
                            if (!flag20)
                            {
                                NPC.ai[3] = num154;
                                NPC.netUpdate = true;
                            }
                            if (Main.netMode == NetmodeID.Server && flag20)
                            {
                                NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4, num104, num105 - 1);
                            }
                        }
                    }
                }
            }
            else
            {
                int num107 = NPC.spriteDirection;
                if ((NPC.velocity.X < 0f && num107 == -1) || (NPC.velocity.X > 0f && num107 == 1))
                {
                    if (NPC.height >= 32 && Main.tile[num104, num105 - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[num104, num105 - 2].TileType])
                    {
                        if (Main.tile[num104, num105 - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[num104, num105 - 3].TileType])
                        {
                            NPC.velocity.Y = -8f;
                            NPC.netUpdate = true;
                        }
                        else
                        {
                            NPC.velocity.Y = -7f;
                            NPC.netUpdate = true;
                        }
                    }
                    else if (Main.tile[num104, num105 - 1].HasUnactuatedTile && Main.tileSolid[Main.tile[num104, num105 - 1].TileType])
                    {
                        NPC.velocity.Y = -6f;
                        NPC.netUpdate = true;
                    }
                    else if (NPC.position.Y + (float)NPC.height - (float)(num105 * 16) > 20f && Main.tile[num104, num105].HasUnactuatedTile && !Main.tile[num104, num105].TopSlope && Main.tileSolid[Main.tile[num104, num105].TileType])
                    {
                        NPC.velocity.Y = -5f;
                        NPC.netUpdate = true;
                    }
                    else if (flag26)
                    {
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                    }
                    if (NPC.velocity.Y == 0f && flag24 && NPC.ai[3] == 1f)
                    {
                        NPC.velocity.Y = -5f;
                    }
                    // this is upgraded fighter AI in expert mode i think
                    if (NPC.velocity.Y == 0f && Main.player[NPC.target].Bottom.Y < NPC.Top.Y && Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) < (float)(Main.player[NPC.target].width * 3) && Collision.CanHit(NPC, Main.player[NPC.target]))
                    {
                        int num109 = (int)((NPC.Bottom.Y - 16f - Main.player[NPC.target].Bottom.Y) / 16f);
                        if (num109 < 14 && Collision.CanHit(NPC, Main.player[NPC.target]))
                        {
                            if (num109 < 7)
                            {
                                NPC.velocity.Y = -8.8f;
                            }
                            else if (num109 < 8)
                            {
                                NPC.velocity.Y = -9.2f;
                            }
                            else if (num109 < 9)
                            {
                                NPC.velocity.Y = -9.7f;
                            }
                            else if (num109 < 10)
                            {
                                NPC.velocity.Y = -10.3f;
                            }
                            else if (num109 < 11)
                            {
                                NPC.velocity.Y = -10.6f;
                            }
                            else
                            {
                                NPC.velocity.Y = -11f;
                            }
                        }

                        if (NPC.velocity.Y == 0f)
                        {
                            int num112 = 6;
                            if (Main.player[NPC.target].Bottom.Y > NPC.Top.Y - (float)(num112 * 16))
                            {
                                NPC.velocity.Y = -7.9f;
                            }
                            else
                            {
                                int num113 = (int)(NPC.Center.X / 16f);
                                int num114 = (int)(NPC.Bottom.Y / 16f) - 1;
                                for (int num115 = num114; num115 > num114 - num112; num115--)
                                {
                                    if (Main.tile[num113, num115].HasUnactuatedTile && TileID.Sets.Platforms[Main.tile[num113, num115].TileType])
                                    {
                                        NPC.velocity.Y = -7.9f;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else if (flag26)
        {
            NPC.ai[1] = 0f;
            NPC.ai[2] = 0f;
        }
    }
}