namespace Maple.StrongId.Test;

public sealed class SkillTemplateIdTests
{
    // ── Digit Decomposition ───────────────────────────────────────────────

    [Test]
    [Arguments(1_111_002, 111, 1002, 1, 0)] // Crusader skill: root 111, serial 1002, category 1, lineage 0
    [Arguments(22_171_004, 2217, 1004, 2, 2)] // Evan Hero's Will: root 2217, serial 1004, category 2, lineage 2
    [Arguments(91_000_003, 9100, 3, 1, 9)] // Guild AttMagUp: root 9100, serial 3, category 1 (9100%1000/100=1)
    public async Task Decomposition_MatchesExpected(int skill, int root, int serial, int category, int lineage)
    {
        var id = new SkillTemplateId(skill);
        await Assert.That(id.Root).IsEqualTo(root);
        await Assert.That(id.Serial).IsEqualTo(serial);
        await Assert.That(id.JobCategory).IsEqualTo(category);
        await Assert.That(id.JobLineage).IsEqualTo(lineage);
    }

    [Test]
    public async Task WzImageName_FormatsCorrectly()
    {
#pragma warning disable CS0618 // Testing convenience API
        await Assert.That(new SkillTemplateId(1_111_002).WzImageName).IsEqualTo("111.img");
        await Assert.That(new SkillTemplateId(91_000_000).WzImageName).IsEqualTo("9100.img");
#pragma warning restore CS0618
    }

    [Test]
    public async Task WzSkillNodePath_FormatsCorrectly()
    {
#pragma warning disable CS0618 // Testing convenience API
        await Assert.That(new SkillTemplateId(1_111_002).WzSkillNodePath).IsEqualTo("111.img/skill/1111002");
#pragma warning restore CS0618
    }

    // ── Active / Passive / Common ─────────────────────────────────────────

    [Test]
    [Arguments(1_121_008, false, true, false)] // active (serial thousands = 1)
    [Arguments(1_129_000, true, false, false)] // passive (serial thousands = 9)
    [Arguments(21_000_000, false, false, true)] // common (serial = 0, thousands = 0) — ARAN_COMBO_ABILITY
    public async Task ActivePassiveCommon_ClassifiesCorrectly(int skill, bool passive, bool active, bool common)
    {
        var id = new SkillTemplateId(skill);
        await Assert.That(id.IsPassive).IsEqualTo(passive);
        await Assert.That(id.IsActive).IsEqualTo(active);
        await Assert.That(id.IsCommon).IsEqualTo(common);
    }

    // ── NeedsMasterLevel ──────────────────────────────────────────────────

    [Test]
    [Arguments(1_120_012, false)] // Hero — exempted transition skill
    [Arguments(1_121_001, true)] // Hero active 4th-job skill (root 112, root%10==2) → needs ML
    [Arguments(22_151_001, false)] // Evan 6th growth (tier 6) — only tiers 9/10 need ML
    [Arguments(22_171_001, true)] // Evan 9th growth (tier 9) → needs ML
    [Arguments(4_341_002, false)] // Dual Blade 4th — explicitly excepted ID
    [Arguments(4_341_006, true)] // Dual Blade 4th — non-excepted ID → needs ML
    [Arguments(1_110_000, false)] // Fighter branch root (root % 100 == 0) → no ML
    public async Task NeedsMasterLevel_MatchesExpected(int skill, bool expected)
    {
        await Assert.That(new SkillTemplateId(skill).NeedsMasterLevel).IsEqualTo(expected);
    }

    // ── IsGuildSkill / IsSuperGmSkill ─────────────────────────────────────

    [Test]
    public async Task GuildVsSuperGm_AreDistinct()
    {
        // Guild root 9100 — NOT SuperGM
        var guild = new SkillTemplateId(91_000_000);
        await Assert.That(guild.IsGuildSkill).IsTrue();
        await Assert.That(guild.IsSuperGmSkill).IsFalse();
        await Assert.That(guild.IsUnregistered).IsTrue(); // 8-digit starting with 9

        // SuperGM root 910 — NOT guild
        var superGm = new SkillTemplateId(9_101_000);
        await Assert.That(superGm.IsGuildSkill).IsFalse();
        await Assert.That(superGm.IsSuperGmSkill).IsTrue();
    }

