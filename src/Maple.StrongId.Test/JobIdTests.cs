namespace Maple.StrongId.Test;

public sealed class JobIdTests
{
    // ── Digit Decomposition ───────────────────────────────────────────────

    [Test]
    [Arguments(112, 0, 1, 1, 2)] // Hero: lineage 0, category 1, branch 1, tierDigit 2
    [Arguments(2218, 2, 2, 1, 8)] // Evan 10th growth: lineage 2, category 2, branch 1, tierDigit 8
    [Arguments(3212, 3, 2, 1, 2)] // Battle Mage 4th: lineage 3, category 2, branch 1, tierDigit 2
    [Arguments(434, 0, 4, 3, 4)] // Dual Blade 4th: lineage 0, category 4, branch 3, tierDigit 4
    public async Task Decomposition_MatchesExpected(int job, int lineage, int category, int branch, int tierDigit)
    {
        var id = new JobId(job);
        await Assert.That(id.Lineage).IsEqualTo(lineage);
        await Assert.That(id.Category).IsEqualTo(category);
        await Assert.That(id.Branch).IsEqualTo(branch);
        await Assert.That(id.TierDigit).IsEqualTo(tierDigit);
    }

    // ── Tier ──────────────────────────────────────────────────────────────

    [Test]
    [Arguments(100, 1)] // Explorer Warrior root → tier 1 (category root: % 100 == 0)
    [Arguments(110, 2)] // Fighter branch root → tier 2 (v1=0, v2=2)
    [Arguments(111, 3)] // Crusader → tier 3
    [Arguments(112, 4)] // Hero → tier 4
    [Arguments(2001, 1)] // Evan pre-advancement root → tier 1
    [Arguments(2210, 2)] // Evan branch root → tier 2 (v1=0, v2=2)
    [Arguments(2211, 3)] // Evan 2nd growth → tier 3
    [Arguments(2218, 10)] // Evan 10th growth → tier 10
    [Arguments(430, 2)] // Dual Blade 1st → tier 2
    [Arguments(431, 2)] // Dual Blade 2nd (paired with 430) → tier 2
    [Arguments(432, 3)] // Dual Blade 3rd → tier 3
    [Arguments(433, 3)] // Dual Blade 3rd advanced (paired with 432) → tier 3
    [Arguments(434, 4)] // Dual Blade 4th → tier 4
    [Arguments(999, 0)] // Invalid → tier 0
    public async Task Tier_MatchesExpected(int job, int expectedTier)
    {
        await Assert.That(new JobId(job).Tier).IsEqualTo(expectedTier);
    }

    // ── GetSkillRoots ─────────────────────────────────────────────────────

    [Test]
    public async Task GetSkillRoots_ForExplorerBeginner_ReturnsEmpty()
    {
        int[] buf = new int[JobId.MaxSkillRoots];
        int count = new JobId(0).GetSkillRoots(buf);
        await Assert.That(count).IsEqualTo(0);
    }

    [Test]
    [Arguments(1000)] // Noblesse
    [Arguments(2000)] // Aran root
    [Arguments(2001)] // Evan pre-advancement
    [Arguments(3000)] // Resistance Citizen
    public async Task GetSkillRoots_ForLineageRootJob_ReturnsSingletonWithOwnId(int job)
    {
        int[] buf = new int[JobId.MaxSkillRoots];
        int count = new JobId(job).GetSkillRoots(buf);
        await Assert.That(count).IsEqualTo(1);
        await Assert.That(buf[0]).IsEqualTo(job);
    }

    [Test]
    public async Task GetSkillRoots_ForHero_IncludesAllAncestorRoots()
    {
        // Hero (112): category root 100, branch root 110, tiers 111, 112
        int[] buf = new int[JobId.MaxSkillRoots];
        int count = new JobId(112).GetSkillRoots(buf);
        int[] roots = buf[..count];
        await Assert.That(roots).Contains(100);
        await Assert.That(roots).Contains(110);
        await Assert.That(roots).Contains(111);
        await Assert.That(roots).Contains(112);
        await Assert.That(count).IsEqualTo(4);
    }

