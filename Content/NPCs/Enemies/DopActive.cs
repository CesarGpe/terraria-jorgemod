using eslamio.Content.Items.Pets.Familiar;
using eslamio.Content.Items.Weapons;
using eslamio.Content.Players;
using eslamio.Core;
using eslamio.Core.Helpers;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Effects;
using Terraria.ID;

namespace eslamio.Content.NPCs.Enemies;
public partial class DopActive : ModNPC
{
    SoundStyle disappearSound = new("eslamio/Assets/Sounds/Dop/Disappear") { PitchVariance = 1f };
    SoundStyle hitSound = new(SoundID.NPCHit37.SoundPath) { PitchVariance = 1f };
    SoundStyle deathSound = new("eslamio/Assets/Sounds/Dop/Death") { PitchVariance = 1f };

    private Player skin = null;

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

        //NPC.aiStyle = NPCAIStyleID.Fighter;
        //AIType = NPCID.ZombieMerman;
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

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (NPC.IsABestiaryIconDummy)
            return true;

        if (skin is null)
        {
            skin = JiskUtils.ClonePlayer(Main.player[DopSkinSystem.dopSkinID]);
            skin.eyeColor = Color.White; // herbronire?????

            NPC.GivenName = skin.name;
            NPC.damage = (int)(60 * skin.statLifeMax * 0.005);
            NPC.defense = skin.statDefense * 2;
            NPC.lifeMax = skin.statLifeMax * 2;
            NPC.life = NPC.lifeMax;

            NPC.netUpdate = true;
        }
        else
        {
            // drawing stuff
            skin.position = NPC.position - new Vector2(0, 2);
            skin.direction = NPC.direction;
            skin.headFrame = NPC.frame;
            skin.bodyFrame = NPC.frame;
            skin.legFrame = NPC.frame;
            skin.hairFrame = NPC.frame;
            skin.PlayerFrame();

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

    const float speedMultiplier = 3.5f;
    public override bool PreAI()
    {
        NPC.velocity.X /= speedMultiplier;
        return base.PreAI();
    }
    public override void PostAI()
    {
        NPC.velocity.X *= speedMultiplier;
        base.PostAI();
    }

    public override bool CheckActive()
    {
        //Main.NewText($"timeLeft: {NPC.timeLeft}");
        if (NPC.timeLeft == 2)
        {
            if (!Main.dedServ) SoundEngine.PlaySound(disappearSound, null);

            var player = Main.player[NPC.target];
            player.GetModPlayer<DopFollowPlayer>().dopSpawnMultiplier += 0.08f;
            player.GetModPlayer<CaveSounds>().PlaySound(Main.rand.Next(4), true);

            NPC.despawnEncouraged = true;
            NPC.active = false;
            NPC.netSkip = -1;
            NPC.life = 0;
            return false;
        }
        return true;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        if (NPC.life <= 0)
        {
            var prevVictim = Main.player[DopSkinSystem.dopSkinID];
            prevVictim.GetModPlayer<DopFollowPlayer>().dopSpawnMultiplier = 0.02f;
            for (int i = 0; i < Main.rand.Next(1, 4); i++)
            {
                GetRandomItem(prevVictim, out var item, out var amount);
                Main.player[NPC.target].QuickSpawnItem(prevVictim.GetSource_FromThis(), item.type, amount);
            }

            if (Main.netMode != NetmodeID.Server && !Main.dedServ)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 2), NPC.velocity, 11);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 2), NPC.velocity, 12);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 3), NPC.velocity, 13);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 3), NPC.velocity, 11);
            }
        }
    }

    private static void GetRandomItem(Player player, out Item item, out int amount)
    {
        item = player.inventory[Main.rand.Next(0, 50)];

        int maxAmount = 50;
        if (item.stack < 50)
            maxAmount = item.stack;

        amount = Main.rand.Next(0, maxAmount);
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FamiliarPetItem>(), 2));
    }

    internal class DopActiveBiome : ModBiome
    {
        public override int Music => MusicLoader.GetMusicSlot("eslamio/Assets/Music/DopChase");
        public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;
        public override string MapBackground => "Terraria/Images/MapBG25";
        public override bool IsBiomeActive(Player player)
        {
            return JiskUtils.NPCInDistance(ModContent.NPCType<DopActive>(), player.Center, 2500);
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("eslamio:VignetteStrong", isActive, player.Center);
        }
        public override void OnInBiome(Player player)
        {
            PunchCameraModifier modifier = new(player.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 1f, 6f, 2, 1f);
            Main.instance.CameraModifiers.Add(modifier);
        }
    }

    // only spawned by angering an inactive doppleganger
    public override float SpawnChance(NPCSpawnInfo spawnInfo) => 0f;

    public override void OnSpawn(IEntitySource source)
    {
        //Main.NewText("JiskMod: Doppleganger spawned in active state?", Color.DeepPink);
    }
}