    // ── IsEventVehicleSkill ───────────────────────────────────────────────

    [Test]
    public async Task IsEventVehicleSkill_MatchesBySerialAcrossJobs()
    {
        // Serial 1025 is an event vehicle serial — matches regardless of job prefix
        int serial = 1025;
        await Assert.That(new SkillTemplateId(serial).IsEventVehicleSkill).IsTrue();
        await Assert.That(new SkillTemplateId((10_000 * 10_000) + serial).IsEventVehicleSkill).IsTrue();

        // Serial 1004 is the regular Monster Riding serial — NOT an event vehicle
        await Assert.That(new SkillTemplateId(1_004).IsEventVehicleSkill).IsFalse();
    }

    // ── CanBeUsedByJob ────────────────────────────────────────────────────

    [Test]
    public async Task CanBeUsedByJob_HeroSkill_OnlyHeroCanUse()
    {
        var heroSkill = new SkillTemplateId(1_121_001); // root 112
        await Assert.That(heroSkill.CanBeUsedByJob(new JobId(112))).IsTrue();
        await Assert.That(heroSkill.CanBeUsedByJob(new JobId(111))).IsFalse(); // Crusader (lower tier)
        await Assert.That(heroSkill.CanBeUsedByJob(new JobId(122))).IsFalse(); // Paladin (different branch)
    }

    // ── OwnerJobDegree ────────────────────────────────────────────────────

    [Test]
    [Arguments(1_000_001, 1)] // root 100 (Warrior category-root) → degree 1
    [Arguments(1_100_001, 2)] // root 110 (Fighter branch-root) → degree 2
    [Arguments(1_110_001, 0)] // root 111 (Crusader specific-tier) → degree 0
    [Arguments(0, 0)] // root 0 → invalid → degree 0
    public async Task OwnerJobDegree_MatchesExpected(int skill, int expectedDegree)
    {
        await Assert.That(new SkillTemplateId(skill).OwnerJobDegree).IsEqualTo(expectedDegree);
    }

    // ── Admin / Guild constants ───────────────────────────────────────────

    [Test]
    public async Task AdminConstants_HaveCorrectRoots()
    {
        await Assert.That(SkillTemplateId.Admin.Haste.Root).IsEqualTo(900);
        await Assert.That(SkillTemplateId.Admin.Dispel.Root).IsEqualTo(910);
        await Assert.That(SkillTemplateId.Admin.Haste.IsAnyAdminSkill).IsTrue();
    }

    [Test]
    public async Task GuildConstants_HaveCorrectRoot()
    {
        await Assert.That(SkillTemplateId.Guild.MesoUp.Root).IsEqualTo(9100);
        await Assert.That(SkillTemplateId.Guild.MesoUp.IsGuildSkill).IsTrue();
        await Assert.That(SkillTemplateId.Guild.RegularSupport.IsGuildSkill).IsTrue();
    }

    // ── IsBaseClassSkill / IsNovice ───────────────────────────────────────

    [Test]
    [Arguments(1_000_001, true)] // root 100, category-root → base class
    [Arguments(1_100_001, false)] // root 110, branch-root → not base class
    [Arguments(1_110_001, false)] // root 111, specific tier → not base class
    public async Task IsBaseClassSkill_MatchesExpected(int skill, bool expected)
    {
        await Assert.That(new SkillTemplateId(skill).IsBaseClassSkill).IsEqualTo(expected);
    }

    [Test]
    [Arguments(1, true)] // root 0, lineage root → novice
    [Arguments(10_000_001, true)] // root 1000, Noblesse → novice
    [Arguments(20_010_001, true)] // root 2001, Evan root → novice
    [Arguments(30_000_001, true)] // root 3000, Citizen → novice
    [Arguments(1_000_001, false)] // root 100, Warrior → not novice (category root but not lineage)
    public async Task IsNovice_MatchesExpected(int skill, bool expected)
    {
        await Assert.That(new SkillTemplateId(skill).IsNovice).IsEqualTo(expected);
    }

