using System.Collections.Frozen;
using System.Globalization;

namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for a skill template ID.
/// </summary>
[StrongIntId]
public readonly partial record struct SkillTemplateId(int Value)
{
    // ── Digit Decomposition ───────────────────────────────────────────────

    /// <summary>
    /// The skill root — equals the job ID that owns this skill.
    /// Also the integer name of the WZ image: Root + ".img".
    /// Formula: nSkillID / 10_000.
    /// </summary>
    public int Root => Value / 10_000;

    /// <summary>Serial number within the job (last 4 digits): nSkillID % 10_000.</summary>
    public int Serial => Value % 10_000;

    /// <summary>
    /// Category of the owning job (1=Warrior…5=Pirate, 0=Beginner).
    /// Formula: (nSkillID / 10_000) % 1_000 / 100.
    /// </summary>
    public int JobCategory => Root % 1_000 / 100;

    /// <summary>
    /// Lineage of the owning job (0=Explorer, 1=Cygnus, 2=Aran/Evan, 3=Resistance).
    /// Formula: (nSkillID / 10_000) / 1_000.
    /// </summary>
    public int JobLineage => Root / 1_000;

    /// <summary>
    /// WZ image filename (without path), e.g. skill ID 1_111_002 → "111.img".
    /// Formula: (nSkillID / 10_000).ToString() + ".img"
    /// </summary>
    [Obsolete("Allocates. Use TryWriteWzImageName(Span<char>, out int) on hot paths.")]
    public string WzImageName => $"{Root}.img";

    /// <summary>
    /// Writes the WZ image name (<c>"{Root}.img"</c>) into the provided span without heap allocation.
    /// </summary>
    public bool TryWriteWzImageName(Span<char> destination, out int charsWritten)
    {
        // Root is 1–4 digits; minimum output is 1 + 4 = 5 chars.
        if (destination.Length < 5)
        {
            charsWritten = 0;
            return false;
        }

        if (
            !Root.TryFormat(destination, out int rootChars, default, CultureInfo.InvariantCulture)
            || destination.Length < rootChars + 4
        )
        {
            charsWritten = 0;
            return false;
        }

        ".img".AsSpan().CopyTo(destination[rootChars..]);
        charsWritten = rootChars + 4;
        return true;
    }

    /// <summary>
    /// Full WZ node path to this skill's data (icon, effect, action, etc.).
    /// Formula: <c>"{Root}.img/skill/{Value}"</c>.
    /// The intermediate <c>skill/</c> node is confirmed by unit test validation.
    /// Example: skill 1_001_004 → <c>"100.img/skill/1001004"</c>.
    /// </summary>
    [Obsolete("Allocates. Use TryWriteWzSkillNodePath(Span<char>, out int) on hot paths.")]
    public string WzSkillNodePath => $"{Root}.img/skill/{Value}";

    /// <summary>
    /// Writes the WZ skill node path (<c>"{Root}.img/skill/{Value}"</c>) into the provided span without heap allocation.
    /// </summary>
    public bool TryWriteWzSkillNodePath(Span<char> destination, out int charsWritten)
    {
        // Minimum: root (1–4 digits) + ".img/skill/" (11) + value (1–9 digits) = 13 chars.
        if (destination.Length < 13)
        {
            charsWritten = 0;
            return false;
        }

        if (!Root.TryFormat(destination, out int rootChars, default, CultureInfo.InvariantCulture))
        {
            charsWritten = 0;
            return false;
        }

        ReadOnlySpan<char> mid = ".img/skill/";
        if (destination.Length < rootChars + mid.Length)
        {
            charsWritten = 0;
            return false;
        }
        mid.CopyTo(destination[rootChars..]);
        int written = rootChars + mid.Length;

        if (!Value.TryFormat(destination[written..], out int valueChars, default, CultureInfo.InvariantCulture))
        {
            charsWritten = 0;
            return false;
        }

        charsWritten = written + valueChars;
        return true;
    }

    // ── Family Discriminators ─────────────────────────────────────────────

    /// <summary>
    /// True for guild passive skills (91_000_000 – 91_009_999). Root = 9100.
    /// ⚠ NOT the same as IsSuperGmSkill (root 910) — root 9100 (guild) ≠ root 910 (SuperGM).
    /// </summary>
    public bool IsGuildSkill => Root == 9100;

    /// <summary>
    /// True for Admin/SuperGM group 2 skills (9_100_000 – 9_109_999). Root = 910.
    /// ⚠ NOT guild skills — guild root is 9100, not 910.
    /// </summary>
    public bool IsSuperGmSkill => Root == 910;

    /// <summary>True for Admin/SuperGM group 1 skills (9_000_000 – 9_009_999). Root = 900.</summary>
    public bool IsAdminSkill900 => Root == 900;

    /// <summary>True for either Admin skill group (roots 900 or 910).</summary>
    public bool IsAnyAdminSkill => IsAdminSkill900 || IsSuperGmSkill;

    /// <summary>True for Cygnus Noblesse beginner skills (10_000_000 – 10_009_999). Root = 1000.</summary>
    public bool IsNoblesseSkill => Root == 1_000;

    /// <summary>
    /// Client's is_unregisterd_skill flag — any 8-digit skill ID starting with 9.
    /// Covers BOTH guild (91_000_000+) AND SuperGM (9_100_000+). Visual-routing ONLY.
    /// ⚠ DO NOT use as a guild discriminator — use IsGuildSkill (Root == 9100) exclusively.
    /// </summary>
    public bool IsUnregistered => Value / 10_000_000 == 9;

    // ── Active / Passive ──────────────────────────────────────────────────

    /// <summary>
    /// True for passive skills: thousands digit of the serial equals 9.
    /// Formula: nSkillID / 1_000 % 10 == 9.
    /// </summary>
    public bool IsPassive => Value / 1_000 % 10 == 9;

    /// <summary>
    /// True for active (key-bindable, macro-eligible) skills: thousands digit of the serial in 1–8.
    /// Formula: nSkillID / 1_000 % 10 in [1, 8].
    /// </summary>
    public bool IsActive => Value / 1_000 % 10 is >= 1 and <= 8;

    /// <summary>
    /// True for "common" cross-class skills: thousands digit of the serial is 0 (serial range 0000–0999).
    /// Formula: nSkillID / 1_000 % 10 == 0.
    /// Examples: Maple Hero (serial 0004), Monster Riding (serial 0004), Aran combo starters.
    /// </summary>
    public bool IsCommon => Value / 1_000 % 10 == 0;

    /// <summary>
    /// True when this skill is owned by a category-root job (Root % 100 == 0).
    /// These skills are shared across all branches of the same lineage+category.
    /// Formula: (nSkillID / 10_000) % 100 == 0.
    /// </summary>
    public bool IsBaseClassSkill => Root % 100 == 0;

    /// <summary>
    /// True for lineage-root novice skills: Root % 1_000 == 0, or the Evan pre-advancement root (Root == 2001).
    /// These cover the Beginner, Noblesse, Aran root, Resistance root, and Evan root job skills.
    /// IsNovice implies IsBaseClassSkill (every lineage root is also a category root).
    /// </summary>
    public bool IsNovice => Root % 1_000 == 0 || Root == 2_001;

    // ── Master Level ──────────────────────────────────────────────────────

    // Transition skills exempted from master level. These are advanced-class skills
    // (root % 10 == 2) that intentionally do NOT require a mastery book —
    // they represent cross-tier "advance" or "link" skills.
    private static readonly FrozenSet<SkillTemplateId> s_ignoreMasterLevelCommon =
        FrozenSet.ToFrozenSet<SkillTemplateId>([
            new(1_120_012), // Hero
            new(1_220_013), // Paladin
            new(1_320_011), // Dark Knight
            new(2_120_009), // Arch Mage (F/P)
            new(2_220_009), // Arch Mage (I/L)
            new(2_320_010), // Bishop
            new(3_120_010), // Bowmaster
            new(3_120_011), // Bowmaster
            new(3_220_009), // Marksman
            new(3_220_010), // Marksman
            new(4_120_010), // Night Lord
            new(4_220_009), // Shadower
            new(5_120_011), // Buccaneer
            new(5_220_009), // Corsair
            new(32_120_009), // Battle Mage
            new(33_120_010), // Wild Hunter
        ]);

    /// <summary>
    /// True when this skill requires a mastery book to increase beyond level 20.
    /// General rule: root % 10 == 2 (2nd-tier branch).
    /// Special cases: Evan (only tiers 9–10 and three specific IDs), Dual Blade (only 4th job minus four specific IDs).
    /// 15–16 transition skills are explicitly exempted even though their root % 10 == 2.
    /// </summary>
    public bool NeedsMasterLevel
    {
        get
        {
            if (s_ignoreMasterLevelCommon.Contains(this))
                return false;

            int root = Root;

            // Evan: root/100 == 22 or root == 2001 (pre-advancement)
            if (root / 100 == 22 || root == 2_001)
            {
                // Only tiers 9 and 10 (Evan 8th/9th growth, jobs 2217/2218) need ML,
                // plus three specific cross-tier skill IDs that bypass tier check.
                int tier = new JobId(root).Tier;
                return tier is 9 or 10 || Value is 22_111_001 or 22_141_002 or 22_140_000;
            }

            // Dual Blade: root/10 == 43 (jobs 430–434)
            if (root / 10 == 43)
            {
                // Only 4th-job DB skills need master level, minus four exception IDs.
                if (new JobId(root).Tier != 4)
                    return false;
                return Value is not (4_341_002 or 4_341_003 or 4_341_004 or 4_341_005);
            }

            // Category-root skills (base class, no branch): never need master level.
            if (root % 100 == 0)
                return false;

            // General rule: 2nd-tier branch skills need master level.
            return root % 10 == 2;
        }
    }

    // ── Hardcoded Skill Predicates ────────────────────────────────────────

    // Key-down skills (fire continuously while key is held).
    private static readonly FrozenSet<SkillTemplateId> s_keyDownSkills = FrozenSet.ToFrozenSet<SkillTemplateId>([
        new(2_121_001), // Arch Mage (F/P) — Big Bang
        new(2_221_001), // Arch Mage (I/L) — Blizzard
        new(2_321_001), // Bishop — Genesis
        new(3_121_004), // Bowmaster — Hurricane
        new(3_221_001), // Marksman — Piercing Arrow
        new(4_341_002), // Dual Blade (4th) — Phantom Blow
        new(4_341_003), // Dual Blade (4th) — Fly Fan
        new(5_101_004), // Brawler — Energy Charge
        new(5_201_002), // Gunslinger — Recoil Shot
        new(5_221_004), // Corsair — Rapid Fire
        new(13_111_002), // Wind Breaker (Cygnus)
        new(14_111_006), // Night Walker (Cygnus)
        new(15_101_003), // Thunder Breaker (Cygnus)
        new(22_121_000), // Evan (3rd growth)
        new(22_151_001), // Evan (6th growth)
        new(33_101_005), // Wild Hunter
        new(33_121_009), // Wild Hunter (4th job)
        new(35_001_001), // Mechanic — Robot Transform (Beginner)
        new(35_101_009), // Mechanic (2nd job)
    ]);

    /// <summary>
    /// True if this skill fires continuously while the key is held.
    /// </summary>
    public bool IsKeyDown => s_keyDownSkills.Contains(this);

    // Base teleport skills used for field teleport detection.
    // These are the 1st-job (base) teleport skills, NOT the upgraded mastery versions.
    // For mastery variants (e.g. 2111002, 2211002, 2311002), see IsTeleportMastery.
    private static readonly FrozenSet<SkillTemplateId> s_teleportSkills = FrozenSet.ToFrozenSet<SkillTemplateId>([
        new(2_101_002), // Wizard (F/P) — Teleport (1st job, root 210)
        new(2_201_002), // Wizard (I/L) — Teleport (1st job, root 220)
        new(2_301_001), // Cleric — Teleport (1st job, root 230)
        new(9_001_002), // Admin — ADMIN_TELEPORT
        new(8_001_001), // Manager/GM — Teleport
        new(12_101_003), // Blaze Wizard (Cygnus, root 1210)
        new(22_101_001), // Evan (1st growth, root 2210)
        new(32_001_002), // Battle Mage (Resistance, root 3200)
    ]);

    /// <summary>
    /// True if this skill is a base teleport skill.
    /// These are the 1st-job teleport skills (Wizard/Cleric, Admin, Battle Mage, Cygnus, Evan).
    /// ⚠ Upgraded mastery-teleport variants (2111002, 2211002, etc.) are NOT in this set — use IsTeleportMastery.
    /// </summary>
    public bool IsTeleport => s_teleportSkills.Contains(this);

    // Post-teleport attack (mastery) skills — checked separately from base teleport.
    private static readonly FrozenSet<SkillTemplateId> s_teleportMasterySkills =
        FrozenSet.ToFrozenSet<SkillTemplateId>([
            new(2_111_007), // Mage (F/P) — Teleport Mastery
            new(2_211_007), // Mage (I/L) — Teleport Mastery
            new(2_311_007), // Priest — Teleport Mastery
            new(32_111_010), // Battle Mage — Teleport Mastery
        ]);

    /// <summary>
    /// True if this skill is a post-teleport attack (mastery) skill.
    /// Distinct from IsTeleport (base teleport) — these are the upgraded variants that also deal damage.
    /// </summary>
    public bool IsTeleportMastery => s_teleportMasterySkills.Contains(this);

    // Monster riding / vehicle skills.
    // Note: event vehicle skills are handled separately and identified by the last 4 digits of the skill ID.
    private static readonly FrozenSet<SkillTemplateId> s_vehicleSkills = FrozenSet.ToFrozenSet<SkillTemplateId>([
        new(1_004), // Explorer Beginner — Monster Riding (root 0, serial 1004)
        new(5_221_006), // Corsair — Battleship
        new(10_001_004), // Noblesse (Cygnus) — Monster Riding
        new(20_001_004), // Aran — Monster Riding
        new(20_011_004), // Evan — Monster Riding (root 2001)
        new(30_001_004), // Resistance Citizen — Monster Riding
        new(33_001_001), // Wild Hunter — Jaguar Riding
        new(35_001_002), // Mechanic — Mechanic (Robot transformation)
    ]);

    /// <summary>
    /// True if this skill is a vehicle/mount skill (persistent riding).
    /// Covers Monster Riding, Corsair Battleship, Wild Hunter Jaguar, and Mechanic Robot.
    /// ⚠ Event-mount skills are handled separately by <see cref="IsEventVehicleSkill"/>.
    /// </summary>
    public bool IsVehicleSkill => s_vehicleSkills.Contains(this);

    // Hero's Will (debuff-cleanse) skill IDs per class.
    // ⚠ Cygnus Knights do NOT have Hero's Will.
    private static readonly FrozenSet<SkillTemplateId> s_heroWillSkills = FrozenSet.ToFrozenSet<SkillTemplateId>([
        new(1_121_011), // Hero (112)
        new(1_221_012), // Paladin (122)
        new(1_321_010), // Dark Knight (132)
        new(2_121_008), // Arch Mage F/P (212)
        new(2_221_008), // Arch Mage I/L (222)
        new(2_321_009), // Bishop (232)
        new(3_121_009), // Bowmaster (312)
        new(3_221_008), // Marksman (322)
        new(4_121_009), // Night Lord (412)
        new(4_221_008), // Shadower (422)
        new(4_341_008), // Dual Blade (434)
        new(5_121_008), // Buccaneer (512)
        new(5_221_010), // Corsair (522)
        new(21_121_008), // Aran (2112)
        new(22_171_004), // Evan (2217, 9th growth)
        new(32_121_008), // Battle Mage (3212)
        new(33_121_008), // Wild Hunter (3312)
        new(35_121_008), // Mechanic (3512)
    ]);

    /// <summary>
    /// True if this skill is the Hero's Will (debuff-cleanse) skill for any class (18 IDs total).
    /// ⚠ Cygnus Knights have NO Hero's Will.
    /// ⚠ Dual Blade (4_341_008) and Buccaneer (5_121_008) ARE included.
    /// </summary>
    public bool IsHeroWill => s_heroWillSkills.Contains(this);

    /// <summary>
    /// True if this skill is one of Aran's five core combo attack skills:
    /// <list type="bullet">
    ///   <item>21_000_000 — ARAN_COMBO_ABILITY</item>
    ///   <item>21_000_002 — ARAN_DOUBLE_SWING</item>
    ///   <item>21_001_000 — ARAN_COMBO_ABILITY (Aran class root variant)</item>
    ///   <item>21_001_001 — ARAN_COMBAT_STEP</item>
    ///   <item>21_001_002 — ARAN_TRIPLE_SWING</item>
    /// </list>
    /// </summary>
    public bool IsAranCombo => Value is 21_000_000 or 21_000_002 or 21_001_000 or 21_001_001 or 21_001_002;

    // Battle Mage aura skills.
    private static readonly FrozenSet<SkillTemplateId> s_battleMageAuraSkills = FrozenSet.ToFrozenSet<SkillTemplateId>([
        new(32_001_003), // Dark Aura (root 3200)
        new(32_101_001), // Battle Mage — Aura variant 1
        new(32_101_002), // Battle Mage — Aura variant 2
        new(32_101_003), // Battle Mage — Aura variant 3
        new(32_110_000), // Battle Mage — Aura (branch-root 3211)
        new(32_120_000), // Battle Mage — Aura (branch-root 3212)
        new(32_120_001), // Battle Mage — Aura variant (3212)
    ]);

    /// <summary>
    /// True if this skill is a Battle Mage aura skill (is_bmage_aura_skill).
    /// Auras provide persistent party buffs and have special handling in combat/move code.
    /// </summary>
    public bool IsBattleMageAura => s_battleMageAuraSkills.Contains(this);

    // Non-slot skills — skills that do not occupy a skill slot.
    // Primarily Monster Riding skills and the Wild Hunter jaguar passive.
    private static readonly FrozenSet<SkillTemplateId> s_nonSlotSkills = FrozenSet.ToFrozenSet<SkillTemplateId>([
        new(1_066), // Explorer Beginner — Maple Warrior / riding passive
        new(1_067), // Explorer Beginner — riding passive variant
        new(4_321_000), // Chief Bandit — non-slot skill
        new(10_001_066), // Noblesse — non-slot riding passive
        new(10_001_067), // Noblesse — non-slot riding passive variant
        new(20_001_066), // Aran — non-slot riding passive
        new(20_001_067), // Aran — non-slot riding passive variant
        new(20_011_066), // Evan — non-slot riding passive
        new(20_011_067), // Evan — non-slot riding passive variant
        new(30_001_066), // Resistance Citizen — non-slot riding passive
        new(30_001_067), // Resistance Citizen — non-slot riding passive variant
        new(33_001_002), // Wild Hunter — Jaguar passive (non-slot)
    ]);

    /// <summary>
    /// True if this skill does not occupy a skill slot (is_nonslot_skill).
    /// These are passive riding/mount skills and one Wild Hunter jaguar skill that bypass slot assignment.
    /// </summary>
    public bool IsNonSlot => s_nonSlotSkills.Contains(this);

    // Prepare-bomb / charging skills.
    // These skills have a charge-up phase before the attack triggers.
    private static readonly FrozenSet<SkillTemplateId> s_prepareBombSkills = FrozenSet.ToFrozenSet<SkillTemplateId>([
        new(4_341_003), // Dual Blade (4th) — Fly Fan
        new(5_201_002), // Gunslinger — Recoil Shot
        new(14_111_006), // Night Walker (Cygnus)
    ]);

    /// <summary>
    /// True if this skill uses a charge-up (prepare-bomb) attack phase.
    /// </summary>
    public bool IsPrepareBomb => s_prepareBombSkills.Contains(this);

    // Event vehicle skills — seasonal/event mount skills identified by their serial (last 4 digits of the skill ID).
    // Any skill whose Value % 10000 matches one of these 28 serials is an event vehicle skill.
    // Unlike s_vehicleSkills (which matches full IDs), this set matches cross-job by serial only.
    private static readonly FrozenSet<int> s_eventVehicleSerials = FrozenSet.ToFrozenSet<int>([
        1025,
        1027,
        1028,
        1029,
        1030,
        1031,
        1033,
        1034,
        1035,
        1036,
        1037,
        1038,
        1039,
        1040,
        1042,
        1044,
        1049,
        1050,
        1051,
        1052,
        1053,
        1054,
        1063,
        1064,
        1065,
        1069,
        1070,
        1071,
    ]);

    /// <summary>
    /// True if this is a seasonal/event vehicle (mount) skill, classified by serial number.
    /// Unlike <see cref="IsVehicleSkill"/> (which checks the full skill ID against 8 known IDs),
    /// this checks the last 4 digits (<c>Value % 10000</c>) against 28 known event-mount serials.
    /// Event mounts can appear under any job prefix; only the serial is tested.
    /// </summary>
    public bool IsEventVehicleSkill => s_eventVehicleSerials.Contains(Value % 10_000);

    // ── Owner Job Context ─────────────────────────────────────────────────

    /// <summary>
    /// True when the owning job uses the ExtendSP system (per-skill SP allocation).
    /// Applies to all Resistance jobs (lineage 3) and all Evan jobs (root/100==22 or root==2001).
    /// Formula: <c>new JobId(Root).IsExtendSP</c>.
    /// </summary>
    public bool IsExtendSPOwner => new JobId(Root).IsExtendSP;

    /// <summary>
    /// "Degree" of the owning job root for <c>CJobClassifier</c> mastery-book slot assignment.
    /// <list type="table">
    ///   <item><term>0</term><description>Invalid (root == 0) or specific-tier job (root % 10 != 0 and root % 100 != 0)</description></item>
    ///   <item><term>1</term><description>Category-root skill (root % 100 == 0, e.g. roots 100, 200, 3200)</description></item>
    ///   <item><term>2</term><description>Branch-root skill (root % 10 == 0 but % 100 != 0, e.g. roots 110, 120, 3210)</description></item>
    /// </list>
    /// </summary>
    public int OwnerJobDegree
    {
        get
        {
            int root = Root;
            if (root == 0)
                return 0;
            if (root % 100 != 0)
                return root % 10 != 0 ? 0 : 2;
            return 1;
        }
    }

    /// <summary>
    /// True if a character with the given job can use this skill (root applicability check).
    /// Delegates to <see cref="JobId.CanUseSkillRoot"/>.
    /// </summary>
    public bool CanBeUsedByJob(JobId job) => job.CanUseSkillRoot(Root);

    // ── Admin Skill Constants ──────────────────────────────────────────────

    /// <summary>
    /// Hardcoded Admin/SuperGM skill ID constants.
    /// Root 900 → image "900.img" (Admin group 1: Haste, Dragon Roar, Teleport, AntiMacro).
    /// Root 910 → image "910.img" (Admin group 2 / SuperGM: Dispel, SuperHaste, HolySymbol, Bless, Hide, Resurrection, HyperBody).
    /// ⚠ Root 9100 ("9100.img") is completely different — that is the guild skill root.
    /// </summary>
    public static class Admin
    {
        // Root 900 — Admin group 1
        public static readonly SkillTemplateId Haste = new(9_001_000);
        public static readonly SkillTemplateId DragonRoar = new(9_001_001);
        public static readonly SkillTemplateId Teleport = new(9_001_002);
        public static readonly SkillTemplateId AntiMacro = new(9_001_009);

        // Root 910 — Admin group 2 (SuperGM)
        public static readonly SkillTemplateId Dispel = new(9_101_000);
        public static readonly SkillTemplateId SuperHaste = new(9_101_001);
        public static readonly SkillTemplateId HolySymbol = new(9_101_002);
        public static readonly SkillTemplateId Bless = new(9_101_003);
        public static readonly SkillTemplateId Hide = new(9_101_004);
        public static readonly SkillTemplateId Resurrection = new(9_101_005);
        public static readonly SkillTemplateId HyperBody = new(9_101_008);
    }

    // ── Guild Skill Constants ──────────────────────────────────────────────

    /// <summary>
    /// Hardcoded guild passive skill ID constants.
    /// All IDs: Root = 9100, image = "9100.img", range 91_000_000 – 91_000_006.
    /// NOTE: Code is present in client though the data become available much later!
    /// </summary>
    public static class Guild
    {
        public static readonly SkillTemplateId MesoUp = new(91_000_000);
        public static readonly SkillTemplateId ExperienceUp = new(91_000_001);
        public static readonly SkillTemplateId DefenceUp = new(91_000_002);

        /// <summary>Attack and Magic Attack Up. Constant name: GUILD_ATTNMAGUP.</summary>
        public static readonly SkillTemplateId AttMagUp = new(91_000_003);
        public static readonly SkillTemplateId AgilityUp = new(91_000_004);

        /// <summary>Note: "BUSINESSEFFICENYUP" has a confirmed typo (missing 'I').</summary>
        public static readonly SkillTemplateId BusinessEfficacyUp = new(91_000_005);
        public static readonly SkillTemplateId RegularSupport = new(91_000_006);
    }
}
