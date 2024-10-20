using eslamio.Core;
using System.IO;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader.Utilities;

namespace eslamio.Content.NPCs.Enemies;
public class DopInactive : ModNPC
{
    SoundStyle disappearSound = new("eslamio/Assets/Sounds/Dop/Disappear") { PitchVariance = 0.5f };
    SoundStyle deathSound = new("eslamio/Assets/Sounds/Dop/Death") { PitchVariance = 0.5f };
    SoundStyle spottedSound = new("eslamio/Assets/Sounds/Dop/Spotted") { PitchVariance = 0.5f };
    SoundStyle hitSound = new(SoundID.NPCHit37.SoundPath) { PitchVariance = 0.5f };

    public override string Texture => "eslamio/Content/NPCs/Enemies/DopSprite";

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BloodMummy];

        NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Hide = true };
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
        NPC.friendly = false;
        NPC.npcSlots = 10f;
        NPC.value = Main.rand.Next(50000, 100000);

        AnimationType = NPCID.BloodMummy;
    }

    public override bool CanBeHitByNPC(NPC attacker) => false;
    public override bool? CanBeHitByItem(Player player, Item item) => false;
    public override bool? CanBeHitByProjectile(Projectile projectile) => false;

    public override void SetBestiary(BestiaryDatabase dataNPC, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange([
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
            new FlavorTextBestiaryInfoElement("You shouldn't be reading NPC.")
        ]);
    }

    Player skin;
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (NPC.IsABestiaryIconDummy)
            return true;

        if (skin is not null)
        {
            // drawing stuff
            skin.position.X = NPC.position.X;
            skin.position.Y = NPC.position.Y;

            skin.direction = NPC.direction;
            skin.headFrame.Y = NPC.frame.Y;
            skin.bodyFrame.Y = NPC.frame.Y;
            skin.legFrame.Y = NPC.frame.Y;
            Main.PlayerRenderer.DrawPlayer(Main.Camera, skin, skin.position, skin.fullRotation, skin.fullRotationOrigin);
        }

        return false;
    }

    ref float AngerLevel => ref NPC.ai[0];
    public override void AI()
    {
        NPC.TargetClosest();
        Lighting.AddLight(NPC.position, 0.8f, 0.5f, 0f);

        if (AngerLevel == 0f)
        {
            if (NPC.target >= 0)
            {
                var target = Main.player[NPC.target];
                float totalDistance = NPC.Center.DistanceSQ(target.Center);
                if (totalDistance < 22500f && Collision.CanHit(NPC.position, NPC.width, NPC.height, target.position, target.width, target.height))
                    AngerLevel = 1f;
            }
            if (NPC.velocity.X != 0f || NPC.velocity.Y < 0f || NPC.velocity.Y > 2f || NPC.life != NPC.lifeMax)
                AngerLevel = 1f;
        }
        else
        {
            AngerLevel += 1f;
            if (AngerLevel >= 21f)
            {
                AngerLevel = 21f;
                NPC.Transform(ModContent.NPCType<DopActive>());
                SoundEngine.PlaySound(spottedSound, NPC.position);
            }
        }
    }

    public override bool CheckActive()
    {
        Main.NewText($"timeLeft: {NPC.timeLeft}");
        if (NPC.timeLeft == 2)
        {
            if (!Main.dedServ)
                SoundEngine.PlaySound(disappearSound, null);

            Main.player[NPC.target].GetModPlayer<DopFollowPlayer>().dopSpawnMultiplier += 0.08f;

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

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (!NPC.AnyNPCs(Type) && !NPC.AnyNPCs(ModContent.NPCType<DopActive>()) &&
            (spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight))
            return SpawnCondition.Cavern.Chance * spawnInfo.Player.GetModPlayer<DopFollowPlayer>().dopSpawnMultiplier;

        return 0f;
    }

    public override void OnSpawn(IEntitySource source)
    {
        AngerLevel = 0;

        NPC.TargetClosest();
        skin = JiskUtils.ClonePlayer(Main.player[NPC.target]);

        if (skin is not null)
        {
            NPC.GivenName = skin.name;
            NPC.damage *= (int)(skin.statLifeMax * 0.005);
            NPC.defense = skin.statDefense * 2;
            NPC.lifeMax = skin.statLifeMax * 2;
            NPC.life = NPC.lifeMax;
        }
    }

    internal class DopInactiveBiome : ModBiome
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;
        public override bool IsBiomeActive(Player player)
        {
            return JiskUtils.NPCInDistance(ModContent.NPCType<DopInactive>(), player.Center, 2000);
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("eslamio:VignetteLight", isActive, player.Center);
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

    /*private static bool IsNpcOnscreen(Vector2 center)
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
    }*/

    /*private bool CheckForPlayer(float maxDetectDistance)
    {
        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

        NPC.TargetClosest();
        if (!NPC.HasPlayerTarget) return false;
        var target = Main.player[NPC.target];

        float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, NPC.Center);
        if (sqrDistanceToTarget < sqrMaxDetectDistance && Collision.CanHit(NPC.position, NPC.width, NPC.height, target.position, target.width, target.height))
            return true;

        return false;
    }*/

    /*private List<Player> FindPlayersInRadius(float maxDetectDistance)
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
    }*/

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