    // ── IsKeyDown ─────────────────────────────────────────────────────────

    [Test]
    public async Task IsKeyDown_KnownSkills_ReturnTrue()
    {
        await Assert.That(new SkillTemplateId(2_121_001).IsKeyDown).IsTrue(); // Arch Mage F/P Big Bang
        await Assert.That(new SkillTemplateId(3_121_004).IsKeyDown).IsTrue(); // Bowmaster Hurricane
        await Assert.That(new SkillTemplateId(5_221_004).IsKeyDown).IsTrue(); // Corsair Rapid Fire
        await Assert.That(new SkillTemplateId(35_001_001).IsKeyDown).IsTrue(); // Mechanic Robot Transform
    }

    [Test]
    public async Task IsKeyDown_NonKeyDown_ReturnFalse()
    {
        await Assert.That(new SkillTemplateId(1_121_001).IsKeyDown).IsFalse();
    }

    // ── IsTeleport / IsTeleportMastery ────────────────────────────────────

    [Test]
    public async Task IsTeleport_MatchesBaseTeleportSkills()
    {
        await Assert.That(new SkillTemplateId(2_101_002).IsTeleport).IsTrue(); // Wizard F/P
        await Assert.That(new SkillTemplateId(2_201_002).IsTeleport).IsTrue(); // Wizard I/L
        await Assert.That(new SkillTemplateId(2_301_001).IsTeleport).IsTrue(); // Cleric
        await Assert.That(new SkillTemplateId(9_001_002).IsTeleport).IsTrue(); // Admin
        await Assert.That(new SkillTemplateId(32_001_002).IsTeleport).IsTrue(); // Battle Mage
        await Assert.That(new SkillTemplateId(2_111_007).IsTeleport).IsFalse(); // mastery, not base
    }

    [Test]
    public async Task IsTeleportMastery_MatchesMasterySkills()
    {
        await Assert.That(new SkillTemplateId(2_111_007).IsTeleportMastery).IsTrue();
        await Assert.That(new SkillTemplateId(2_211_007).IsTeleportMastery).IsTrue();
        await Assert.That(new SkillTemplateId(2_311_007).IsTeleportMastery).IsTrue();
        await Assert.That(new SkillTemplateId(32_111_010).IsTeleportMastery).IsTrue();
        await Assert.That(new SkillTemplateId(2_101_002).IsTeleportMastery).IsFalse(); // base, not mastery
    }

    // ── IsVehicleSkill ────────────────────────────────────────────────────

    [Test]
    public async Task IsVehicleSkill_MatchesKnownVehicles()
    {
        await Assert.That(new SkillTemplateId(1_004).IsVehicleSkill).IsTrue(); // Explorer Monster Riding
        await Assert.That(new SkillTemplateId(5_221_006).IsVehicleSkill).IsTrue(); // Corsair Battleship
        await Assert.That(new SkillTemplateId(33_001_001).IsVehicleSkill).IsTrue(); // Wild Hunter Jaguar
        await Assert.That(new SkillTemplateId(35_001_002).IsVehicleSkill).IsTrue(); // Mechanic Robot
        await Assert.That(new SkillTemplateId(1_025).IsVehicleSkill).IsFalse(); // event vehicle serial
    }

    // ── IsHeroWill ────────────────────────────────────────────────────────

    [Test]
    public async Task IsHeroWill_MatchesKnownSkills()
    {
        await Assert.That(new SkillTemplateId(1_121_011).IsHeroWill).IsTrue(); // Hero
        await Assert.That(new SkillTemplateId(4_341_008).IsHeroWill).IsTrue(); // Dual Blade
        await Assert.That(new SkillTemplateId(22_171_004).IsHeroWill).IsTrue(); // Evan
        await Assert.That(new SkillTemplateId(32_121_008).IsHeroWill).IsTrue(); // Battle Mage
        await Assert.That(new SkillTemplateId(1_121_001).IsHeroWill).IsFalse(); // non-HW Hero skill
    }

