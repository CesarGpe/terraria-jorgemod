using eslamio.Core;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace eslamio.Content.Buffs;

public class GlebaBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = false;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        Lighting.AddLight(player.position, 0.3f, 0.33f, 1);

        const float colorMult = 0.005f;
        if (!Main.dedServ)
            player.GetModPlayer<GlebaShaderPlayer>().SetColor(Main.DiscoR * colorMult, Main.DiscoG * colorMult, Main.DiscoB * colorMult);
    }
}

internal class GlebaShaderPlayer : ModPlayer
{
    public bool IsActive;

    private float Red;
    private float Green;
    private float Blue;

    public override void ResetEffects()
    {
        IsActive = false;
        Red = 1f;
        Green = 1f;
        Blue = 1f;
    }

    public void SetColor(float red, float green, float blue)
    {
        IsActive = true;
        Red = red;
        Green = green;
        Blue = blue;
    }

    public override void PostUpdateMiscEffects()
    {
        eslamio.screenTintEffect.Parameters["Red"].SetValue(Red);
        eslamio.screenTintEffect.Parameters["Green"].SetValue(Green);
        eslamio.screenTintEffect.Parameters["Blue"].SetValue(Blue);
        Player.ManageSpecialBiomeVisuals("eslamio:ScreenTintShader", IsActive, Main.screenPosition);
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        // dust
        if (IsActive)
        {
            if (Main.myPlayer == Player.whoAmI)
            {
                Rectangle screenArea = new Rectangle((int)Main.screenPosition.X - 500, (int)Main.screenPosition.Y - 50, Main.screenWidth + 1000, Main.screenHeight + 100);
                int dustDrawn = 0;
                float maxShroomDust = Main.maxDustToDraw / 2;
                Color shroomColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, Main.DiscoR);
                for (int i = 0; i < Main.maxDustToDraw; i++)
                {
                    Dust dust = Main.dust[i];
                    if (dust.active)
                    {
                        // Only draw dust near the screen, for performance reasons.
                        if (new Rectangle((int)dust.position.X, (int)dust.position.Y, 4, 4).Intersects(screenArea))
                        {
                            dust.color = shroomColor;
                            for (int j = 0; j < 4; j++)
                            {
                                Vector2 dustDrawPosition = dust.position;
                                Vector2 dustCenter = dustDrawPosition + new Vector2(4f);

                                float distanceX = Math.Abs(dustCenter.X - Player.Center.X);
                                float distanceY = Math.Abs(dustCenter.Y - Player.Center.Y);
                                if (j == 0 || j == 2)
                                    dustDrawPosition.X = Player.Center.X + distanceX;
                                else
                                    dustDrawPosition.X = Player.Center.X - distanceX;

                                dustDrawPosition.X -= 4f;

                                if (j == 0 || j == 1)
                                    dustDrawPosition.Y = Player.Center.Y + distanceY;
                                else
                                    dustDrawPosition.Y = Player.Center.Y - distanceY;

                                dustDrawPosition.Y -= 4f;
                                Main.spriteBatch.Draw(TextureAssets.Dust.Value, dustDrawPosition - Main.screenPosition, dust.frame, dust.color, dust.rotation, new Vector2(4f), dust.scale, SpriteEffects.None, 0f);
                                dustDrawn++;
                            }

                            // Break if too many dust clones have been drawn
                            if (dustDrawn > maxShroomDust)
                                break;
                        }
                    }
                }
            }
        }
    }
}

internal class GlebalProjectile : GlobalProjectile
{
    public override Color? GetAlpha(Projectile projectile, Color lightColor)
    {
        if (Main.player[Main.myPlayer].GetModPlayer<GlebaShaderPlayer>().IsActive)
            return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, Main.DiscoR);

