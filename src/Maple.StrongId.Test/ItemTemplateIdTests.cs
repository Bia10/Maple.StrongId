namespace Maple.StrongId.Test;

public sealed class ItemTemplateIdTests
{
    // ── Digit Decomposition ───────────────────────────────────────────────

    [Test]
    [Arguments(1_002_140, 1, 100, 0, 2)] // equip hat: cat 1, typeindex 100, sub 0, gender 2
    [Arguments(2_000_001, 2, 200, 0, 0)] // use: cat 2, typeindex 200
    [Arguments(1_302_000, 1, 130, 30, 2)] // one-handed sword
    [Arguments(1_402_000, 1, 140, 40, 2)] // two-handed sword
    public async Task DigitDecomposition_MatchesExpected(
        int id,
        int category,
        int typeIndex,
        int subType,
        int genderDigit
    )
    {
        var item = new ItemTemplateId(id);
        await Assert.That(item.Category).IsEqualTo(category);
        await Assert.That(item.TypeIndex).IsEqualTo(typeIndex);
        await Assert.That(item.SubType).IsEqualTo(subType);
        await Assert.That(item.GenderDigit).IsEqualTo(genderDigit);
    }

    // ── Category Predicates ───────────────────────────────────────────────

    [Test]
    [Arguments(1_002_140, true, false, false, false, false)] // equip
    [Arguments(2_000_001, false, true, false, false, false)] // use
    [Arguments(3_010_000, false, false, true, false, false)] // setup
    [Arguments(4_000_001, false, false, false, true, false)] // etc
    [Arguments(5_000_001, false, false, false, false, true)] // cash
    public async Task CategoryPredicates_MatchExpected(
        int id,
        bool equip,
        bool consume,
        bool setup,
        bool etc,
        bool cash
    )
    {
        var item = new ItemTemplateId(id);
        await Assert.That(item.IsEquip).IsEqualTo(equip);
        await Assert.That(item.IsConsume).IsEqualTo(consume);
        await Assert.That(item.IsSetup).IsEqualTo(setup);
        await Assert.That(item.IsEtc).IsEqualTo(etc);
        await Assert.That(item.IsCash).IsEqualTo(cash);
    }

    // ── InventoryTab ──────────────────────────────────────────────────────

    [Test]
    [Arguments(1_002_140, 0)] // equip → tab 0
    [Arguments(2_000_001, 1)] // use → tab 1
    [Arguments(3_010_000, 3)] // setup → tab 3
    [Arguments(4_000_001, 3)] // etc → tab 3 (shares with setup)
    [Arguments(5_000_001, 4)] // cash → tab 4
    public async Task InventoryTab_MatchesExpected(int id, int expectedTab)
    {
        await Assert.That(new ItemTemplateId(id).InventoryTab).IsEqualTo(expectedTab);
    }

    // ── Weapon Predicates ─────────────────────────────────────────────────

    [Test]
    [Arguments(1_302_000, true, false, true)] // one-handed sword (typeindex 130)
    [Arguments(1_402_000, true, true, false)] // two-handed sword (typeindex 140)
    [Arguments(1_002_140, false, false, false)] // hat (typeindex 100), not a weapon
    public async Task WeaponPredicates_MatchExpected(int id, bool isWeapon, bool twoHanded, bool oneHanded)
    {
        var item = new ItemTemplateId(id);
        await Assert.That(item.IsWeapon).IsEqualTo(isWeapon);
        await Assert.That(item.IsTwoHandedWeapon).IsEqualTo(twoHanded);
        await Assert.That(item.IsOneHandedWeapon).IsEqualTo(oneHanded);
    }

    // ── IsNonStackable ────────────────────────────────────────────────────

    [Test]
    [Arguments(1_002_140, true)] // equip → non-stackable
    [Arguments(5_000_001, true)] // cash → non-stackable
    [Arguments(2_070_000, true)] // rechargeable (javelin, typeindex 207) → non-stackable
    [Arguments(2_000_001, false)] // regular potion → stackable
    [Arguments(4_000_001, false)] // etc item → stackable
    public async Task IsNonStackable_MatchesExpected(int id, bool expected)
    {
        await Assert.That(new ItemTemplateId(id).IsNonStackable).IsEqualTo(expected);
    }

    // ── MasteryBook predicates ────────────────────────────────────────────

    [Test]
    public async Task IsDualMasteryBook_ExcludesExceptionIds()
    {
        // TypeIndex 562 — normally dual mastery books
        await Assert.That(new ItemTemplateId(5_620_000).IsDualMasteryBook).IsTrue();
        // IDs 5_620_007 and 5_620_008 are NOT dual mastery books
        await Assert.That(new ItemTemplateId(5_620_007).IsDualMasteryBook).IsFalse();
        await Assert.That(new ItemTemplateId(5_620_008).IsDualMasteryBook).IsFalse();
        // But they ARE regular mastery books
        await Assert.That(new ItemTemplateId(5_620_007).IsMasteryBook).IsTrue();
        await Assert.That(new ItemTemplateId(5_620_008).IsMasteryBook).IsTrue();
    }

    // ── CanRegularScrollApplyTo ───────────────────────────────────────────