    [Test]
    public async Task GetSkillRoots_ForEvan9thGrowth_IncludesAllGrowthRoots()
    {
        // Evan 2217 (9th growth): category 2200, branch 2210, tiers 2211..2217
        int[] buf = new int[JobId.MaxSkillRoots];
        int count = new JobId(2217).GetSkillRoots(buf);
        int[] roots = buf[..count];
        await Assert.That(roots).Contains(2200);
        await Assert.That(roots).Contains(2210);
        for (int t = 2211; t <= 2217; t++)
            await Assert.That(roots).Contains(t);
        await Assert.That(count).IsEqualTo(9); // 2200 + 2210 + 2211..2217
    }

    [Test]
    public async Task GetSkillRoots_ForDualBlade4th_IncludesFullBranch()
    {
        // Dual Blade 434: category 400, branch 430, tiers 431..434
        int[] buf = new int[JobId.MaxSkillRoots];
        int count = new JobId(434).GetSkillRoots(buf);
        int[] roots = buf[..count];
        await Assert.That(roots).Contains(400);
        await Assert.That(roots).Contains(430);
        await Assert.That(roots).Contains(431);
        await Assert.That(roots).Contains(432);
        await Assert.That(roots).Contains(433);
        await Assert.That(roots).Contains(434);
        await Assert.That(count).IsEqualTo(6);
    }

    // ── GetAdvanceLevel ───────────────────────────────────────────────────

    [Test]
    [Arguments(100, (short)0, 1, 10)] // Explorer Warrior, step 1 → 10
    [Arguments(200, (short)0, 1, 8)] // Explorer Mage, step 1 → 8 (special case)
    [Arguments(300, (short)0, 2, 30)] // Explorer Archer, step 2 → 30
    [Arguments(400, (short)1, 2, 20)] // Dual Blade born, step 2 → 20
    [Arguments(400, (short)0, 2, 30)] // Non-DB Thief, step 2 → 30
    [Arguments(400, (short)1, 3, 55)] // Dual Blade born, step 3 → 55
    [Arguments(400, (short)0, 4, 120)] // Any, step 4 → 120
    [Arguments(3200, (short)0, 1, 200)] // Resistance → always 200
    [Arguments(2210, (short)0, 1, 200)] // Evan → always 200
    [Arguments(2001, (short)0, 1, 200)] // Evan root → always 200
    public async Task GetAdvanceLevel_MatchesExpected(int job, short subJob, int step, int expected)
    {
        await Assert.That(new JobId(job).GetAdvanceLevel(subJob, step)).IsEqualTo(expected);
    }

    // ── HP / MP Per Level ─────────────────────────────────────────────────

    [Test]
    [Arguments(100, 20)] // Warrior
    [Arguments(200, 6)] // Mage
    [Arguments(3200, 20)] // Battle Mage uses Warrior HP roll despite Mage category
    [Arguments(2210, 12)] // Evan: 12
    [Arguments(300, 16)] // Archer
    [Arguments(400, 16)] // Thief
    [Arguments(500, 18)] // Pirate
    [Arguments(0, 8)] // Beginner
    public async Task HpPerLevel_MatchesExpected(int job, int expected)
    {
        await Assert.That(new JobId(job).HpPerLevel).IsEqualTo(expected);
    }

    // ── CanEquip ──────────────────────────────────────────────────────────

    [Test]
    public async Task CanEquip_WithReqJobAll_AlwaysTrue()
    {
        await Assert.That(new JobId(112).CanEquip(JobId.ReqJobAll)).IsTrue();
        await Assert.That(new JobId(0).CanEquip(JobId.ReqJobAll)).IsTrue();
    }

    [Test]
    public async Task CanEquip_BeginnerOnly_OnlyBeginner()
    {
        await Assert.That(new JobId(0).CanEquip(JobId.ReqJobBeginner)).IsTrue();
        await Assert.That(new JobId(100).CanEquip(JobId.ReqJobBeginner)).IsFalse();
        await Assert.That(new JobId(112).CanEquip(JobId.ReqJobBeginner)).IsFalse();
    }

    [Test]
    public async Task CanEquip_WarriorBit_OnlyWarriors()
    {
        int warriorBit = 0x01;
        await Assert.That(new JobId(112).CanEquip(warriorBit)).IsTrue(); // Hero
        await Assert.That(new JobId(222).CanEquip(warriorBit)).IsFalse(); // Arch Mage
    }

    // ── Lineage Predicates ────────────────────────────────────────────────