    // ── IsAranCombo ───────────────────────────────────────────────────────

    [Test]
    public async Task IsAranCombo_MatchesFiveKnownSkills()
    {
        await Assert.That(new SkillTemplateId(21_000_000).IsAranCombo).IsTrue();
        await Assert.That(new SkillTemplateId(21_000_002).IsAranCombo).IsTrue();
        await Assert.That(new SkillTemplateId(21_001_000).IsAranCombo).IsTrue();
        await Assert.That(new SkillTemplateId(21_001_001).IsAranCombo).IsTrue();
        await Assert.That(new SkillTemplateId(21_001_002).IsAranCombo).IsTrue();
        await Assert.That(new SkillTemplateId(21_000_001).IsAranCombo).IsFalse();
    }

    // ── IsBattleMageAura ──────────────────────────────────────────────────

    [Test]
    public async Task IsBattleMageAura_MatchesKnownAuras()
    {
        await Assert.That(new SkillTemplateId(32_001_003).IsBattleMageAura).IsTrue(); // Dark Aura
        await Assert.That(new SkillTemplateId(32_101_001).IsBattleMageAura).IsTrue();
        await Assert.That(new SkillTemplateId(32_120_001).IsBattleMageAura).IsTrue();
        await Assert.That(new SkillTemplateId(32_001_002).IsBattleMageAura).IsFalse(); // not an aura
    }

    // ── IsNonSlot / IsPrepareBomb ─────────────────────────────────────────

    [Test]
    public async Task IsNonSlot_MatchesKnownSkills()
    {
        await Assert.That(new SkillTemplateId(1_066).IsNonSlot).IsTrue();
        await Assert.That(new SkillTemplateId(33_001_002).IsNonSlot).IsTrue(); // Wild Hunter Jaguar passive
        await Assert.That(new SkillTemplateId(1_004).IsNonSlot).IsFalse(); // Monster Riding
    }

    [Test]
    public async Task IsPrepareBomb_MatchesKnownSkills()
    {
        await Assert.That(new SkillTemplateId(4_341_003).IsPrepareBomb).IsTrue(); // DB Fly Fan
        await Assert.That(new SkillTemplateId(5_201_002).IsPrepareBomb).IsTrue(); // Gunslinger Recoil
        await Assert.That(new SkillTemplateId(14_111_006).IsPrepareBomb).IsTrue(); // Night Walker
        await Assert.That(new SkillTemplateId(5_221_004).IsPrepareBomb).IsFalse();
    }

    // ── TryWriteWzImageName / TryWriteWzSkillNodePath ─────────────────────

    [Test]
    public async Task TryWriteWzImageName_WritesCorrectly()
    {
        var buf = new char[8]; // "111.img" = 7 chars
        var ok = new SkillTemplateId(1_111_002).TryWriteWzImageName(buf, out int written);
        var result = new string(buf, 0, written);
        await Assert.That(ok).IsTrue();
        await Assert.That(result).IsEqualTo("111.img");
    }

    [Test]
    public async Task TryWriteWzImageName_FailsWhenTooSmall()
    {
        var buf = new char[4]; // too small
        var ok = new SkillTemplateId(1_111_002).TryWriteWzImageName(buf, out int written);
        await Assert.That(ok).IsFalse();
        await Assert.That(written).IsEqualTo(0);
    }

    [Test]
    public async Task TryWriteWzSkillNodePath_WritesCorrectly()
    {
        var buf = new char[30];
        var ok = new SkillTemplateId(1_111_002).TryWriteWzSkillNodePath(buf, out int written);
        var result = new string(buf, 0, written);
        await Assert.That(ok).IsTrue();
        await Assert.That(result).IsEqualTo("111.img/skill/1111002");
    }