    [Test]
    public async Task CanRegularScrollApplyTo_HatScroll_OnlyAppliesToHats()
    {
        // TypeIndex 204, formula: (nScrollId - 2_040_000) / 100 == sub-type
        // Sub-type 00 → targets typeindex % 100 == 0 → typeindex 100 (hats)
        var hatScroll = new ItemTemplateId(2_040_000);
        var hat = new ItemTemplateId(1_002_140); // typeindex 100
        var gloves = new ItemTemplateId(1_082_000); // typeindex 108

        await Assert.That(hatScroll.CanRegularScrollApplyTo(hat)).IsTrue();
        await Assert.That(hatScroll.CanRegularScrollApplyTo(gloves)).IsFalse();
    }

    [Test]
    public async Task CanRegularScrollApplyTo_NonScrollItem_ReturnsFalse()
    {
        var potion = new ItemTemplateId(2_000_001); // not a TypeIndex-204 scroll
        var hat = new ItemTemplateId(1_002_140);
        await Assert.That(potion.CanRegularScrollApplyTo(hat)).IsFalse();
    }

    // ── CashSlotItemType ──────────────────────────────────────────────────

    [Test]
    [Arguments(5_000_001, 8)] // TypeIndex 500 → 8
    [Arguments(5_010_000, 9)] // TypeIndex 501 → 9
    [Arguments(5_130_000, 7)] // TypeIndex 513 → 7
    [Arguments(5_170_000, 17)] // TypeIndex 517, exact multiple of 10_000 → 17
    [Arguments(5_171_000, 0)] // TypeIndex 517, not exact multiple → 0
    [Arguments(5_251_100, 37)] // TypeIndex 525, specific ID 5_251_100 → 37
    [Arguments(5_250_000, 36)] // TypeIndex 525, any other → 36
    [Arguments(9_999_999, 0)] // Unknown typeindex → 0
    public async Task CashSlotItemType_MatchesExpected(int id, int expected)
    {
        await Assert.That(new ItemTemplateId(id).CashSlotItemType).IsEqualTo(expected);
    }

    // ── Predicate / Set Agreement ─────────────────────────────────────────

    [Test]
    public async Task IsEngagementRingItem_AgreesWithEngagementRingsSet()
    {
        foreach (var id in ItemTemplateId.EngagementRings)
            await Assert.That(id.IsEngagementRingItem).IsTrue();
    }

    [Test]
    public async Task IsWeddingRingItem_AgreesWithWeddingRingsSet()
    {
        foreach (var id in ItemTemplateId.WeddingRings)
            await Assert.That(id.IsWeddingRingItem).IsTrue();
    }

    [Test]
    public async Task IsEvanDragonRidingItem_AgreesWithEvanDragonRidingItemsSet()
    {
        foreach (var id in ItemTemplateId.EvanDragonRidingItems)
            await Assert.That(id.IsEvanDragonRidingItem).IsTrue();
    }

    [Test]
    public async Task IsEngagementRingItem_RejectsAdjacentIds()
    {
        // One below and one above the 8-ID sequential range
        await Assert.That(new ItemTemplateId(4_031_356).IsEngagementRingItem).IsFalse();
        await Assert.That(new ItemTemplateId(4_031_365).IsEngagementRingItem).IsFalse();
    }

    // ── Wedding Predicates ────────────────────────────────────────────────

    [Test]
    public async Task IsInvitationBundle_MatchesExpectedIds()
    {
        await Assert.That(new ItemTemplateId(4_031_377).IsInvitationBundle).IsTrue();
        await Assert.That(new ItemTemplateId(4_031_395).IsInvitationBundle).IsTrue();
        await Assert.That(new ItemTemplateId(4_031_396).IsInvitationBundle).IsFalse();
    }

    // ── IsSpecialItem range ───────────────────────────────────────────────

    [Test]
    public async Task IsSpecialItem_CoversExpectedRange()
    {
        await Assert.That(new ItemTemplateId(9_100_000).IsSpecialItem).IsTrue();
        await Assert.That(new ItemTemplateId(9_199_999).IsSpecialItem).IsTrue();
        await Assert.That(new ItemTemplateId(9_099_999).IsSpecialItem).IsFalse(); // below range
        await Assert.That(new ItemTemplateId(9_200_000).IsSpecialItem).IsFalse(); // above range
    }

    // ── Equip Slot Predicates ─────────────────────────────────────────────

    [Test]
    [Arguments(1_002_140, true)] // typeindex 100 → hat
    [Arguments(1_012_000, false)] // typeindex 101 → face acc, not hat
    public async Task IsHat_MatchesExpected(int id, bool expected)
    {
        await Assert.That(new ItemTemplateId(id).IsHat).IsEqualTo(expected);
    }