        return base.GetAlpha(projectile, lightColor);
    }

    public override bool PreDraw(Projectile projectile, ref Color lightColor)
    {
        if (Main.player[Main.myPlayer].GetModPlayer<GlebaShaderPlayer>().IsActive)
        {
            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Color rainbow = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, Main.DiscoR);
            Color alphaColor = projectile.GetAlpha(rainbow);
            float RGBMult = 0.99f;
            alphaColor.R = (byte)(alphaColor.R * RGBMult);
            alphaColor.G = (byte)(alphaColor.G * RGBMult);
            alphaColor.B = (byte)(alphaColor.B * RGBMult);
            alphaColor.A = (byte)(alphaColor.A * RGBMult);
            int totalAfterimages = 16;
            for (int i = 0; i < totalAfterimages; i++)
            {
                Vector2 position = projectile.position;
                float distanceFromTargetX = Math.Abs(projectile.Center.X - Main.player[Main.myPlayer].Center.X);
                float distanceFromTargetY = Math.Abs(projectile.Center.Y - Main.player[Main.myPlayer].Center.Y);

                float smallDistanceMult = 0.48f;
                float largeDistanceMult = 1.33f;
                bool whatTheFuck = true;

                switch (i)
                {
                    case 0:
                        position.X = Main.player[Main.myPlayer].Center.X - distanceFromTargetX;
                        position.Y = Main.player[Main.myPlayer].Center.Y - distanceFromTargetY;
                        break;

                    case 1:
                        position.X = Main.player[Main.myPlayer].Center.X + distanceFromTargetX;
                        position.Y = Main.player[Main.myPlayer].Center.Y - distanceFromTargetY;
                        break;

                    case 2:
                        position.X = Main.player[Main.myPlayer].Center.X + distanceFromTargetX;
                        position.Y = Main.player[Main.myPlayer].Center.Y + distanceFromTargetY;
                        break;

                    case 3:
                        position.X = Main.player[Main.myPlayer].Center.X - distanceFromTargetX;
                        position.Y = Main.player[Main.myPlayer].Center.Y + distanceFromTargetY;
                        break;

                    case 4: // 1 o'clock position
                        position.X = Main.player[Main.myPlayer].Center.X + (distanceFromTargetX * (whatTheFuck ? 1f : smallDistanceMult));
                        position.Y = Main.player[Main.myPlayer].Center.Y - (distanceFromTargetY * (whatTheFuck ? 0f : largeDistanceMult));
                        break;

                    case 5: // 4 o'clock position
                        position.X = Main.player[Main.myPlayer].Center.X + (distanceFromTargetX * (whatTheFuck ? 0f : largeDistanceMult));
                        position.Y = Main.player[Main.myPlayer].Center.Y + (distanceFromTargetY * (whatTheFuck ? 1f : smallDistanceMult));
                        break;

                    case 6: // 7 o'clock position
                        position.X = Main.player[Main.myPlayer].Center.X - (distanceFromTargetX * (whatTheFuck ? 1f : smallDistanceMult));
                        position.Y = Main.player[Main.myPlayer].Center.Y + (distanceFromTargetY * (whatTheFuck ? 0f : largeDistanceMult));
                        break;

                    case 7: // 10 o'clock position
                        position.X = Main.player[Main.myPlayer].Center.X - (distanceFromTargetX * (whatTheFuck ? 0f : largeDistanceMult));
                        position.Y = Main.player[Main.myPlayer].Center.Y - (distanceFromTargetY * (whatTheFuck ? 1f : smallDistanceMult));
                        break;

                    case 8: // 11 o'clock position
                        position.X = Main.player[Main.myPlayer].Center.X - (distanceFromTargetX * (whatTheFuck ? 0f : smallDistanceMult));
                        position.Y = Main.player[Main.myPlayer].Center.Y - (distanceFromTargetY * (whatTheFuck ? 0.5f : largeDistanceMult));
                        break;

                    case 9: // 2 o'clock position
                        position.X = Main.player[Main.myPlayer].Center.X + (distanceFromTargetX * (whatTheFuck ? 0.5f : largeDistanceMult));
                        position.Y = Main.player[Main.myPlayer].Center.Y - (distanceFromTargetY * (whatTheFuck ? 0f : smallDistanceMult));
                        break;

                    case 10: // 5 o'clock position
                        position.X = Main.player[Main.myPlayer].Center.X + (distanceFromTargetX * (whatTheFuck ? 0f : smallDistanceMult));
                        position.Y = Main.player[Main.myPlayer].Center.Y + (distanceFromTargetY * (whatTheFuck ? 0.5f : largeDistanceMult));
                        break;

                    case 11: // 8 o'clock position
                        position.X = Main.player[Main.myPlayer].Center.X - (distanceFromTargetX * (whatTheFuck ? 0.5f : largeDistanceMult));
                        position.Y = Main.player[Main.myPlayer].Center.Y + (distanceFromTargetY * (whatTheFuck ? 0f : smallDistanceMult));
                        break;

                    case 12:
                        position.X = Main.player[Main.myPlayer].Center.X - distanceFromTargetX * 0.5f;
                        position.Y = Main.player[Main.myPlayer].Center.Y - distanceFromTargetY * 0.5f;
                        break;

                    case 13:
                        position.X = Main.player[Main.myPlayer].Center.X + distanceFromTargetX * 0.5f;
                        position.Y = Main.player[Main.myPlayer].Center.Y - distanceFromTargetY * 0.5f;
                        break;

                    case 14:
                        position.X = Main.player[Main.myPlayer].Center.X + distanceFromTargetX * 0.5f;
                        position.Y = Main.player[Main.myPlayer].Center.Y + distanceFromTargetY * 0.5f;
                        break;

                    case 15:
                        position.X = Main.player[Main.myPlayer].Center.X - distanceFromTargetX * 0.5f;
                        position.Y = Main.player[Main.myPlayer].Center.Y + distanceFromTargetY * 0.5f;
                        break;

                    default:
                        break;
                }

                position.X -= projectile.width / 2;
                position.Y -= projectile.height / 2;

                int frameHeight = texture.Height / Main.projFrames[projectile.type];
                int currentframeHeight = frameHeight * projectile.frame;
                Rectangle frame = new Rectangle(0, currentframeHeight, texture.Width, frameHeight);

                Vector2 halfSize = frame.Size() / 2;

                Main.spriteBatch.Draw(texture,
                    new Vector2(position.X - Main.screenPosition.X + (float)(projectile.width / 2) - (float)TextureAssets.Projectile[projectile.type].Width() * projectile.scale / 2f + halfSize.X * projectile.scale,
                    position.Y - Main.screenPosition.Y + (float)projectile.height - (float)TextureAssets.Projectile[projectile.type].Height() * projectile.scale / (float)Main.projFrames[projectile.type] + 4f + halfSize.Y * projectile.scale + projectile.gfxOffY),
                    frame, alphaColor, projectile.rotation, halfSize, projectile.scale, spriteEffects, 0f);
            }
        }

        return base.PreDraw(projectile, ref lightColor);
    }
}