    [Test]
    public async Task TryWriteWzSkillNodePath_FailsWhenTooSmall()
    {
        var buf = new char[5]; // way too small
        var ok = new SkillTemplateId(1_111_002).TryWriteWzSkillNodePath(buf, out int written);
        await Assert.That(ok).IsFalse();
        await Assert.That(written).IsEqualTo(0);
    }

    // ── Additional Admin / Guild Constants ─────────────────────────────────

    [Test]
    public async Task AdminSkill900_MatchesRoot900()
    {
        await Assert.That(SkillTemplateId.Admin.Haste.IsAdminSkill900).IsTrue();
        await Assert.That(SkillTemplateId.Admin.DragonRoar.IsAdminSkill900).IsTrue();
        await Assert.That(SkillTemplateId.Admin.Teleport.IsAdminSkill900).IsTrue();
        await Assert.That(SkillTemplateId.Admin.AntiMacro.IsAdminSkill900).IsTrue();
        await Assert.That(SkillTemplateId.Admin.Dispel.IsAdminSkill900).IsFalse(); // root 910
    }

    [Test]
    public async Task NoblesseSkill_MatchesRoot1000()
    {
        await Assert.That(new SkillTemplateId(10_000_001).IsNoblesseSkill).IsTrue();
        await Assert.That(new SkillTemplateId(10_009_999).IsNoblesseSkill).IsTrue();
        await Assert.That(new SkillTemplateId(10_010_000).IsNoblesseSkill).IsFalse();
    }

    [Test]
    public async Task IsExtendSPOwner_MatchesExpected()
    {
        await Assert.That(new SkillTemplateId(32_001_003).IsExtendSPOwner).IsTrue(); // Resistance
        await Assert.That(new SkillTemplateId(22_171_004).IsExtendSPOwner).IsTrue(); // Evan
        await Assert.That(new SkillTemplateId(1_121_001).IsExtendSPOwner).IsFalse(); // Explorer
    }

    [Test]
    public async Task AllAdminConstants_HaveCorrectValues()
    {
        await Assert.That(SkillTemplateId.Admin.Haste.Value).IsEqualTo(9_001_000);
        await Assert.That(SkillTemplateId.Admin.DragonRoar.Value).IsEqualTo(9_001_001);
        await Assert.That(SkillTemplateId.Admin.Teleport.Value).IsEqualTo(9_001_002);
        await Assert.That(SkillTemplateId.Admin.AntiMacro.Value).IsEqualTo(9_001_009);
        await Assert.That(SkillTemplateId.Admin.Dispel.Value).IsEqualTo(9_101_000);
        await Assert.That(SkillTemplateId.Admin.SuperHaste.Value).IsEqualTo(9_101_001);
        await Assert.That(SkillTemplateId.Admin.HolySymbol.Value).IsEqualTo(9_101_002);
        await Assert.That(SkillTemplateId.Admin.Bless.Value).IsEqualTo(9_101_003);
        await Assert.That(SkillTemplateId.Admin.Hide.Value).IsEqualTo(9_101_004);
        await Assert.That(SkillTemplateId.Admin.Resurrection.Value).IsEqualTo(9_101_005);
        await Assert.That(SkillTemplateId.Admin.HyperBody.Value).IsEqualTo(9_101_008);
    }

    [Test]
    public async Task AllGuildConstants_HaveCorrectValues()
    {
        await Assert.That(SkillTemplateId.Guild.MesoUp.Value).IsEqualTo(91_000_000);
        await Assert.That(SkillTemplateId.Guild.ExperienceUp.Value).IsEqualTo(91_000_001);
        await Assert.That(SkillTemplateId.Guild.DefenceUp.Value).IsEqualTo(91_000_002);
        await Assert.That(SkillTemplateId.Guild.AttMagUp.Value).IsEqualTo(91_000_003);
        await Assert.That(SkillTemplateId.Guild.AgilityUp.Value).IsEqualTo(91_000_004);
        await Assert.That(SkillTemplateId.Guild.BusinessEfficacyUp.Value).IsEqualTo(91_000_005);
        await Assert.That(SkillTemplateId.Guild.RegularSupport.Value).IsEqualTo(91_000_006);
    }
}