    [Test]
    public async Task EquipSlotPredicates_MatchTypeIndex()
    {
        await Assert.That(new ItemTemplateId(1_012_000).IsFaceAcc).IsTrue(); // typeindex 101
        await Assert.That(new ItemTemplateId(1_022_000).IsEyeAcc).IsTrue(); // typeindex 102
        await Assert.That(new ItemTemplateId(1_032_000).IsEarring).IsTrue(); // typeindex 103
        await Assert.That(new ItemTemplateId(1_042_000).IsTop).IsTrue(); // typeindex 104
        await Assert.That(new ItemTemplateId(1_052_000).IsLongCoat).IsTrue(); // typeindex 105
        await Assert.That(new ItemTemplateId(1_042_000).IsTopOrOverall).IsTrue(); // 104
        await Assert.That(new ItemTemplateId(1_052_000).IsTopOrOverall).IsTrue(); // 105
        await Assert.That(new ItemTemplateId(1_062_000).IsBottom).IsTrue(); // typeindex 106
        await Assert.That(new ItemTemplateId(1_072_000).IsShoes).IsTrue(); // typeindex 107
        await Assert.That(new ItemTemplateId(1_082_000).IsGloves).IsTrue(); // typeindex 108
        await Assert.That(new ItemTemplateId(1_092_000).IsShield).IsTrue(); // typeindex 109
        await Assert.That(new ItemTemplateId(1_102_000).IsCape).IsTrue(); // typeindex 110
        await Assert.That(new ItemTemplateId(1_112_000).IsRing).IsTrue(); // typeindex 111
        await Assert.That(new ItemTemplateId(1_122_000).IsPendant).IsTrue(); // typeindex 112
        await Assert.That(new ItemTemplateId(1_132_000).IsBelt).IsTrue(); // typeindex 113
        await Assert.That(new ItemTemplateId(1_142_000).IsMedal).IsTrue(); // typeindex 114
        await Assert.That(new ItemTemplateId(1_152_000).IsAndroidEquip).IsTrue(); // typeindex 115
    }

    [Test]
    public async Task EquipSlotPredicates_RejectOtherTypeIndexes()
    {
        var hat = new ItemTemplateId(1_002_140); // typeindex 100
        await Assert.That(hat.IsFaceAcc).IsFalse();
        await Assert.That(hat.IsEyeAcc).IsFalse();
        await Assert.That(hat.IsShield).IsFalse();
    }

    // ── Pet / Taming / Vehicle Equip Predicates ───────────────────────────

    [Test]
    public async Task IsPetEquip_MatchesRange()
    {
        await Assert.That(new ItemTemplateId(1_800_000).IsPetEquip).IsTrue();
        await Assert.That(new ItemTemplateId(1_899_999).IsPetEquip).IsTrue();
        await Assert.That(new ItemTemplateId(1_900_000).IsPetEquip).IsFalse();
    }

    [Test]
    public async Task IsTamingMobItem_MatchesTypeIndex190()
    {
        await Assert.That(new ItemTemplateId(1_900_000).IsTamingMobItem).IsTrue();
        await Assert.That(new ItemTemplateId(1_909_999).IsTamingMobItem).IsTrue();
        await Assert.That(new ItemTemplateId(1_910_000).IsTamingMobItem).IsFalse();
    }

    [Test]
    public async Task IsSaddle_MatchesTypeIndex191()
    {
        await Assert.That(new ItemTemplateId(1_910_000).IsSaddle).IsTrue();
        await Assert.That(new ItemTemplateId(1_900_000).IsSaddle).IsFalse();
    }

    [Test]
    public async Task IsDragonWing_MatchesTypeIndex192()
    {
        await Assert.That(new ItemTemplateId(1_920_000).IsDragonWing).IsTrue();
        await Assert.That(new ItemTemplateId(1_930_000).IsDragonWing).IsFalse();
    }

