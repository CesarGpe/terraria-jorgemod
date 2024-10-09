using eslamio.Common;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;

namespace eslamio.Content.Players;
class ExplodeNPCPlayer : ModPlayer
{
    public bool showImage = false;

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (KeybindSystem.ExplodeNPCBind.JustPressed)
        {
            var npc = FindClosestNPC(100);
            if (npc is not null && Collision.CanHit(Player, npc))
            {
                npc.AddBuff(ModContent.BuffType<FloatingBuff>(), 300);
                showImage = true;
            }
        }
    }

    private NPC FindClosestNPC(float maxDetectDistance)
    {
        NPC closestNPC = null;
        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;
        foreach (NPC target in Main.npc)
        {
            if (!target.townNPC)
                continue;

            float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Player.Center);
            if (sqrDistanceToTarget < sqrMaxDetectDistance)
            {
                sqrMaxDetectDistance = sqrDistanceToTarget;
                closestNPC = target;
            }
        }
        return closestNPC;
    }
}

class FloatingBuff : ModBuff
{
    SoundStyle scream = new("eslamio/Assets/Sounds/MaltigiScream");
    SoundStyle explosion = new("eslamio/Assets/Sounds/StockExplosion");

    public override string Texture => "eslamio/Assets/Textures/Blank";
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
        BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
    }

    bool playedSound;
    public override void Update(NPC npc, ref int buffIndex)
    {
        npc.aiStyle = -1;
        npc.noGravity = true;
        npc.velocity.X = 0f;
        npc.position.Y -= 1.5f;
        npc.direction = Main.rand.NextBool() ? 1 : -1;

        var time = npc.buffTime[npc.FindBuffIndex(Type)];

        if (time > 295 && time < 300)
            playedSound = false;
        else if (time > 240 && time < 295 && !playedSound)
        {
            SoundEngine.PlaySound(scream, npc.position);
            playedSound = true;
        }
        else if (time > 180 && time < 240)
        {
            npc.HitEffect(npc.direction, 9999999999999999999, true);
            npc.rotation += 0.5f;
        }
        else if (time > 0 && time < 180)
        {
            Projectile.NewProjectile(npc.GetSource_FromThis(), npc.position, Vector2.Zero, ModContent.ProjectileType<StockExplosion>(), 0, 0);
            SoundEngine.PlaySound(explosion, npc.position);
            npc.StrikeInstantKill();
        }
    }
}

class StockExplosion : ModProjectile
{
    public override string Texture => "eslamio/Assets/Textures/StockExplosion";
    public override void SetStaticDefaults() => Main.projFrames[Projectile.type] = 17;

    public override void AI()
    {
        Projectile.ai[0] += 1f;

        if (++Projectile.frameCounter >= 5)
        {
            Projectile.frameCounter = 0;
            if (++Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.frame = 0;
        }

        if (Projectile.ai[0] >= 85f)
            Projectile.Kill();
    }

    public override bool PreDraw(ref Color lightColor)
    {
        SpriteEffects spriteEffects = SpriteEffects.None;
        if (Projectile.spriteDirection == -1)
            spriteEffects = SpriteEffects.FlipHorizontally;

        Texture2D texture = TextureAssets.Projectile[Type].Value;

        int frameHeight = texture.Height / Main.projFrames[Type];
        int startY = frameHeight * Projectile.frame;

        Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
        Vector2 origin = sourceRectangle.Size() / 2f;

        float offsetX = 50f;
        origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;

        float offsetY = 70f;
        origin.Y = Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY;

        Main.EntitySpriteDraw(texture,
            Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
            sourceRectangle, Color.White, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

        return false;
    }
}

internal class ExplodesWithHeadUI : UIState
{
    float drawOpacity;

    public override void Draw(SpriteBatch spriteBatch)
    {
        Asset<Texture2D> texture = ModContent.Request<Texture2D>("eslamio/Assets/Textures/ExplodesYou");

        Vector2 drawOffset = Vector2.Zero;
        float xScale = (float)Main.screenWidth / texture.Width();
        float yScale = (float)Main.screenHeight / texture.Height();
        float scale = xScale;

        if (xScale != yScale)
        {
            if (yScale > xScale)
            {
                scale = yScale;
                drawOffset.X -= (texture.Width() * scale - Main.screenWidth) * 0.5f;
            }
            else
            {
                drawOffset.Y -= (texture.Height() * scale - Main.screenHeight) * 0.5f;
            }
        }
        spriteBatch.Draw(texture.Value, drawOffset, null, Color.White * drawOpacity, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

        base.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        if (Main.LocalPlayer.GetModPlayer<ExplodeNPCPlayer>().showImage)
        {
            drawOpacity = 1f;
            Main.LocalPlayer.GetModPlayer<ExplodeNPCPlayer>().showImage = false;
        }
        drawOpacity -= 0.02f;

        base.Update(gameTime);
    }
}

[Autoload(Side = ModSide.Client)]
internal class ExplodesWithHeadSystem : ModSystem
{
    private UserInterface Interface;

    internal ExplodesWithHeadUI ExplodesOverlay;

    public override void Load()
    {
        ExplodesOverlay = new();
        Interface = new();
        Interface.SetState(ExplodesOverlay);
    }

    public override void UpdateUI(GameTime gameTime) => Interface?.Update(gameTime);

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
        if (index != -1)
        {
            layers.Insert(index, new LegacyGameInterfaceLayer(
                "eslamio: Explodes You With My Mind",
                delegate
                {
                    Interface.Draw(Main.spriteBatch, new GameTime());
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}