    [Test]
    public async Task IsEvan_MatchesJobRange()
    {
        await Assert.That(new JobId(2001).IsEvan).IsTrue(); // Evan root
        await Assert.That(new JobId(2210).IsEvan).IsTrue(); // 1st growth
        await Assert.That(new JobId(2218).IsEvan).IsTrue(); // 10th growth
        await Assert.That(new JobId(2110).IsEvan).IsFalse(); // Aran
        await Assert.That(new JobId(112).IsEvan).IsFalse(); // Explorer
    }

    [Test]
    public async Task IsDualBlade_MatchesJobRange()
    {
        await Assert.That(new JobId(430).IsDualBlade).IsTrue();
        await Assert.That(new JobId(434).IsDualBlade).IsTrue();
        await Assert.That(new JobId(420).IsDualBlade).IsFalse(); // regular Bandit
    }

    // ── IsMatchedForMuLungItem ────────────────────────────────────────────

    [Test]
    public async Task IsMatchedForMuLungItem_Evan_BlockedByNonEvanTickets()
    {
        var evan = new JobId(2215);
        await Assert.That(evan.IsMatchedForMuLungItem(new ItemTemplateId(5_050_001))).IsFalse();
        await Assert.That(evan.IsMatchedForMuLungItem(new ItemTemplateId(5_050_004))).IsFalse();
        await Assert.That(evan.IsMatchedForMuLungItem(new ItemTemplateId(5_050_005))).IsTrue(); // Evan ticket
    }

    [Test]
    public async Task IsMatchedForMuLungItem_NonEvan_BlockedByEvanTickets()
    {
        var hero = new JobId(112);
        await Assert.That(hero.IsMatchedForMuLungItem(new ItemTemplateId(5_050_001))).IsTrue(); // non-Evan ticket
        await Assert.That(hero.IsMatchedForMuLungItem(new ItemTemplateId(5_050_005))).IsFalse(); // Evan ticket
        await Assert.That(hero.IsMatchedForMuLungItem(new ItemTemplateId(5_050_009))).IsFalse();
    }

    // ── Valid Job Registry ────────────────────────────────────────────────

    [Test]
    public async Task IsValidJob_KnownValidJobs_ReturnsTrue()
    {
        foreach (int id in JobId.AllValidJobIds)
            await Assert.That(new JobId(id).IsValidJob).IsTrue();
    }

    [Test]
    public async Task IsValidJob_InvalidJobs_ReturnsFalse()
    {
        await Assert.That(new JobId(999).IsValidJob).IsFalse();
        await Assert.That(new JobId(150).IsValidJob).IsFalse();
        await Assert.That(new JobId(3100).IsValidJob).IsFalse(); // no Resistance warrior
    }

    // ── IsBeginner ────────────────────────────────────────────────────────

    [Test]
    [Arguments(0, true)] // Explorer Beginner
    [Arguments(1000, true)] // Noblesse
    [Arguments(2000, true)] // Aran root
    [Arguments(2001, true)] // Evan pre-advancement root
    [Arguments(3000, true)] // Resistance Citizen
    [Arguments(100, false)] // Explorer Warrior
    [Arguments(112, false)] // Hero
    public async Task IsBeginner_MatchesExpected(int job, bool expected)
    {
        await Assert.That(new JobId(job).IsBeginner).IsEqualTo(expected);
    }

    // ── CanUseSkillRoot ───────────────────────────────────────────────────

    [Test]
    public async Task CanUseSkillRoot_CategoryRoot_MatchesSameCategory()
    {
        await Assert.That(new JobId(112).CanUseSkillRoot(100)).IsTrue(); // Hero can use Warrior root
        await Assert.That(new JobId(112).CanUseSkillRoot(200)).IsFalse(); // Hero cannot use Mage root
    }

    [Test]
    public async Task CanUseSkillRoot_BranchAndTier_MatchesCorrectly()
    {
        await Assert.That(new JobId(112).CanUseSkillRoot(110)).IsTrue(); // same branch
        await Assert.That(new JobId(112).CanUseSkillRoot(111)).IsTrue(); // lower tier
        await Assert.That(new JobId(112).CanUseSkillRoot(112)).IsTrue(); // same tier
        await Assert.That(new JobId(111).CanUseSkillRoot(112)).IsFalse(); // higher tier
        await Assert.That(new JobId(112).CanUseSkillRoot(120)).IsFalse(); // different branch
    }