    [Test]
    public async Task IsVehicle_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(1_900_000).IsVehicle).IsTrue(); // typeindex 190
        await Assert.That(new ItemTemplateId(1_930_000).IsVehicle).IsTrue(); // typeindex 193
        await Assert.That(new ItemTemplateId(1_983_000).IsVehicle).IsTrue(); // 1_983_xxx range
        await Assert.That(new ItemTemplateId(1_910_000).IsVehicle).IsFalse(); // saddle ≠ vehicle
    }

    [Test]
    public async Task IsEventVehicleType1_MatchesExactRange()
    {
        await Assert.That(new ItemTemplateId(1_932_001).IsEventVehicleType1).IsTrue();
        await Assert.That(new ItemTemplateId(1_932_002).IsEventVehicleType1).IsTrue();
        await Assert.That(new ItemTemplateId(1_932_000).IsEventVehicleType1).IsFalse();
        await Assert.That(new ItemTemplateId(1_932_003).IsEventVehicleType1).IsFalse();
    }

    [Test]
    public async Task IsEventVehicleType2_MatchesKnownIds()
    {
        await Assert.That(new ItemTemplateId(1_932_004).IsEventVehicleType2).IsTrue();
        await Assert.That(new ItemTemplateId(1_932_040).IsEventVehicleType2).IsTrue();
        await Assert.That(new ItemTemplateId(1_932_003).IsEventVehicleType2).IsFalse(); // gap
        await Assert.That(new ItemTemplateId(1_932_005).IsEventVehicleType2).IsFalse(); // gap
    }

    [Test]
    public async Task IsAndroidPart_MatchesTypeIndexRange()
    {
        await Assert.That(new ItemTemplateId(1_610_000).IsAndroidPart).IsTrue(); // typeindex 161
        await Assert.That(new ItemTemplateId(1_650_000).IsAndroidPart).IsTrue(); // typeindex 165
        await Assert.That(new ItemTemplateId(1_600_000).IsAndroidPart).IsFalse(); // typeindex 160
        await Assert.That(new ItemTemplateId(1_660_000).IsAndroidPart).IsFalse(); // typeindex 166
    }

    // ── Couple / Friendship Equip ─────────────────────────────────────────

    [Test]
    public async Task IsCoupleEquip_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(1_112_001).IsCoupleEquip).IsTrue(); // /100==11120
        await Assert.That(new ItemTemplateId(1_112_000).IsCoupleEquip).IsFalse(); // excluded ID
        await Assert.That(new ItemTemplateId(1_113_000).IsCoupleEquip).IsFalse();
    }

    [Test]
    public async Task IsFriendshipEquip_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(1_112_800).IsFriendshipEquip).IsTrue(); // /100==11128, %10==0
        await Assert.That(new ItemTemplateId(1_112_802).IsFriendshipEquip).IsTrue(); // %10==2
        await Assert.That(new ItemTemplateId(1_112_803).IsFriendshipEquip).IsFalse(); // %10==3
    }

    // ── Weapon Sub-Types ──────────────────────────────────────────────────

    [Test]
    public async Task WeaponSubType_ReturnsTypeIndexMod100()
    {
        await Assert.That(new ItemTemplateId(1_302_000).WeaponSubType).IsEqualTo(30);
        await Assert.That(new ItemTemplateId(1_402_000).WeaponSubType).IsEqualTo(40);
    }

    [Test]
    public async Task IsWeaponSticker_MatchesRange()
    {
        await Assert.That(new ItemTemplateId(1_700_000).IsWeaponSticker).IsTrue();
        await Assert.That(new ItemTemplateId(1_600_000).IsWeaponSticker).IsFalse();
    }

    [Test]
    public async Task IsSkillEffectWeapon_MatchesRange()
    {
        await Assert.That(new ItemTemplateId(1_600_000).IsSkillEffectWeapon).IsTrue();
        await Assert.That(new ItemTemplateId(1_700_000).IsSkillEffectWeapon).IsFalse();
    }

    // ── Pet Item Predicates ───────────────────────────────────────────────

    [Test]
    public async Task PetItemPredicates_MatchTypeIndexes()
    {
        await Assert.That(new ItemTemplateId(2_120_000).IsPetFood).IsTrue(); // typeindex 212
        await Assert.That(new ItemTemplateId(2_130_000).IsPetFood).IsFalse();
        await Assert.That(new ItemTemplateId(1_810_000).IsPetAbility).IsTrue(); // typeindex 181
        await Assert.That(new ItemTemplateId(1_820_000).IsPetAbility).IsFalse();
        await Assert.That(new ItemTemplateId(1_822_000).IsPetRing).IsTrue();
        await Assert.That(new ItemTemplateId(1_832_000).IsPetRing).IsTrue();
        await Assert.That(new ItemTemplateId(1_842_000).IsPetRing).IsFalse();
        await Assert.That(new ItemTemplateId(5_240_000).IsCashPetFood).IsTrue(); // typeindex 524
        await Assert.That(new ItemTemplateId(5_250_000).IsCashPetFood).IsFalse();
    }

    // ── Use/Consume Item Predicates ───────────────────────────────────────

    [Test]
    public async Task IsRechargeable_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(2_070_000).IsRechargeable).IsTrue(); // javelin 207
        await Assert.That(new ItemTemplateId(2_330_000).IsRechargeable).IsTrue(); // pellet 233
        await Assert.That(new ItemTemplateId(2_000_000).IsRechargeable).IsFalse();
    }

    [Test]
    public async Task UseItemPredicates_MatchTypeIndexes()
    {
        await Assert.That(new ItemTemplateId(2_000_000).IsStateChange).IsTrue(); // typeindex 200
        await Assert.That(new ItemTemplateId(2_010_000).IsStateChange).IsTrue(); // typeindex 201
        await Assert.That(new ItemTemplateId(2_030_000).IsPortalScroll).IsTrue(); // typeindex 203
        await Assert.That(new ItemTemplateId(2_280_000).IsSkillLearn).IsTrue(); // typeindex 228
        await Assert.That(new ItemTemplateId(2_290_000).IsSkillLearn).IsTrue(); // typeindex 229 (mastery book)
        await Assert.That(new ItemTemplateId(2_500_000).IsSkillReset).IsTrue(); // typeindex 250
        await Assert.That(new ItemTemplateId(2_100_000).IsMobSummon).IsTrue(); // typeindex 210
        await Assert.That(new ItemTemplateId(2_270_000).IsBridle).IsTrue(); // typeindex 227
        await Assert.That(new ItemTemplateId(2_260_000).IsTamingMobFood).IsTrue(); // typeindex 226
        await Assert.That(new ItemTemplateId(2_210_000).IsMorph).IsTrue(); // typeindex 221
        await Assert.That(new ItemTemplateId(2_310_000).IsShopScanner).IsTrue(); // typeindex 231
        await Assert.That(new ItemTemplateId(2_320_000).IsMapTransfer).IsTrue(); // typeindex 232
        await Assert.That(new ItemTemplateId(2_240_000).IsEngagementRingBox).IsTrue(); // typeindex 224
        await Assert.That(new ItemTemplateId(2_160_000).IsNewYearCardConsume).IsTrue(); // typeindex 216
        await Assert.That(new ItemTemplateId(4_300_000).IsNewYearCardEtc).IsTrue(); // typeindex 430
        await Assert.That(new ItemTemplateId(2_370_000).IsExpUp).IsTrue(); // typeindex 237
        await Assert.That(new ItemTemplateId(4_280_000).IsGachaponBox).IsTrue(); // typeindex 428
        await Assert.That(new ItemTemplateId(4_160_000).IsBook).IsTrue(); // typeindex 416
        await Assert.That(new ItemTemplateId(4_170_000).IsPigmyEgg).IsTrue(); // typeindex 417
        await Assert.That(new ItemTemplateId(4_320_000).IsUiOpen).IsTrue(); // typeindex 432
        await Assert.That(new ItemTemplateId(5_450_000).IsSelectNpc).IsTrue(); // typeindex 545
        await Assert.That(new ItemTemplateId(2_390_000).IsSelectNpc).IsTrue(); // typeindex 239
        await Assert.That(new ItemTemplateId(4_080_000).IsMinigame).IsTrue(); // typeindex 408
        await Assert.That(new ItemTemplateId(4_290_000).IsNonCashEffect).IsTrue(); // typeindex 429
        await Assert.That(new ItemTemplateId(2_190_000).IsAntiMacro).IsTrue(); // typeindex 219
        await Assert.That(new ItemTemplateId(3_010_000).IsPortableChair).IsTrue(); // typeindex 301
    }

    [Test]
    public async Task IsImmediateMobSummon_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(2_109_000).IsImmediateMobSummon).IsTrue(); // /1000==2109
        await Assert.That(new ItemTemplateId(2_100_067).IsImmediateMobSummon).IsTrue(); // exact ID
        await Assert.That(new ItemTemplateId(2_100_000).IsImmediateMobSummon).IsFalse();
    }

    [Test]
    public async Task IsRandomMorphOther_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(2_212_000).IsRandomMorphOther).IsTrue();
        await Assert.That(new ItemTemplateId(2_211_000).IsRandomMorphOther).IsFalse();
        await Assert.That(new ItemTemplateId(2_213_000).IsRandomMorphOther).IsFalse();
    }

    [Test]
    public async Task IsScriptRun_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(2_430_000).IsScriptRun).IsTrue(); // typeindex 243
        await Assert.That(new ItemTemplateId(3_994_225).IsScriptRun).IsTrue(); // exact ID
        await Assert.That(new ItemTemplateId(3_994_226).IsScriptRun).IsFalse();
    }

    [Test]
    public async Task IsRaise_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(4_220_000).IsRaise).IsTrue();
        await Assert.That(new ItemTemplateId(4_220_999).IsRaise).IsTrue();
        await Assert.That(new ItemTemplateId(4_221_000).IsRaise).IsFalse();
    }

    [Test]
    public async Task IsMesoProtect_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(2_250_000).IsMesoProtect).IsTrue();
        await Assert.That(new ItemTemplateId(2_250_099).IsMesoProtect).IsTrue();
        await Assert.That(new ItemTemplateId(2_250_100).IsMesoProtect).IsFalse();
    }

    // ── Upgrade Scroll Predicates ─────────────────────────────────────────

    [Test]
    public async Task UpgradeScrollPredicates_MatchExpected()
    {
        await Assert.That(new ItemTemplateId(2_046_000).IsNewUpgrade).IsTrue();
        await Assert.That(new ItemTemplateId(2_046_999).IsNewUpgrade).IsTrue();
        await Assert.That(new ItemTemplateId(2_047_000).IsNewUpgrade).IsFalse();

        await Assert.That(new ItemTemplateId(2_047_000).IsDurabilityUpgrade).IsTrue();
        await Assert.That(new ItemTemplateId(2_047_999).IsDurabilityUpgrade).IsTrue();

        await Assert.That(new ItemTemplateId(2_049_300).IsHyperUpgrade).IsTrue();
        await Assert.That(new ItemTemplateId(2_049_399).IsHyperUpgrade).IsTrue();
        await Assert.That(new ItemTemplateId(2_049_400).IsHyperUpgrade).IsFalse();

        await Assert.That(new ItemTemplateId(2_049_400).IsItemOptionUpgrade).IsTrue();
        await Assert.That(new ItemTemplateId(2_049_499).IsItemOptionUpgrade).IsTrue();

        await Assert.That(new ItemTemplateId(2_049_200).IsAccUpgrade).IsTrue();
        await Assert.That(new ItemTemplateId(2_049_100).IsBlackUpgrade).IsTrue();

        await Assert.That(new ItemTemplateId(2_460_000).IsRelease).IsTrue(); // typeindex 246
    }

    [Test]
    public async Task IsWhiteScrollNoConsume_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(2_040_727).IsWhiteScrollNoConsume).IsTrue();
        await Assert.That(new ItemTemplateId(2_041_058).IsWhiteScrollNoConsume).IsTrue();
        await Assert.That(new ItemTemplateId(2_049_000).IsWhiteScrollNoConsume).IsTrue(); // /100==20490
        await Assert.That(new ItemTemplateId(2_040_000).IsWhiteScrollNoConsume).IsFalse();
    }

    // ── Cash Item Predicates ──────────────────────────────────────────────

    [Test]
    public async Task CashItemPredicates_MatchTypeIndexes()
    {
        await Assert.That(new ItemTemplateId(5_000_000).IsCashPet).IsTrue(); // typeindex 500
        await Assert.That(new ItemTemplateId(5_300_000).IsCashMorph).IsTrue(); // typeindex 530
        await Assert.That(new ItemTemplateId(9_100_000).IsCashPackage).IsTrue(); // typeindex 910
        await Assert.That(new ItemTemplateId(5_550_000).IsEquipSlotExt).IsTrue(); // typeindex 555
        await Assert.That(new ItemTemplateId(5_500_000).IsExtendExpireDate).IsTrue(); // typeindex 550
    }

    [Test]
    public async Task IsCharSlotInc_MatchesRange()
    {
        await Assert.That(new ItemTemplateId(5_430_000).IsCharSlotInc).IsTrue();
        await Assert.That(new ItemTemplateId(5_430_999).IsCharSlotInc).IsTrue();
        await Assert.That(new ItemTemplateId(5_431_000).IsCharSlotInc).IsFalse();
    }

    [Test]
    public async Task IsSlotInc_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(9_110_000).IsSlotInc).IsTrue(); // typeindex 911
        await Assert.That(new ItemTemplateId(5_430_000).IsSlotInc).IsTrue(); // charslot inc range
        await Assert.That(new ItemTemplateId(5_000_000).IsSlotInc).IsFalse();
    }

    [Test]
    public async Task IsTrunkCountInc_MatchesRange()
    {
        await Assert.That(new ItemTemplateId(9_110_000).IsTrunkCountInc).IsTrue();
        await Assert.That(new ItemTemplateId(9_110_999).IsTrunkCountInc).IsTrue();
        await Assert.That(new ItemTemplateId(9_111_000).IsTrunkCountInc).IsFalse();
    }

    [Test]
    public async Task IsNamingItem_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(5_060_000).IsNamingItem).IsTrue(); // /1000==5060, %10==0
        await Assert.That(new ItemTemplateId(5_060_010).IsNamingItem).IsTrue();
        await Assert.That(new ItemTemplateId(5_060_001).IsNamingItem).IsFalse(); // %10 != 0
    }

    [Test]
    public async Task IsChangeMaplePoint_MatchesExactIds()
    {
        await Assert.That(new ItemTemplateId(5_200_009).IsChangeMaplePoint).IsTrue();
        await Assert.That(new ItemTemplateId(5_200_010).IsChangeMaplePoint).IsTrue();
        await Assert.That(new ItemTemplateId(5_200_008).IsChangeMaplePoint).IsFalse();
    }

    [Test]
    public async Task IsOnlyForPrepaid_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(5_200_000).IsOnlyForPrepaid).IsTrue(); // /10==520000
        await Assert.That(new ItemTemplateId(5_490_000).IsOnlyForPrepaid).IsTrue(); // typeindex 549
        await Assert.That(new ItemTemplateId(5_221_001).IsOnlyForPrepaid).IsTrue(); // exact ID
        await Assert.That(new ItemTemplateId(5_221_002).IsOnlyForPrepaid).IsFalse();
    }

    // ── Wedding Predicates ────────────────────────────────────────────────

    [Test]
    public async Task WeddingPredicates_MatchExpectedIds()
    {
        await Assert.That(new ItemTemplateId(4_031_406).IsInvitationGuest).IsTrue();
        await Assert.That(new ItemTemplateId(4_031_407).IsInvitationGuest).IsTrue();
        await Assert.That(new ItemTemplateId(4_031_408).IsInvitationGuest).IsFalse();

        await Assert.That(new ItemTemplateId(4_031_481).IsWeddingReceipt).IsTrue();
        await Assert.That(new ItemTemplateId(4_031_375).IsWeddingReceipt).IsTrue();
        await Assert.That(new ItemTemplateId(4_031_374).IsWeddingReceipt).IsFalse();

        await Assert.That(new ItemTemplateId(4_031_373).IsWeddingCantDrop).IsTrue();
        await Assert.That(new ItemTemplateId(4_031_374).IsWeddingCantDrop).IsTrue();
        await Assert.That(new ItemTemplateId(4_031_375).IsWeddingCantDrop).IsFalse();
    }

    // ── EquipGender ───────────────────────────────────────────────────────

    [Test]
    [Arguments(1_000_000, 0)] // male equip (gender digit 0)
    [Arguments(1_001_000, 1)] // female equip (gender digit 1)
    [Arguments(1_002_000, 2)] // unisex equip (gender digit 2)
    [Arguments(2_000_001, 2)] // non-equip → always unisex
    public async Task EquipGender_MatchesExpected(int id, int expected)
    {
        await Assert.That(new ItemTemplateId(id).EquipGender).IsEqualTo(expected);
    }

    // ── WzImageName / TryWriteWzImageName ─────────────────────────────────

    [Test]
    public async Task WzImageName_IsPaddedTo8Digits()
    {
#pragma warning disable CS0618 // Testing convenience API
        await Assert.That(new ItemTemplateId(1_002_140).WzImageName).IsEqualTo("01002140.img");
        await Assert.That(new ItemTemplateId(10_000_000).WzImageName).IsEqualTo("10000000.img");
#pragma warning restore CS0618
    }

    [Test]
    public async Task TryWriteWzImageName_WritesCorrectly()
    {
        var buf = new char[12];
        var ok = new ItemTemplateId(1_002_140).TryWriteWzImageName(buf, out int written);
        var result = new string(buf, 0, written);
        await Assert.That(ok).IsTrue();
        await Assert.That(written).IsEqualTo(12);
        await Assert.That(result).IsEqualTo("01002140.img");
    }

    [Test]
    public async Task TryWriteWzImageName_FailsWhenBufferTooSmall()
    {
        var buf = new char[11];
        var ok = new ItemTemplateId(1_002_140).TryWriteWzImageName(buf, out int written);
        await Assert.That(ok).IsFalse();
        await Assert.That(written).IsEqualTo(0);
    }

    // ── ScrollTargetSubType / IsAnyEquipScroll ────────────────────────────

    [Test]
    public async Task ScrollTargetSubType_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(2_040_000).ScrollTargetSubType).IsEqualTo(0); // hat scroll
        await Assert.That(new ItemTemplateId(2_040_800).ScrollTargetSubType).IsEqualTo(8); // gloves scroll
        await Assert.That(new ItemTemplateId(2_000_001).ScrollTargetSubType).IsEqualTo(-1); // not a scroll
    }

    [Test]
    public async Task IsAnyEquipScroll_MatchesExpected()
    {
        await Assert.That(new ItemTemplateId(2_470_000).IsAnyEquipScroll).IsTrue(); // typeindex 247
        await Assert.That(new ItemTemplateId(2_490_000).IsAnyEquipScroll).IsTrue(); // typeindex 249
        await Assert.That(new ItemTemplateId(2_480_000).IsAnyEquipScroll).IsFalse();
    }

    [Test]
    public async Task CanRegularScrollApplyTo_NonEquipTarget_ReturnsFalse()
    {
        var scroll = new ItemTemplateId(2_040_000); // valid scroll
        var potion = new ItemTemplateId(2_000_001); // not equip
        await Assert.That(scroll.CanRegularScrollApplyTo(potion)).IsFalse();
    }

    // ── IsBundle ──────────────────────────────────────────────────────────

    [Test]
    [Arguments(1_002_140, false)] // equip → not bundle
    [Arguments(2_000_001, true)] // use → bundle
    [Arguments(3_010_000, true)] // setup → bundle
    [Arguments(4_000_001, true)] // etc → bundle
    [Arguments(5_000_001, true)] // cash → bundle
    public async Task IsBundle_MatchesExpected(int id, bool expected)
    {
        await Assert.That(new ItemTemplateId(id).IsBundle).IsEqualTo(expected);
    }

    // ── Notable Constants ─────────────────────────────────────────────────

    [Test]
    public async Task WellKnownConstants_HaveCorrectValues()
    {
        await Assert.That(ItemTemplateId.WhiteScroll.Value).IsEqualTo(2_340_000);
        await Assert.That(ItemTemplateId.CharacterSaleA.Value).IsEqualTo(5_431_000);
        await Assert.That(ItemTemplateId.CharacterSaleB.Value).IsEqualTo(5_432_000);
    }

    // ── CashSlotItemType extended coverage ─────────────────────────────────

    [Test]
    [Arguments(5_020_000, 10)] // TypeIndex 502 → 10
    [Arguments(5_030_000, 11)] // TypeIndex 503 → 11
    [Arguments(5_040_000, 22)] // TypeIndex 504 → 22
    [Arguments(5_050_000, 23)] // TypeIndex 505, %10==0 → 23
    [Arguments(5_050_001, 24)] // TypeIndex 505, %10==1 → 24
    [Arguments(5_060_000, 25)] // TypeIndex 506, %10==0 → 25
    [Arguments(5_060_001, 26)] // TypeIndex 506, %10==1 → 26
    [Arguments(5_060_002, 27)] // TypeIndex 506, %10==2 → 27
    [Arguments(5_061_000, 65)] // TypeIndex 506, /1000==5061 → 65
    [Arguments(5_062_000, 74)] // TypeIndex 506, /1000==5062 → 74
    [Arguments(5_071_000, 12)] // TypeIndex 507, thousandths 1 → 12
    [Arguments(5_072_000, 13)] // TypeIndex 507, thousandths 2 → 13
    [Arguments(5_074_000, 45)] // TypeIndex 507, thousandths 4 → 45
    [Arguments(5_075_000, 47)] // TypeIndex 507, thousandths 5, %10==0 → 47
    [Arguments(5_076_000, 14)] // TypeIndex 507, thousandths 6 → 14
    [Arguments(5_077_000, 61)] // TypeIndex 507, thousandths 7 → 61
    [Arguments(5_078_000, 15)] // TypeIndex 507, thousandths 8 → 15
    [Arguments(5_080_000, 18)] // TypeIndex 508 → 18
    [Arguments(5_090_000, 21)] // TypeIndex 509 → 21
    [Arguments(5_100_000, 20)] // TypeIndex 510 → 20
    [Arguments(5_120_000, 16)] // TypeIndex 512 → 16
    [Arguments(5_140_000, 4)] // TypeIndex 514 → 4
    [Arguments(5_150_000, 1)] // TypeIndex 515, /1000==5150 → 1
    [Arguments(5_160_000, 6)] // TypeIndex 516 → 6
    [Arguments(5_180_000, 5)] // TypeIndex 518 → 5
    [Arguments(5_190_000, 28)] // TypeIndex 519 → 28
    [Arguments(5_200_000, 19)] // TypeIndex 520 → 19
    [Arguments(5_220_000, 40)] // TypeIndex 522 → 40
    [Arguments(5_230_000, 29)] // TypeIndex 523 → 29
    [Arguments(5_240_000, 30)] // TypeIndex 524 → 30
    [Arguments(5_280_000, 33)] // TypeIndex 528, /1000==5280 → 33
    [Arguments(5_281_000, 34)] // TypeIndex 528, /1000==5281 → 34
    [Arguments(5_330_000, 31)] // TypeIndex 533 → 31
    [Arguments(5_370_000, 32)] // TypeIndex 537 → 32
    [Arguments(5_380_000, 42)] // TypeIndex 538 → 42
    [Arguments(5_390_000, 43)] // TypeIndex 539 → 43
    [Arguments(5_400_000, 53)] // TypeIndex 540, /1000==5400 → 53
    [Arguments(5_401_000, 54)] // TypeIndex 540, /1000==5401 → 54
    [Arguments(5_420_000, 55)] // TypeIndex 542, /1000==5420 → 55
    [Arguments(5_450_000, 38)] // TypeIndex 545, /1000 != 5451 → 38
    [Arguments(5_451_000, 60)] // TypeIndex 545, /1000==5451 → 60
    [Arguments(5_460_000, 58)] // TypeIndex 546 → 58
    [Arguments(5_470_000, 39)] // TypeIndex 547 → 39
    [Arguments(5_490_000, 59)] // TypeIndex 549 → 59
    [Arguments(5_500_000, 62)] // TypeIndex 550 → 62
    [Arguments(5_510_000, 63)] // TypeIndex 551 → 63
    [Arguments(5_520_000, 64)] // TypeIndex 552 → 64
    [Arguments(5_530_000, 72)] // TypeIndex 553 → 72
    [Arguments(5_570_000, 67)] // TypeIndex 557 → 67
    [Arguments(5_610_000, 71)] // TypeIndex 561 → 71
    [Arguments(5_620_000, 73)] // TypeIndex 562 → 73
    [Arguments(5_640_000, 77)] // TypeIndex 564 → 77
    [Arguments(5_660_000, 78)] // TypeIndex 566 → 78
    [Arguments(5_300_000, 41)] // TypeIndex 530 → 41
    public async Task CashSlotItemType_ExtendedCoverage(int id, int expected)
    {
        await Assert.That(new ItemTemplateId(id).CashSlotItemType).IsEqualTo(expected);
    }

    // ── FrozenSet field coverage ──────────────────────────────────────────

    [Test]
    public async Task AdventureRings_ContainsExpectedIds()
    {
        await Assert.That(ItemTemplateId.AdventureRings.Count).IsEqualTo(5);
        await Assert.That(ItemTemplateId.AdventureRings.Contains(new(1_112_427))).IsTrue();
    }

    [Test]
    public async Task PetRings_AgreesWithIsPetRing()
    {
        foreach (var id in ItemTemplateId.PetRings)
            await Assert.That(id.IsPetRing).IsTrue();
    }

    [Test]
    public async Task InvitationBundles_AgreesWithPredicate()
    {
        foreach (var id in ItemTemplateId.InvitationBundles)
            await Assert.That(id.IsInvitationBundle).IsTrue();
    }

    [Test]
    public async Task InvitationGuests_AgreesWithPredicate()
    {
        foreach (var id in ItemTemplateId.InvitationGuests)
            await Assert.That(id.IsInvitationGuest).IsTrue();
    }

    [Test]
    public async Task WeddingReceiptItems_AgreesWithPredicate()
    {
        foreach (var id in ItemTemplateId.WeddingReceiptItems)
            await Assert.That(id.IsWeddingReceipt).IsTrue();
    }

    [Test]
    public async Task WeddingCantDropItems_AgreesWithPredicate()
    {
        foreach (var id in ItemTemplateId.WeddingCantDropItems)
            await Assert.That(id.IsWeddingCantDrop).IsTrue();
    }

    [Test]
    public async Task DojoTickets_HaveExpectedCounts()
    {
        await Assert.That(ItemTemplateId.DojoTicketsEvanBlocked.Count).IsEqualTo(4);
        await Assert.That(ItemTemplateId.DojoTicketsNonEvanBlocked.Count).IsEqualTo(5);
    }
}
