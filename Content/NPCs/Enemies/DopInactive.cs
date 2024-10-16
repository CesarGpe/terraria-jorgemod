using eslamio.Core;
using eslamio.Effects;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader.Utilities;

namespace eslamio.Content.NPCs.Enemies;

public class DopInactive : ModNPC
{
    Player skin;
    SoundStyle disappearSound = new("eslamio/Assets/Sounds/Dop/Disappear") { PitchVariance = 0.5f };
    SoundStyle deathSound = new("eslamio/Assets/Sounds/Dop/Death") { PitchVariance = 0.5f };
    SoundStyle spottedSound = new("eslamio/Assets/Sounds/Dop/Spotted") { PitchVariance = 0.5f };
    SoundStyle hitSound = new(SoundID.NPCHit37.SoundPath) { PitchVariance = 0.5f };

    public override string Texture => "eslamio/Content/NPCs/Enemies/DopSprite";

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BloodMummy];

        NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() { Hide = true };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
    }

    public override void SetDefaults()
    {
        NPC.width = 32;
        NPC.height = 48;
        NPC.damage = 60;
        NPC.defense = 40;
        NPC.lifeMax = 400;
        NPC.rarity = 10;
        NPC.HitSound = hitSound;
        NPC.DeathSound = deathSound;
        NPC.value = Main.rand.Next(25000, 50000);
        NPC.knockBackResist = 0f;

        //NPC.aiStyle = NPCAIStyleID.LostGirl;
        //AIType = NPCID.BloodMummy;
        AnimationType = NPCID.BloodMummy;
    }

    public override void SetBestiary(BestiaryDatabase dataNPC, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange([
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
            new FlavorTextBestiaryInfoElement("You shouldn't be reading this.")
        ]);
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (NPC.IsABestiaryIconDummy)
            return true;

        if (skin is not null)
        {
            // drawing stuff
            skin.position.X = NPC.position.X;
            skin.position.Y = NPC.position.Y + 6;

            skin.direction = NPC.direction;
            skin.headFrame.Y = NPC.frame.Y;
            skin.bodyFrame.Y = NPC.frame.Y;
            skin.legFrame.Y = NPC.frame.Y;
            Main.PlayerRenderer.DrawPlayer(Main.Camera, skin, skin.position, skin.fullRotation, skin.fullRotationOrigin);
        }

        return false;
    }

    // will not despawn naturally, we handle that in PreAI
    public override bool CheckActive() => false;

    public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
    {
        if (Main.rand.NextBool(10))
            target.AddBuff(BuffID.Cursed, 180);
    }

    ref float DespawnTimer => ref NPC.ai[0];
    public override void AI()
    {
        NPC.TargetClosest();
        Lighting.AddLight(NPC.position, 0.5f, 0.2f, 0);
        var players = FindPlayersInRadius(1200);

        // set screen effect for nearby players
        foreach (var player in players)
        {
            VignettePlayer vignettePlayer = player.GetModPlayer<VignettePlayer>();
            vignettePlayer.SetVignette(0f, 900f, 0.85f, Color.Black, player.Center, 0.35f);
            vignettePlayer.shakeIntensity = 0.6f;
        }

        // raise the timer for despawn when the player is too far away
        if (IsNpcOnscreen(NPC.Center))
            DespawnTimer = 0;
        else
            DespawnTimer++;

        // if attacked or player comes nearby it activates
        if (CheckForPlayer(150) || NPC.life != NPC.lifeMax)
        {
            SoundEngine.PlaySound(spottedSound, NPC.position);
            NPC.Transform(ModContent.NPCType<DopActive>());
        }

        // despawn and play a sound when it does
        if (DespawnTimer == 350)
        {
            if (!Main.dedServ)
                SoundEngine.PlaySound(disappearSound, NPC.Center);

            NPC.despawnEncouraged = true;
            NPC.active = false;
            NPC.netSkip = -1;
            NPC.life = 0;
        }

        // debug trash
        //Main.NewText($"ai[0]: {NPC.ai[0]}, ai[1]: {NPC.ai[1]}, ai[2]: {NPC.ai[2]}, ai[3]: {NPC.ai[3]}");
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (!NPC.AnyNPCs(Type) && (spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight))
            return SpawnCondition.Cavern.Chance * 0.02f;

        return 0f;
    }

    public override void OnSpawn(IEntitySource source)
    {
        DespawnTimer = 0;

        NPC.TargetClosest();
        skin = eslamio.playerCloneHelper.ClonePlayer(Main.player[NPC.target]);

        if (skin is not null)
        {
            NPC.GivenName = skin.name;
            NPC.damage *= (int)(skin.statLifeMax * 0.005);
            NPC.defense = skin.statDefense * 2;
            NPC.lifeMax = skin.statLifeMax * 2;
            NPC.life = NPC.lifeMax;

            //Main.NewText("You can hear someone mining in the distance.", Color.MediumPurple);
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

    /*public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<Items.Sets.RunicSet.Rune>();
			npcLoot.AddCommon<SoulDagger>(25);
		}*/

    private static bool IsNpcOnscreen(Vector2 center)
    {
        int w = NPC.sWidth + NPC.safeRangeX * 2;
        int h = NPC.sHeight + NPC.safeRangeY * 2;
        Rectangle npcScreenRect = new((int)center.X - w / 2, (int)center.Y - h / 2, w, h);
        foreach (Player player in Main.ActivePlayers)
        {
            if (player.getRect().Intersects(npcScreenRect))
                return true;
        }
        return false;
    }

    private bool CheckForPlayer(float maxDetectDistance)
    {
        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

        NPC.TargetClosest();
        if (!NPC.HasPlayerTarget) return false;
        var target = Main.player[NPC.target];

        float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, NPC.Center);
        if (sqrDistanceToTarget < sqrMaxDetectDistance && Collision.CanHit(NPC.position, NPC.width, NPC.height, target.position, target.width, target.height))
            return true;

        return false;
    }

    private List<Player> FindPlayersInRadius(float maxDetectDistance)
    {
        List<Player> foundPlayers = [];
        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

        foreach (Player target in Main.ActivePlayers)
        {
            float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, NPC.Center);

            if (sqrDistanceToTarget < sqrMaxDetectDistance)
            {
                sqrMaxDetectDistance = sqrDistanceToTarget;
                foundPlayers.Add(target);
            }
        }

        return foundPlayers;
    }

    /*private Player FindClosestPlayer(float maxDetectDistance)
    {
        Player closestPlayer = null;

        // Using squared values in distance checks will let us skip square root calculations, drastically improving NPC method's speed.
        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

        // Loop through all Players
        foreach (Player target in Main.ActivePlayers)
        {
            // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
            float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, NPC.Center);

            // Check if it is within the radius
            if (sqrDistanceToTarget < sqrMaxDetectDistance)
            {
                sqrMaxDetectDistance = sqrDistanceToTarget;
                closestPlayer = target;
            }
        }

        return closestPlayer;
    }*/
}