    // ── EquipBit ──────────────────────────────────────────────────────────

    [Test]
    [Arguments(0, 0)] // Beginner → 0
    [Arguments(100, 1)] // Warrior → bit 0
    [Arguments(200, 2)] // Mage → bit 1
    [Arguments(300, 4)] // Archer → bit 2
    [Arguments(400, 8)] // Thief → bit 3
    [Arguments(500, 16)] // Pirate → bit 4
    public async Task EquipBit_MatchesExpected(int job, int expected)
    {
        await Assert.That(new JobId(job).EquipBit).IsEqualTo(expected);
    }

    // ── IsDualBladeBorn ───────────────────────────────────────────────────

    [Test]
    public async Task IsDualBladeBorn_ExplorerWithSubJob1_ReturnsTrue()
    {
        await Assert.That(new JobId(400).IsDualBladeBorn(1)).IsTrue();
        await Assert.That(new JobId(0).IsDualBladeBorn(1)).IsTrue();
    }

    [Test]
    public async Task IsDualBladeBorn_NonExplorerOrSubJob0_ReturnsFalse()
    {
        await Assert.That(new JobId(400).IsDualBladeBorn(0)).IsFalse();
        await Assert.That(new JobId(1400).IsDualBladeBorn(1)).IsFalse(); // Cygnus
    }

    // ── DualJobChangeLevel ────────────────────────────────────────────────

    [Test]
    [Arguments(400, 10)]
    [Arguments(430, 20)]
    [Arguments(431, 30)]
    [Arguments(432, 55)]
    [Arguments(433, 70)]
    [Arguments(434, 120)]
    [Arguments(112, 200)] // non-DB → default 200
    public async Task DualJobChangeLevel_MatchesExpected(int job, int expected)
    {
        await Assert.That(new JobId(job).DualJobChangeLevel).IsEqualTo(expected);
    }

    // ── DecodeReqJob ──────────────────────────────────────────────────────

    [Test]
    public async Task DecodeReqJob_AllJobs_ReturnsSingleEntry()
    {
        var buf = new string[JobId.MaxReqJobNames];
        int count = JobId.DecodeReqJob(JobId.ReqJobAll, buf);
        await Assert.That(count).IsEqualTo(1);
        await Assert.That(buf[0]).IsEqualTo("All jobs");
    }

    [Test]
    public async Task DecodeReqJob_BeginnerOnly_ReturnsSingleEntry()
    {
        var buf = new string[JobId.MaxReqJobNames];
        int count = JobId.DecodeReqJob(JobId.ReqJobBeginner, buf);
        await Assert.That(count).IsEqualTo(1);
        await Assert.That(buf[0]).IsEqualTo("Beginner only");
    }

    [Test]
    public async Task DecodeReqJob_WarriorMageBits_ReturnsTwoEntries()
    {
        var buf = new string[JobId.MaxReqJobNames];
        int count = JobId.DecodeReqJob(0x01 | 0x02, buf);
        await Assert.That(count).IsEqualTo(2);
        await Assert.That(buf[0]).IsEqualTo("Warrior");
        await Assert.That(buf[1]).IsEqualTo("Magician");
    }

    [Test]
    public async Task DecodeReqJob_AllFiveBits_ReturnsFiveEntries()
    {
        var buf = new string[JobId.MaxReqJobNames];
        int count = JobId.DecodeReqJob(0x1F, buf);
        await Assert.That(count).IsEqualTo(5);
    }

    // ── GetJobIconPath ────────────────────────────────────────────────────

    [Test]
    public async Task GetJobIconPath_WarriorOnline_ReturnsCorrectPath()
    {
        var path = new JobId(100).GetJobIconPath(bOnline: true);
        await Assert.That(path).IsEqualTo("UI/UIWindow2.img/UserList/Main/Expedition/icon1/1");
    }

    [Test]
    public async Task GetJobIconPath_MageOffline_ReturnsCorrectPath()
    {
        var path = new JobId(200).GetJobIconPath(bOnline: false);
        await Assert.That(path).IsEqualTo("UI/UIWindow2.img/UserList/Main/Expedition/icon2/0");
    }

