using System.Collections.Frozen;
using System.Diagnostics;

namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for a job ID (3–4 digit codes).
/// </summary>
[StrongIntId]
public readonly partial record struct JobId(int Value)
{
    // ── Digit Decomposition ───────────────────────────────────────────────

    /// <summary>
    /// Thousands digit. 0=Explorer, 1=Cygnus, 2=Aran/Evan, 3=Resistance.
    /// </summary>
    public int Lineage => Value / 1_000;

    /// <summary>
    /// Hundreds-within-lineage. 1=Warrior, 2=Mage, 3=Archer, 4=Thief, 5=Pirate, 0=Beginner.
    /// </summary>
    public int Category => Value % 1_000 / 100;

    /// <summary>Tens-within-hundred. Which branch within the category.</summary>
    public int Branch => Value % 100 / 10;

    /// <summary>Units digit. Raw advancement digit within the branch.</summary>
    public int TierDigit => Value % 10;

    /// <summary>
    /// Full job-category code preserving the lineage prefix: <c>nJob / 100</c>.
    /// Unlike <see cref="Category"/> (which strips lineage via <c>% 1000 / 100</c>), this value
    /// retains the thousands prefix, matching the <c>nJobCategory</c> stored in WZ field conditions.
    /// <list type="table">
    ///   <item><term>Hero (112)</term><description>1</description></item>
    ///   <item><term>Blaze Wizard (1212)</term><description>12</description></item>
    ///   <item><term>Aran (2112)</term><description>21</description></item>
    ///   <item><term>Battle Mage (3212)</term><description>32</description></item>
    /// </list>
    /// </summary>
    public int FieldCategoryCode => Value / 100;

    // ── Lineage Predicates ────────────────────────────────────────────────

    /// <summary>Jobs 0–999 (lineage=0).</summary>
    public bool IsExplorer => Value / 1_000 == 0;

    /// <summary>Cygnus Knights: jobs 1000–1999</summary>
    public bool IsCygnus => Value / 1_000 == 1;

    /// <summary>Aran: jobs 2100–2112 + root 2000</summary>
    public bool IsAran => Value / 100 == 21 || Value == 2_000;

    /// <summary>Evan: jobs 2200–2218 + root 2001</summary>
    public bool IsEvan => Value / 100 == 22 || Value == 2_001;

    /// <summary>
    /// MapleHero lineage (Aran + Evan combined) — NOT "reached 3rd job tier".
    /// The name is misleading; it means lineage == 2.
    /// </summary>
    public bool IsMapleHero => Value / 1_000 == 2;

    /// <summary>Dual Blade: job IDs 430–439</summary>
    public bool IsDualBlade => Value / 10 == 43;

    /// <summary>Resistance: jobs 3000–3999</summary>
    public bool IsResistance => Value / 1_000 == 3;

    // ── Resistance Subclass ───────────────────────────────────────────────

    /// <summary>Battle Mage (3200–3299)</summary>
    public bool IsBattleMage => Value / 100 == 32;

    /// <summary>Wild Hunter (3300–3399)</summary>
    public bool IsWildHunter => Value / 100 == 33;

    /// <summary>Mechanic (3500–3599)</summary>
    public bool IsMechanic => Value / 100 == 35;

    /// <summary>
    /// True for all mage-type jobs regardless of lineage:
    /// Explorer Mage (200–299), Blaze Wizard (1200–1299), Evan (2200–2299), Battle Mage (3200–3299).
    /// Note: the underlying is_mage_job function takes nJob/100 — this property computes it internally.
    /// </summary>
    public bool IsMage => Value / 100 is 2 or 12 or 22 or 32;

    // ── Extended Predicates ───────────────────────────────────────────────

    /// <summary>
    /// Resistance + Evan extended SP system.
    /// </summary>
    public bool IsExtendSP =>
        Value / 1_000 == 3 // All Resistance
        || Value / 100 == 22 // Evan class jobs
        || Value == 2_001; // Evan root (pre-advancement)

    /// <summary>Admin/GM jobs (category digit 9)</summary>
    public bool IsAdmin => Value % 1_000 / 100 == 9;

    /// <summary>Manager/GM jobs (category digit 8)</summary>
    public bool IsManager => Value % 1_000 / 100 == 8;

    /// <summary>
    /// Beginner: nJob % 1000 == 0, OR Evan's pre-advancement root 2001.
    /// </summary>
    public bool IsBeginner => Value % 1_000 == 0 || Value == 2_001;

    // ── Advancement Tier ──────────────────────────────────────────────────

    /// <summary>
    /// Advancement tier: 1=base, 2=2nd job, 3=3rd job, 4=4th job.
    /// Evan can reach 10. Dual Blade uses a 2-step pairing formula.
    /// Returns 0 for invalid input.
    /// </summary>
    public int Tier
    {
        get
        {
            if (Value % 100 == 0 || Value == 2_001)
                return 1;
            // Dual Blade pairing: IDs 430–434 pair adjacent jobs into the same tier.
            // (430-430)/2=0, (431-430)/2=0, (432-430)/2=1, (433-430)/2=1, (434-430)/2=2.
            // Adding 2 yields tiers 2, 2, 3, 3, 4 — matching the game client's advancement steps.
            int v1 = (Value / 10 == 43) ? (Value - 430) / 2 : Value % 10;
            int v2 = v1 + 2;
            if (v2 >= 2 && (v2 <= 4 || (v2 <= 10 && IsEvan)))
                return v2;
            return 0;
        }
    }

    // ── Skill Roots ───────────────────────────────────────────────────────

    /// <summary>Maximum number of skill roots any job can have (Evan 10th growth: 2200, 2210, 2211–2218).</summary>
    public const int MaxSkillRoots = 10;

    /// <summary>
    /// Writes all skill roots accessible by this job (accumulated across all tiers)
    /// into <paramref name="destination"/> and returns the number written.
    /// </summary>
    /// <param name="destination">
    /// Buffer to receive the skill root IDs. Must have at least <see cref="MaxSkillRoots"/> elements.
    /// </param>
    /// <returns>Number of roots written.</returns>
    public int GetSkillRoots(Span<int> destination)
    {
        Debug.Assert(
            destination.Length >= MaxSkillRoots,
            $"destination must have at least {MaxSkillRoots} elements; got {destination.Length}"
        );
        if (Value == 0)
            return 0;
        int cat = Category;
        if (cat == 0)
        {
            // Lineage-root jobs (Noblesse=1000, Aran=2000, Evan-root=2001, Citizen=3000)
            // have exactly one skill root equal to their own job ID.
            // Explorer Beginner (Value=0) was already handled above.
            destination[0] = Value;
            return 1;
        }
        int count = 0;
        int categoryRoot = 100 * (cat + (10 * Lineage));
        destination[count++] = categoryRoot;
        int branch = Branch;
        if (branch == 0)
            return count;
        int branchRoot = categoryRoot + (10 * branch);
        destination[count++] = branchRoot;
        for (int i = 1; i <= 8; i++)
        {
            if (TierDigit < i)
                break;
            destination[count++] = ++branchRoot;
        }
        return count;
    }

    /// <summary>
    /// True if a player with this job can use a skill with the given root.
    /// </summary>
    public bool CanUseSkillRoot(int nSkillRoot)
    {
        if (nSkillRoot % 100 == 0)
            return nSkillRoot / 100 == Value / 100;
        return nSkillRoot / 10 == Value / 10 && Value % 10 >= nSkillRoot % 10;
    }

    // ── reqJob Bitmask ────────────────────────────────────────────────────

    /// <summary>Bitmask value 0 = all jobs allowed</summary>
    public const int ReqJobAll = 0;

    /// <summary>Bitmask value -1 = beginner only</summary>
    public const int ReqJobBeginner = -1;

    /// <summary>
    /// Equipment reqJob bitmask bit for this job (1&lt;&lt;(category-1), or 0 for beginners).
    /// </summary>
    public int EquipBit => Category > 0 ? (1 << (Category - 1)) : 0;

    // ── Dual Blade Birth ──────────────────────────────────────────────────

    /// <summary>
    /// True if a character with this job code was born as a Dual Blade (nSubJob == 1),
    /// rather than job-advancing from a Thief.
    /// Only Explorer class (lineage 0) can be born Dual Blade.
    /// </summary>
    public bool IsDualBladeBorn(short nSubJob) => Value / 1_000 == 0 && nSubJob == 1;

    // ── HP / MP Per Level ─────────────────────────────────────────────────

    /// <summary>
    /// HP gained per level-up roll for this job class (client-side stat-transfer value).
    /// ⚠ This is the roll value added to the UI stat-transfer display.
    ///   The server's final HP grant may additionally apply MaxHP-Up passive skill bonuses.
    /// Battle Mage (nJob/100==32) is a Mage-category job that shares the Warrior HP roll (20).
    /// </summary>
    public int HpPerLevel =>
        Category switch
        {
            0 => 8, // Beginner
            1 => 20, // Warrior
            2 => Value / 100 == 32 ? 20 // Battle Mage
            : IsEvan ? 12 // Evan
            : 6, // Mage
            3 or 4 => 16, // Bowman / Thief
            5 => 18, // Pirate
            _ => 0,
        };

    /// <summary>
    /// MP gained per level-up roll for this job class.
    /// ⚠ For Mages (category 2), the MP decrease on stat-transfer uses a separate formula:
    ///   result = 3 × INT / 40 + 30.
    /// </summary>
    public int MpPerLevel =>
        Category switch
        {
            0 => 6, // Beginner
            1 => 2, // Warrior
            2 => 18, // Mage (all mages, including Battle Mage)
            3 or 4 => 10, // Bowman / Thief
            5 => 14, // Pirate
            _ => 0,
        };

    // ── Advancement Level Thresholds ─────────────────────────────────────

    /// <summary>
    /// Level required to advance to the given job step (1–4).
    /// Resistance and Evan always return 200 (their SP system uses a single fixed threshold).
    /// Explorer Mage advances at 8 (step 1); all others at 10.
    /// Dual Blade born-thief advances at 20/55 instead of 30/70 for steps 2/3.
    /// </summary>
    public int GetAdvanceLevel(short nSubJob, int nStep)
    {
        int lineage = Lineage;
        // Resistance and Evan use a flat 200 threshold (their SP system requires max level)
        if (lineage == 3 || Value / 100 == 22 || Value == 2_001)
            return 200;
        return nStep switch
        {
            1 => lineage == 0 && Category == 2 ? 8 : 10, // Explorer Mage: 8, all others: 10
            2 => IsDualBladeBorn(nSubJob) ? 20 : 30,
            3 => IsDualBladeBorn(nSubJob) ? 55 : 70,
            4 => 120,
            _ => 200,
        };
    }

    /// <summary>
    /// Level required for a Dual Blade job advancement, keyed by the actual job ID (not a step number).
    /// Only valid for Dual Blade job IDs: 400 (Thief root), 430–434.
    /// Returns 200 for any other job ID.
    /// </summary>
    public int DualJobChangeLevel =>
        Value switch
        {
            400 => 10, // Thief class root (born-DB pre-advancement)
            430 => 20, // DB 1st job
            431 => 30, // DB 2nd job
            432 => 55, // DB 3rd job
            433 => 70, // DB 3rd job (advanced step)
            434 => 120, // DB 4th job
            _ => 200,
        };

    /// <summary>True if an equip with the given nrJob bitmask can be worn by this job.</summary>
    public bool CanEquip(int nrJob)
    {
        if (nrJob == ReqJobAll)
            return true;
        int bit = EquipBit;
        if (nrJob == ReqJobBeginner)
            return bit == 0;
        return (nrJob & bit) != 0;
    }

    /// <summary>Maximum number of names that <see cref="DecodeReqJob"/> can write.</summary>
    public const int MaxReqJobNames = 5;

    /// <summary>
    /// Writes human-readable job category names for an nrJob bitmask into <paramref name="destination"/>
    /// and returns the number written.
    /// </summary>
    public static int DecodeReqJob(int nrJob, Span<string> destination)
    {
        if (nrJob == ReqJobAll)
        {
            destination[0] = "All jobs";
            return 1;
        }
        if (nrJob == ReqJobBeginner)
        {
            destination[0] = "Beginner only";
            return 1;
        }
        int count = 0;
        if ((nrJob & 0x01) != 0)
            destination[count++] = "Warrior";
        if ((nrJob & 0x02) != 0)
            destination[count++] = "Magician";
        if ((nrJob & 0x04) != 0)
            destination[count++] = "Archer";
        if ((nrJob & 0x08) != 0)
            destination[count++] = "Thief";
        if ((nrJob & 0x10) != 0)
            destination[count++] = "Pirate";
        return count;
    }

    // ── Mu Lung Dojo Mastery Ticket Gate ─────────────────────────────────

    /// <summary>
    /// Returns <see langword="true"/> if this job is allowed to use the given Mu Lung Dojo mastery
    /// ticket item, <see langword="false"/> if it is blocked.
    /// <para>
    /// Evan jobs (<c>nJob/100 == 22</c> or root <c>2001</c>) are blocked from non-Evan tickets
    /// (items 5050001–5050004); all other jobs are blocked from Evan tickets (5050005–5050009).
    /// </para>
    /// </summary>
    public bool IsMatchedForMuLungItem(ItemTemplateId item)
    {
        if (Value / 100 == 22 || Value == 2_001)
            // Evan: block items 5050001–5050004 (unsigned subtraction range trick from C source)
            return (uint)(item.Value - 5_050_001) > 3;
        // non-Evan: block items 5050005–5050009
        return (uint)(item.Value - 5_050_005) > 4;
    }

    // ── UI Icon Path ──────────────────────────────────────────────────────

    // Pre-computed icon paths for categories 0–9, online states 0–1.
    // Index: category * 2 + (bOnline ? 1 : 0).
    private static readonly string[] s_jobIconPaths = InitJobIconPaths();

    private static string[] InitJobIconPaths()
    {
        var paths = new string[20];
        for (int cat = 0; cat < 10; cat++)
        {
            paths[cat * 2] = $"UI/UIWindow2.img/UserList/Main/Expedition/icon{cat}/0";
            paths[(cat * 2) + 1] = $"UI/UIWindow2.img/UserList/Main/Expedition/icon{cat}/1";
        }
        return paths;
    }

    /// <summary>
    /// WZ UI path for this job's icon in the Party/Expedition member list (UIWindow2).
    /// Uses <see cref="Category"/> (strips lineage) — the same 5 category icons are shared across
    /// all lineages. Beginner (category 0) has no icon at this path.
    /// <para><c>bOnline</c>: 0 = offline, 1 = online.</para>
    /// </summary>
    public string GetJobIconPath(bool bOnline)
    {
        int idx = (Category * 2) + (bOnline ? 1 : 0);
        // s_jobIconPaths covers categories 0–9 (indices 0–19); invalid input returns empty.
        return (uint)idx < (uint)s_jobIconPaths.Length ? s_jobIconPaths[idx] : string.Empty;
    }

    // ── Valid Job ID Registry ─────────────────────────────────────────────

    // Complete set of valid job IDs.
    // Any ID not in this set is considered invalid/unrecognized.
    private static readonly FrozenSet<JobId> s_validJobIds = FrozenSet.ToFrozenSet<JobId>([
        // Explorer — Beginner
        new(0),
        // Explorer — Warrior branch
        new(100),
        new(110),
        new(111),
        new(112), // Warrior → Fighter → Crusader → Hero
        new(120),
        new(121),
        new(122), // Page → White Knight → Paladin
        new(130),
        new(131),
        new(132), // Spearman → Dragon Knight → Dark Knight
        // Explorer — Mage branch (advances at level 8, not 10)
        new(200),
        new(210),
        new(211),
        new(212), // Magician → Wizard F/P → Mage F/P → Arch Mage F/P
        new(220),
        new(221),
        new(222), // Wizard I/L → Mage I/L → Arch Mage I/L
        new(230),
        new(231),
        new(232), // Cleric → Priest → Bishop
        // Explorer — Archer branch
        new(300),
        new(310),
        new(311),
        new(312), // Archer → Hunter → Ranger → Bowmaster
        new(320),
        new(321),
        new(322), // Crossbowman → Sniper → Marksman
        // Explorer — Thief branch
        new(400),
        new(410),
        new(411),
        new(412), // Rogue → Assassin → Hermit → Night Lord
        new(420),
        new(421),
        new(422), // Bandit → Chief Bandit → Shadower
        // Explorer — Dual Blade (sub-branch of Thief)
        new(430),
        new(431),
        new(432),
        new(433),
        new(434),
        // Explorer — Pirate branch
        new(500),
        new(510),
        new(511),
        new(512), // Pirate → Brawler → Marauder → Buccaneer
        new(520),
        new(521),
        new(522), // Gunslinger → Outlaw → Corsair
        // GM / Admin
        new(800), // GM Manager (is_manager_job)
        new(900),
        new(910),
        new(920), // SuperGM / Admin (is_admin_job)
        // Cygnus Knights — root
        new(1000), // Noblesse
        // Cygnus — Soul Master (Warrior)
        new(1100),
        new(1110),
        new(1111),
        new(1112),
        // Cygnus — Blaze Wizard (Mage)
        new(1200),
        new(1210),
        new(1211),
        new(1212),
        // Cygnus — Wind Breaker (Archer)
        new(1300),
        new(1310),
        new(1311),
        new(1312),
        // Cygnus — Night Walker (Thief)
        new(1400),
        new(1410),
        new(1411),
        new(1412),
        // Cygnus — Thunder Breaker (Pirate)
        new(1500),
        new(1510),
        new(1511),
        new(1512),
        // MapleHero — Aran
        new(2000), // Aran root (is_aran_job)
        new(2100),
        new(2110),
        new(2111),
        new(2112),
        // MapleHero — Evan
        new(2001), // Evan pre-advancement root (is_beginner_job + is_evan_job)
        new(2200),
        new(2210),
        new(2211),
        new(2212),
        new(2213),
        new(2214),
        new(2215),
        new(2216),
        new(2217),
        new(2218),
        // Resistance — root
        new(3000), // Citizen
        // Resistance — Battle Mage
        new(3200),
        new(3210),
        new(3211),
        new(3212),
        // Resistance — Wild Hunter
        new(3300),
        new(3310),
        new(3311),
        new(3312),
        // Resistance — Mechanic
        new(3500),
        new(3510),
        new(3511),
        new(3512),
        // NOTE: 3100–3199 (no Resistance warrior) and 3400–3499 (no Resistance thief) are ABSENT.
    ]);

    /// <summary>True if this job ID is a recognized valid job.</summary>
    public bool IsValidJob => s_validJobIds.Contains(this);

    /// <summary>All valid job IDs.</summary>
    public static FrozenSet<JobId> AllValidJobIds => s_validJobIds;
}
