using eslamio.Core;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;

namespace eslamio.Content.NPCs.Enemies;
public class DopActive : ModNPC
{
    SoundStyle disappearSound = new("eslamio/Assets/Sounds/Dop/Disappear") { PitchVariance = 0.5f };
    SoundStyle deathSound = new("eslamio/Assets/Sounds/Dop/Death") { PitchVariance = 0.5f };
    SoundStyle hitSound = new(SoundID.NPCHit37.SoundPath) { PitchVariance = 0.5f };

    public override string Texture => "eslamio/Content/NPCs/Enemies/DopSprite";

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BloodMummy];

        NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { PortraitPositionYOverride = -12f };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
    }

    public override void SetDefaults()
    {
        NPC.width = 18;
        NPC.height = 40;
        NPC.damage = 60;
        NPC.defense = 40;
        NPC.lifeMax = 400;
        NPC.rarity = 10;
        NPC.knockBackResist = 0f;
        NPC.HitSound = hitSound;
        NPC.DeathSound = deathSound;
        NPC.npcSlots = 10f;
        NPC.value = Main.rand.Next(50000, 100000);

        NPC.aiStyle = NPCAIStyleID.Fighter;
        AIType = NPCID.ZombieMerman;
        AnimationType = NPCID.BloodMummy;
    }

    public override void SetBestiary(BestiaryDatabase dataNPC, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange([
            //BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
            new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
            new FlavorTextBestiaryInfoElement("It's just you.")
        ]);
    }

    Player skin;
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (NPC.IsABestiaryIconDummy)
            return true;

        if (skin is null)
        {
            NPC.TargetClosest();
            skin = JiskUtils.ClonePlayer(Main.player[NPC.target]);

            NPC.GivenName = skin.name;
            NPC.damage *= (int)(skin.statLifeMax * 0.005);
            NPC.defense = skin.statDefense * 2;
            NPC.lifeMax = skin.statLifeMax * 2;
            NPC.life = NPC.lifeMax;
        }
        else
        {
            // drawing stuff
            skin.position.X = NPC.position.X;
            skin.position.Y = NPC.position.Y;

            skin.direction = NPC.direction;
            skin.headFrame.Y = NPC.frame.Y;
            skin.bodyFrame.Y = NPC.frame.Y;
            skin.legFrame.Y = NPC.frame.Y;
            Main.PlayerRenderer.DrawPlayer(Main.Camera, skin, skin.position, skin.fullRotation, skin.fullRotationOrigin, 0f);
        }

        return false;
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
    {
        if (Main.rand.NextBool(10))
            target.AddBuff(BuffID.Cursed, 180);
    }

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
    {
        modifiers.ScalingArmorPenetration += 1f;
    }

    const float speedX = 3.5f;
    public override bool PreAI()
    {
        NPC.TargetClosest();
        Lighting.AddLight(NPC.position, 0.5f, 0.2f, 0f);

        NPC.velocity.X /= speedX;
        return base.PreAI();
    }
    public override void PostAI()
    {
        NPC.velocity.X *= speedX;
        base.PostAI();
    }

    public override bool CheckActive()
    {
        if (NPC.timeLeft == 2)
        {
            if (!Main.dedServ)
                SoundEngine.PlaySound(disappearSound, null);

            Main.player[NPC.target].GetModPlayer<DopFollowPlayer>().dopSpawnMultiplier += 0.1f;

            NPC.despawnEncouraged = true;
            NPC.active = false;
            NPC.netSkip = -1;
            NPC.life = 0;
            return false;
        }
        return true;
    }

    public override void OnKill()
    {
        Main.player[NPC.target].GetModPlayer<DopFollowPlayer>().dopSpawnMultiplier = 0.02f;
    }

    internal class DopActiveBiome : ModBiome
    {
        public override int Music => MusicLoader.GetMusicSlot("eslamio/Assets/Music/DopChase");
        public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;
        public override string MapBackground => "Terraria/Images/MapBG25";
        public override bool IsBiomeActive(Player player)
        {
            return JiskUtils.NPCInDistance(ModContent.NPCType<DopActive>(), player.Center, 2000);
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("eslamio:VignetteStrong", isActive, player.Center);
        }
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        if (Main.netMode != NetmodeID.Server && !Main.dedServ && NPC.life <= 0)
        {
            Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 2), NPC.velocity, 11);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 2), NPC.velocity, 12);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 3), NPC.velocity, 13);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 3), NPC.velocity, 11);
        }
    }

    // only spawned by angering an inactive doppleganger
    public override float SpawnChance(NPCSpawnInfo spawnInfo) => 0f;

    public override void OnSpawn(IEntitySource source)
    {
        Main.NewText("Doppleganger spawned in active state?");
    }
}