    // ── FieldCategoryCode ─────────────────────────────────────────────────

    [Test]
    [Arguments(112, 1)] // Hero
    [Arguments(1212, 12)] // Blaze Wizard
    [Arguments(2112, 21)] // Aran
    [Arguments(3212, 32)] // Battle Mage
    public async Task FieldCategoryCode_MatchesExpected(int job, int expected)
    {
        await Assert.That(new JobId(job).FieldCategoryCode).IsEqualTo(expected);
    }

    // ── Lineage / Subclass Predicates ─────────────────────────────────────

    [Test]
    public async Task LineagePredicates_MatchExpected()
    {
        await Assert.That(new JobId(0).IsExplorer).IsTrue();
        await Assert.That(new JobId(112).IsExplorer).IsTrue();
        await Assert.That(new JobId(1000).IsCygnus).IsTrue();
        await Assert.That(new JobId(1112).IsCygnus).IsTrue();
        await Assert.That(new JobId(2000).IsAran).IsTrue();
        await Assert.That(new JobId(2112).IsAran).IsTrue();
        await Assert.That(new JobId(2001).IsAran).IsFalse(); // Evan root
        await Assert.That(new JobId(2000).IsMapleHero).IsTrue();
        await Assert.That(new JobId(2001).IsMapleHero).IsTrue();
        await Assert.That(new JobId(3000).IsResistance).IsTrue();
        await Assert.That(new JobId(3212).IsResistance).IsTrue();
    }

    [Test]
    public async Task SubclassPredicates_MatchExpected()
    {
        await Assert.That(new JobId(3200).IsBattleMage).IsTrue();
        await Assert.That(new JobId(3212).IsBattleMage).IsTrue();
        await Assert.That(new JobId(3300).IsWildHunter).IsTrue();
        await Assert.That(new JobId(3500).IsMechanic).IsTrue();
        await Assert.That(new JobId(3200).IsWildHunter).IsFalse();
    }

    [Test]
    public async Task IsMage_MatchesAllMageLineages()
    {
        await Assert.That(new JobId(200).IsMage).IsTrue();
        await Assert.That(new JobId(1200).IsMage).IsTrue();
        await Assert.That(new JobId(2200).IsMage).IsTrue();
        await Assert.That(new JobId(3200).IsMage).IsTrue();
        await Assert.That(new JobId(100).IsMage).IsFalse();
    }

    [Test]
    public async Task IsExtendSP_MatchesResistanceAndEvan()
    {
        await Assert.That(new JobId(3000).IsExtendSP).IsTrue();
        await Assert.That(new JobId(3212).IsExtendSP).IsTrue();
        await Assert.That(new JobId(2200).IsExtendSP).IsTrue();
        await Assert.That(new JobId(2001).IsExtendSP).IsTrue();
        await Assert.That(new JobId(112).IsExtendSP).IsFalse();
    }

    [Test]
    public async Task IsAdmin_IsManager_MatchExpected()
    {
        await Assert.That(new JobId(900).IsAdmin).IsTrue();
        await Assert.That(new JobId(910).IsAdmin).IsTrue();
        await Assert.That(new JobId(800).IsManager).IsTrue();
        await Assert.That(new JobId(100).IsAdmin).IsFalse();
        await Assert.That(new JobId(100).IsManager).IsFalse();
    }

    // ── MpPerLevel ────────────────────────────────────────────────────────

    [Test]
    [Arguments(100, 2)] // Warrior
    [Arguments(200, 18)] // Mage
    [Arguments(300, 10)] // Archer
    [Arguments(400, 10)] // Thief
    [Arguments(500, 14)] // Pirate
    [Arguments(0, 6)] // Beginner
    public async Task MpPerLevel_MatchesExpected(int job, int expected)
    {
        await Assert.That(new JobId(job).MpPerLevel).IsEqualTo(expected);
    }

    // ── Constants ─────────────────────────────────────────────────────────

    [Test]
    public async Task Constants_HaveCorrectValues()
    {
        await Assert.That(JobId.MaxSkillRoots).IsEqualTo(10);
        await Assert.That(JobId.MaxReqJobNames).IsEqualTo(5);
        await Assert.That(JobId.ReqJobAll).IsEqualTo(0);
        await Assert.That(JobId.ReqJobBeginner).IsEqualTo(-1);
    }
}
