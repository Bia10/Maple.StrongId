using System.Collections.Frozen;

namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for an item template ID.
/// </summary>
[StrongIntId]
public readonly partial record struct ItemTemplateId(int Value)
{
    // ── Digit Decomposition ───────────────────────────────────────────────

    /// <summary>
    /// Top-level category digit: nItemID / 1_000_000.
    /// 1=Equip, 2=Use, 3=SetUp, 4=Etc, 5=Cash.
    /// </summary>
    public int Category => Value / 1_000_000;

    /// <summary>
    /// TypeIndex = nItemID / 10_000. Encodes equip slot, use sub-type, etc.
    /// </summary>
    public int TypeIndex => Value / 10_000;

    /// <summary>Sub-type discriminator within typeindex: typeindex % 100.</summary>
    public int SubType => TypeIndex % 100;

    /// <summary>
    /// Gender digit: (nItemID / 1_000) % 10. 0=Male, 1=Female, 2+=Unisex.
    /// Only meaningful for equip items (Category == 1).
    /// </summary>
    public int GenderDigit => (Value / 1_000) % 10;

    // ── Category Predicates ───────────────────────────────────────────────

    /// <summary>True for equip items (1_000_000 – 1_999_999).</summary>
    public bool IsEquip => Category == 1;

    /// <summary>True for use/consume items (2_000_000 – 2_999_999).</summary>
    public bool IsConsume => Category == 2;

    /// <summary>True for setup/install items (3_000_000 – 3_999_999).</summary>
    public bool IsSetup => Category == 3;

    /// <summary>True for Etc/quest items (4_000_000 – 4_999_999).</summary>
    public bool IsEtc => Category == 4;

    /// <summary>True for cash items (5_000_000 – 5_999_999).</summary>
    public bool IsCash => Category == 5;

    // ── Equip Slot Predicates ─────────────────────────────────────────────

    /// <summary>Hat / Cap items (typeindex 100 → bodyPart 1)</summary>
    public bool IsHat => TypeIndex == 100;

    /// <summary>Face accessory items (typeindex 101 → bodyPart 2).</summary>
    public bool IsFaceAcc => TypeIndex == 101;

    /// <summary>Eye accessory items (typeindex 102 → bodyPart 3).</summary>
    public bool IsEyeAcc => TypeIndex == 102;

    /// <summary>Earring items (typeindex 103 → bodyPart 4).</summary>
    public bool IsEarring => TypeIndex == 103;

    /// <summary>Top (shirt) items only — does NOT include Overall (typeindex 104 → bodyPart 5).</summary>
    public bool IsTop => TypeIndex == 104;

    /// <summary>True for Overall items (typeindex 105)</summary>
    public bool IsLongCoat => TypeIndex == 105;

    /// <summary>True for Top or Overall items (typeindex 104 or 105).</summary>
    public bool IsTopOrOverall => TypeIndex is 104 or 105;

    /// <summary>Bottom / pants items (typeindex 106 → bodyPart 6).</summary>
    public bool IsBottom => TypeIndex == 106;

    /// <summary>Shoes / footwear items (typeindex 107 → bodyPart 7).</summary>
    public bool IsShoes => TypeIndex == 107;

    /// <summary>Gloves items (typeindex 108 → bodyPart 8).</summary>
    public bool IsGloves => TypeIndex == 108;

    /// <summary>True for shield items (typeindex 109 → bodyPart 10)</summary>
    public bool IsShield => TypeIndex == 109;

    /// <summary>Cape items (typeindex 110 → bodyPart 9).</summary>
    public bool IsCape => TypeIndex == 110;

    /// <summary>Ring items (typeindex 111 → bodyParts 12/13/15/16, up to 4 ring slots).</summary>
    public bool IsRing => TypeIndex == 111;

    /// <summary>Pendant items (typeindex 112 → bodyParts 17 and 59 — ext pendant slot).</summary>
    public bool IsPendant => TypeIndex == 112;

    /// <summary>Belt items (typeindex 113 → bodyPart 50).</summary>
    public bool IsBelt => TypeIndex == 113;

    /// <summary>Medal items (typeindex 114 → bodyPart 49).</summary>
    public bool IsMedal => TypeIndex == 114;

    /// <summary>Android/Totem items (typeindex 115 → bodyPart 51).</summary>
    public bool IsAndroidEquip => TypeIndex == 115;

    /// <summary>
    /// True for pet equip items: all items in range 1_800_000 – 1_899_999.
    /// This INCLUDES pet ability scrolls (typeindex 181, is_pet_ability_item) and pet rings.
    /// Formula: nItemID / 100_000 == 18.
    /// </summary>
    public bool IsPetEquip => Value / 100_000 == 18;

    /// <summary>
    /// True for taming mob (mount) items (typeindex 190 → bodyPart 18; 1_900_000 – 1_909_999).
    /// </summary>
    public bool IsTamingMobItem => TypeIndex == 190;

    /// <summary>Saddle items (typeindex 191 → bodyPart 19)</summary>
    public bool IsSaddle => TypeIndex == 191;

    /// <summary>Dragon wing / glider items (typeindex 192 → bodyPart 20; Evan dragon riding).</summary>
    public bool IsDragonWing => TypeIndex == 192;

    /// <summary>
    /// True for saddle-pack vehicle equip items (typeindex 193 → 1_930_000–1_939_999).
    /// These occupy a second vehicle-equip slot distinct from the taming-mob item slot (typeindex 190).
    /// </summary>
    public bool IsVehicleEquipB => TypeIndex == 193;

    /// <summary>
    /// True for vehicle/mount items: TamingMob (typeindex 190), vehicle equip B (typeindex 193),
    /// or special mount IDs in the 1_983_xxx range.
    /// Evan dragon riding IDs 1_902_040–1_902_042 are also covered by <see cref="IsTamingMobItem"/>.
    /// </summary>
    public bool IsVehicle => IsTamingMobItem || IsVehicleEquipB || Value / 1_000 == 1_983;

    /// <summary>
    /// True for Evan dragon riding items: exactly IDs 1_902_040, 1_902_041, or 1_902_042.
    /// These fall within IsTamingMobItem (TypeIndex==190) but have unique wing/glider slot handling.
    /// </summary>
    public bool IsEvanDragonRidingItem => EvanDragonRidingItems.Contains(this);

    /// <summary>
    /// True for event vehicle type-1 items: IDs 1_932_001 – 1_932_002.
    /// </summary>
    public bool IsEventVehicleType1 => (uint)(Value - 1_932_001) <= 1;

    // 28 specific event vehicle type-2 item IDs. These are non-contiguous within the 1_932_xxx range;
    // each ID maps to a unique event-mount skill serial.
    private static readonly FrozenSet<ItemTemplateId> s_eventVehicleType2Ids = FrozenSet.ToFrozenSet<ItemTemplateId>([
        new(1_932_004),
        new(1_932_006),
        new(1_932_007),
        new(1_932_008),
        new(1_932_009),
        new(1_932_010),
        new(1_932_011),
        new(1_932_012),
        new(1_932_013),
        new(1_932_014),
        new(1_932_017),
        new(1_932_018),
        new(1_932_019),
        new(1_932_020),
        new(1_932_021),
        new(1_932_022),
        new(1_932_023),
        new(1_932_025),
        new(1_932_026),
        new(1_932_027),
        new(1_932_028),
        new(1_932_029),
        new(1_932_034),
        new(1_932_035),
        new(1_932_037),
        new(1_932_038),
        new(1_932_039),
        new(1_932_040),
    ]);

    /// <summary>
    /// True for event vehicle type-2 items: 28 specific IDs in the 1_932_xxx range.
    /// Each type-2 vehicle has an associated event-mount skill with a unique serial.
    /// Unlike type-1 (<see cref="IsEventVehicleType1"/>), the type-2 IDs are non-contiguous.
    /// </summary>
    public bool IsEventVehicleType2 => s_eventVehicleType2Ids.Contains(this);

    /// <summary>
    /// Android head accessory (typeindex 161 → bodyPart 1100) through android outfit (165 → 1104).
    /// Covers all android-specific equip slot items.
    /// </summary>
    public bool IsAndroidPart => TypeIndex is >= 161 and <= 165;

    // ── Couple / Friendship Equip ─────────────────────────────────────────

    /// <summary>
    /// True for couple equip ring items: nItemID / 100 == 11120, excluding 1_112_000.
    /// </summary>
    public bool IsCoupleEquip => Value / 100 == 11120 && Value != 1_112_000;

    /// <summary>
    /// True for friendship equip ring items: nItemID / 100 == 11128 and nItemID % 10 ≤ 2.
    /// </summary>
    public bool IsFriendshipEquip => Value / 100 == 11128 && Value % 10 <= 2;

    // ── Weapon Predicates ─────────────────────────────────────────────────

    /// <summary>
    /// True for character weapon items: typeindex / 10 ∈ {13, 14, 16, 17}.
    /// Covers typeindices 130–139, 140–149, 160–169, 170–179 (weapon slots bodyPart 11/14).
    /// </summary>
    public bool IsWeapon => TypeIndex / 10 is 13 or 14 or 16 or 17;

    /// <summary>One-handed weapons: typeindex % 100 ∈ 30–39</summary>
    public bool IsOneHandedWeapon => IsWeapon && SubType is >= 30 and <= 39;

    /// <summary>Two-handed weapons: typeindex % 100 ∈ 40–49</summary>
    public bool IsTwoHandedWeapon => IsWeapon && SubType is >= 40 and <= 49;

    /// <summary>
    /// Weapon sub-type code (typeindex % 100).
    /// Only meaningful when IsWeapon.
    /// </summary>
    public int WeaponSubType => TypeIndex % 100;

    /// <summary>
    /// True for weapon sticker items (nItemID / 100_000 == 17 → range 1_700_000–1_799_999).
    /// </summary>
    public bool IsWeaponSticker => Value / 100_000 == 17;

    /// <summary>
    /// True for skill-effect weapon items (nItemID / 100_000 == 16 → range 1_600_000–1_699_999).
    /// </summary>
    public bool IsSkillEffectWeapon => Value / 100_000 == 16;

    // ── Pet Item Predicates ───────────────────────────────────────────────

    /// <summary>
    /// True for pet food items (typeindex 212 → 2_120_000 – 2_129_999).
    /// </summary>
    public bool IsPetFood => TypeIndex == 212;

    /// <summary>
    /// True for pet ability scroll items (typeindex 181 → 1_810_000 – 1_819_999).
    /// Note: IsPetAbility implies IsPetEquip (same 18xxxxx range).
    /// </summary>
    public bool IsPetAbility => TypeIndex == 181;

    /// <summary>
    /// True for pet ring items — exactly IDs 1_822_000 and 1_832_000.
    /// </summary>
    public bool IsPetRing => PetRings.Contains(this);

    /// <summary>
    /// True for Cash pet food items (typeindex 524 → Cash tab).
    /// </summary>
    public bool IsCashPetFood => TypeIndex == 524;

    // ── Use/Consume Item Predicates ───────────────────────────────────────

    /// <summary>
    /// True for rechargeable projectile items: javelin (typeindex 207) or pellet (typeindex 233).
    /// </summary>
    public bool IsRechargeable => TypeIndex is 207 or 233;

    /// <summary>True for state-change (buff/potion) items: typeindices 200, 201, 202, 205, 221, 236, 238, 245.
    /// </summary>
    public bool IsStateChange => TypeIndex is 200 or 201 or 202 or 205 or 221 or 236 or 238 or 245;

    /// <summary>True for portal scroll / return scroll items (typeindex 203)</summary>
    public bool IsPortalScroll => TypeIndex == 203;

    /// <summary>
    /// True for mastery book items: typeindex 229 or 562 (all variants including 5_620_007/5_620_008).
    /// </summary>
    public bool IsMasteryBook => TypeIndex is 229 or 562;

    /// <summary>
    /// True for dual mastery book items: typeindex 562, excluding IDs 5_620_007 and 5_620_008.
    /// IDs 5_620_007/5_620_008 are regular mastery books, NOT dual mastery books.
    /// </summary>
    public bool IsDualMasteryBook => TypeIndex == 562 && Value is not (5_620_007 or 5_620_008);

    /// <summary>
    /// True for skill book items (typeindex 228) or any mastery book.
    /// </summary>
    public bool IsSkillLearn => TypeIndex == 228 || IsMasteryBook;

    /// <summary>True for skill reset items (typeindex 250)</summary>
    public bool IsSkillReset => TypeIndex == 250;

    /// <summary>True for mob summon sack items (typeindex 210)</summary>
    public bool IsMobSummon => TypeIndex == 210;

    /// <summary>
    /// True for immediate-use mob summon bags: nItemID / 1_000 == 2109 or nItemID == 2_100_067.
    /// </summary>
    public bool IsImmediateMobSummon => Value / 1_000 == 2109 || Value == 2_100_067;

    /// <summary>True for bridle/mount-capture items (typeindex 227)</summary>
    public bool IsBridle => TypeIndex == 227;

    /// <summary>True for taming mob food items (typeindex 226)</summary>
    public bool IsTamingMobFood => TypeIndex == 226;

    /// <summary>True for morph/transform items (typeindex 221)</summary>
    public bool IsMorph => TypeIndex == 221;

    /// <summary>
    /// True for "other random morph" items: typeindex 221 and IDs in the 2212xxx range.
    /// Formula: typeindex == 221 AND (nItemID - 2210000) / 1000 == 2.
    /// </summary>
    public bool IsRandomMorphOther => TypeIndex == 221 && (Value - 2_210_000) / 1_000 == 2;

    /// <summary>True for shop scanner items (typeindex 231)</summary>
    public bool IsShopScanner => TypeIndex == 231;

    /// <summary>True for map transfer items (typeindex 232)</summary>
    public bool IsMapTransfer => TypeIndex == 232;

    /// <summary>True for engagement ring box items (typeindex 224)</summary>
    public bool IsEngagementRingBox => TypeIndex == 224;

    /// <summary>True for new year card (consume, typeindex 216)</summary>
    public bool IsNewYearCardConsume => TypeIndex == 216;

    /// <summary>True for new year card (Etc, typeindex 430)</summary>
    public bool IsNewYearCardEtc => TypeIndex == 430;

    /// <summary>True for EXP accumulation items (typeindex 237)</summary>
    public bool IsExpUp => TypeIndex == 237;

    /// <summary>True for gachapon box items (typeindex 428)</summary>
    public bool IsGachaponBox => TypeIndex == 428;

    /// <summary>True for book (monster card / encyclopedia) items (typeindex 416)</summary>
    public bool IsBook => TypeIndex == 416;

    /// <summary>True for pigmy egg items (typeindex 417)</summary>
    public bool IsPigmyEgg => TypeIndex == 417;

    /// <summary>True for UI-open cash items (typeindex 432)</summary>
    public bool IsUiOpen => TypeIndex == 432;

    /// <summary>True for select-NPC items: typeindex 545 or 239</summary>
    public bool IsSelectNpc => TypeIndex is 545 or 239;

    /// <summary>True for mini-game items (typeindex 408)</summary>
    public bool IsMinigame => TypeIndex == 408;

    /// <summary>True for non-cash effect items (typeindex 429)</summary>
    public bool IsNonCashEffect => TypeIndex == 429;

    /// <summary>True for script-run items: typeindex 243 or the exact ID 3_994_225</summary>
    public bool IsScriptRun => TypeIndex == 243 || Value == 3_994_225;

    /// <summary>True for anti-macro items (typeindex 219).</summary>
    public bool IsAntiMacro => TypeIndex == 219;

    /// <summary>
    /// True for raise / hatch items (nItemID / 1_000 == 4220).
    /// </summary>
    public bool IsRaise => Value / 1_000 == 4220;

    /// <summary>
    /// True for meso protect items (nItemID / 100 == 22500 → 2_250_000–2_250_099).
    /// </summary>
    public bool IsMesoProtect => Value / 100 == 22500;

    // ── Setup Item Predicates ─────────────────────────────────────────────

    /// <summary>True for portable chair items (typeindex 301)</summary>
    public bool IsPortableChair => TypeIndex == 301;

    // ── Upgrade Scroll Predicates ─────────────────────────────────────────

    /// <summary>
    /// True for new-type upgrade scrolls (Value / 1_000 == 2046 → 2_046_000–2_046_999).
    /// Covers Chaos/Innocent scroll-style upgrades.
    /// </summary>
    public bool IsNewUpgrade => Value / 1_000 == 2046;

    /// <summary>
    /// True for durability upgrade items (nItemID / 1_000 == 2047 → 2_047_000–2_047_999).
    /// </summary>
    public bool IsDurabilityUpgrade => Value / 1_000 == 2047;

    /// <summary>
    /// True for Hyper (Chaos) upgrade stones (nItemID / 100 == 20493 → 2_049_300–2_049_399).
    /// </summary>
    public bool IsHyperUpgrade => Value / 100 == 20493;

    /// <summary>
    /// True for item option (potential) upgrade items (nItemID / 100 == 20494 → 2_049_400–2_049_499).
    /// </summary>
    public bool IsItemOptionUpgrade => Value / 100 == 20494;

    /// <summary>
    /// True for accessory upgrade items (nItemID / 100 == 20492 → 2_049_200–2_049_299).
    /// </summary>
    public bool IsAccUpgrade => Value / 100 == 20492;

    /// <summary>
    /// True for black upgrade items (nItemID / 100 == 20491 → 2_049_100–2_049_199).
    /// </summary>
    public bool IsBlackUpgrade => Value / 100 == 20491;

    /// <summary>
    /// True for release-seal items (typeindex 246). Removes equip seals.
    /// </summary>
    public bool IsRelease => TypeIndex == 246;

    /// <summary>
    /// True for white-scroll-no-consume upgrade scrolls: IDs 2_040_727, 2_041_058, or nItemID / 100 == 20490.
    /// </summary>
    public bool IsWhiteScrollNoConsume => Value is 2_040_727 or 2_041_058 || Value / 100 == 20490;

    // ── Cash Item Predicates ──────────────────────────────────────────────

    /// <summary>
    /// True for Cash pet items (typeindex 500 → 5_000_000–5_009_999).
    /// ⚠ Pet TEMPLATE IDs (MobTemplateId) use this same range, but here it's the Cash item type.
    /// </summary>
    public bool IsCashPet => TypeIndex == 500;

    /// <summary>True for Cash morph items (typeindex 530)</summary>
    public bool IsCashMorph => TypeIndex == 530;

    /// <summary>True for Cash package (bundle box) items (typeindex 910)</summary>
    public bool IsCashPackage => TypeIndex == 910;

    /// <summary>
    /// True for character slot increase items (nItemID / 1_000 == 5430 → 5_430_000–5_430_999).
    /// </summary>
    public bool IsCharSlotInc => Value / 1_000 == 5430;

    /// <summary>
    /// True for equip slot extension items (typeindex 555 → 5_550_000–5_559_999).
    /// </summary>
    public bool IsEquipSlotExt => TypeIndex == 555;

    /// <summary>
    /// True for slot increase items: typeindex 911 or nItemID / 1_000 == 5430.
    /// </summary>
    public bool IsSlotInc => TypeIndex == 911 || Value / 1_000 == 5430;

    /// <summary>
    /// True for trunk slot increase items (nItemID / 1_000 == 9110 → 9_110_000–9_110_999).
    /// </summary>
    public bool IsTrunkCountInc => Value / 1_000 == 9110;

    /// <summary>
    /// True for naming / item rename items: nItemID / 1_000 == 5060 AND nItemID % 10 == 0.
    /// </summary>
    public bool IsNamingItem => Value / 1_000 == 5060 && Value % 10 == 0;

    /// <summary>True for extend expire date items (typeindex 550).</summary>
    public bool IsExtendExpireDate => TypeIndex == 550;

    /// <summary>
    /// True for MaplePoint exchange items: exactly IDs 5_200_009 and 5_200_010.
    /// </summary>
    public bool IsChangeMaplePoint => Value is 5_200_009 or 5_200_010;

    /// <summary>
    /// True for prepaid-only items: nItemID / 10 == 520000 (5_200_000–5_200_009), typeindex 549, or nItemID == 5_221_001.
    /// </summary>
    public bool IsOnlyForPrepaid => Value / 10 == 520000 || TypeIndex == 549 || Value == 5_221_001;

    /// <summary>
    /// Cash shop inventory slot-type code for this item.
    /// Returns 0 for non-cash items or unrecognized sub-types.
    /// The 540/542/543/546 typeindex cluster shares a signed-integer fallthrough in the switch.
    /// ⚠ NOTE: This retarded monstrosity comes straight from the original C++ client source, do not touch! ⚠
    /// </summary>
    public int CashSlotItemType
    {
        get
        {
            switch (TypeIndex)
            {
                case 500:
                    return 8;
                case 501:
                    return 9;
                case 502:
                    return 10;
                case 503:
                    return 11;
                case 504:
                    return 22;
                case 505:
                {
                    int r = Value % 10;
                    return r == 0 ? 23 : ((uint)(r - 1) > 8u ? 0 : 24);
                }
                case 506:
                {
                    int k = Value / 1_000;
                    if (k == 5061)
                        return 65;
                    if (k == 5062)
                        return 74;
                    return (Value % 10) switch
                    {
                        0 => 25,
                        1 => 26,
                        2 or 3 => 27,
                        _ => 0,
                    };
                }
                case 507:
                    return (Value % 10_000 / 1_000) switch
                    {
                        1 => 12,
                        2 => 13,
                        4 => 45,
                        5 => (Value % 10) switch
                        {
                            0 => 47,
                            1 => 48,
                            2 => 49,
                            3 => 50,
                            4 => 51,
                            5 => 52,
                            _ => 14,
                        },
                        6 => 14,
                        7 => 61,
                        8 => 15,
                        _ => 0,
                    };
                case 508:
                    return 18;
                case 509:
                    return 21;
                case 510:
                    return 20;
                case 512:
                    return 16;
                case 513:
                    return 7;
                case 514:
                    return 4;
                case 515:
                {
                    int k = Value / 1_000;
                    if (k is 5150 or 5151 or 5154)
                        return 1;
                    if (k == 5152)
                        return Value / 100 == 51520 ? 2 : (Value / 100 == 51521 ? 35 : 0);
                    if (k == 5153)
                        return 3;
                    return 0;
                }
                case 516:
                    return 6;
                case 517:
                    // Returns 17 only when the item ID is an exact multiple of 10_000. All others return 0.
                    return Value % 10_000 == 0 ? 17 : 0;
                case 518:
                    return 5;
                case 519:
                    return 28;
                case 520:
                    return 19;
                case 522:
                    return 40;
                case 523:
                    return 29;
                case 524:
                    return 30;
                case 525:
                    // Only item 5_251_100 is assigned slot type 37; all others in this typeindex use 36.
                    return Value == 5_251_100 ? 37 : 36;
                case 528:
                {
                    int k = Value / 1_000;
                    return k == 5280 ? 33 : (k == 5281 ? 34 : 0);
                }
                case 530:
                    return 41;
                case 533:
                    return 31;
                case 537:
                    return 32;
                case 538:
                    return 42;
                case 539:
                    return 43;
                case 540:
                {
                    int k = Value / 1_000;
                    if (k == 5400)
                        return 53;
                    if (k == 5401)
                        return 54;
                    // Falls through to 542/543/546 chain logic.
                    if (k == 5420)
                        return 55;
                    // Signed comparison from C source: k - 5431 <= 1 ≡ k <= 5432 for positive k.
                    return k - 5431 <= 1 ? 66 : 58;
                }
                case 542:
                {
                    int k = Value / 1_000;
                    if (k == 5420)
                        return 55;
                    // Signed comparison from C source: k - 5431 <= 1 ≡ k <= 5432 for positive k.
                    return k - 5431 <= 1 ? 66 : 58;
                }
                case 543:
                {
                    int k = Value / 1_000;
                    // Signed comparison from C source: k - 5431 <= 1 ≡ k <= 5432 for positive k.
                    return k - 5431 <= 1 ? 66 : 58;
                }
                case 545:
                    return Value / 1_000 == 5451 ? 60 : 38;
                case 546:
                    return 58;
                case 547:
                    return 39;
                case 549:
                    return 59;
                case 550:
                    return 62;
                case 551:
                    return 63;
                case 552:
                    return 64;
                case 553:
                    return 72;
                case 557:
                    return 67;
                case 561:
                    return 71;
                case 562:
                    return 73;
                case 564:
                    return 77;
                case 566:
                    return 78;
                default:
                    return 0;
            }
        }
    }

    // ── Wedding Item Predicates ───────────────────────────────────────────

    /// <summary>
    /// True for wedding invitation bundle items: IDs 4_031_377 or 4_031_395.
    /// </summary>
    public bool IsInvitationBundle => InvitationBundles.Contains(this);

    /// <summary>
    /// True for wedding invitation guest items: IDs 4_031_406 or 4_031_407.
    /// </summary>
    public bool IsInvitationGuest => InvitationGuests.Contains(this);

    /// <summary>
    /// True for wedding receipt items: IDs 4_031_481, 4_031_376, 4_031_480, or 4_031_375.
    /// </summary>
    public bool IsWeddingReceipt => WeddingReceiptItems.Contains(this);

    /// <summary>
    /// True for wedding items that cannot be dropped: IDs 4_031_373 or 4_031_374.
    /// </summary>
    public bool IsWeddingCantDrop => WeddingCantDropItems.Contains(this);

    /// <summary>
    /// True for engagement ring equip items: IDs 4_031_357 – 4_031_364 (8 IDs, sequential).
    /// </summary>
    public bool IsEngagementRingItem => EngagementRings.Contains(this);

    /// <summary>
    /// True for wedding ring equip items: IDs 1_112_803, 1_112_806, 1_112_807, 1_112_809.
    /// </summary>
    public bool IsWeddingRingItem => WeddingRings.Contains(this);

    // ── Special Item Predicates ───────────────────────────────────────────

    /// <summary>
    /// True for special items (nItemID / 100_000 == 91 → 9_100_000–9_199_999).
    /// </summary>
    public bool IsSpecialItem => Value / 100_000 == 91;

    // ── Stacking Predicate ────────────────────────────────────────────────

    /// <summary>
    /// True when the item is treated as a single non-stackable unit:
    /// equip or Cash items (category 1 or 5) OR any non-Use/Setup/Etc category, OR rechargeable projectiles.
    /// </summary>
    public bool IsNonStackable => Category is not (2 or 3 or 4) || IsRechargeable;

    // ── Gender Predicate ──────────────────────────────────────────────────

    /// <summary>
    /// Gender of an equip item.  0=Male, 1=Female, 2=Unisex (also returned for non-equips).
    /// </summary>
    public int EquipGender =>
        IsEquip
            ? GenderDigit switch
            {
                0 => 0,
                1 => 1,
                _ => 2,
            }
            : 2;

    // ── WZ Path Helper ────────────────────────────────────────────────────

    /// <summary>
    /// WZ image filename for this item, as it appears in the archive.
    /// All items: "{id:D8}.img" — equip IDs (7 digits) are naturally zero-padded to 8.
    /// </summary>
    [Obsolete("Allocates. Use TryWriteWzImageName(Span<char>, out int) on hot paths.")]
    public string WzImageName => $"{Value:D8}.img";

    /// <summary>
    /// Writes the WZ image name (<c>"{id:D8}.img"</c>) into the provided span without heap allocation.
    /// </summary>
    public bool TryWriteWzImageName(Span<char> destination, out int charsWritten)
    {
        return WzFormatHelper.TryWriteD8ImgName(Value, destination, out charsWritten);
    }

    // ── Inventory Tab ─────────────────────────────────────────────────────

    /// <summary>
    /// Inventory bag tab index for this item.
    /// Mapping: Equip(1)→0, Use(2)→1, Setup(3)→3, Etc(4)→3, Cash(5)→4. Returns -1 for invalid.
    /// Note: Setup and Etc share tab index 3.
    /// </summary>
    public int InventoryTab =>
        Category switch
        {
            1 => 0, // Equip
            2 => 1, // Use
            3 => 3, // Setup
            4 => 3, // Etc (shares Setup tab)
            5 => 4, // Cash
            _ => -1,
        };

    // ── Upgrade Scroll ↔ Equip Pairing ────────────────────────────────────

    /// <summary>
    /// For upgrade scrolls with TypeIndex 204 (IDs 2_040_000 – 2_049_999):
    /// the equip <c>TypeIndex % 100</c> sub-type that this scroll targets.
    /// Formula: (nScrollID - 2_040_000) / 100.
    /// Returns -1 when TypeIndex != 204 (i.e. this item is not a regular upgrade scroll).
    /// ⚠ Sub-types 90–99 within this range (IsHyperUpgrade, IsItemOptionUpgrade, IsAccUpgrade,
    ///   IsBlackUpgrade, IsNewUpgrade, IsDurabilityUpgrade) have special applicability overrides
    ///   that bypass the standard sub-type match — handle those separately.
    /// </summary>
    public int ScrollTargetSubType => TypeIndex == 204 ? (Value - 2_040_000) / 100 : -1;

    /// <summary>
    /// True for "any equip" upgrade scrolls: TypeIndex 247 (IDs 2_470_000–2_479_999) or
    /// TypeIndex 249 (IDs 2_490_000–2_499_999). These apply to any equip regardless of slot.
    /// </summary>
    public bool IsAnyEquipScroll => TypeIndex is 247 or 249;

    /// <summary>
    /// True when this standard upgrade scroll can be applied to the given equip item,
    /// using the core pairing rule: <c>ScrollTargetSubType == equip.TypeIndex % 100</c>.
    /// Returns false when ScrollTargetSubType is -1 (not a TypeIndex-204 scroll).
    /// ⚠ Does NOT cover IsHyperUpgrade, IsItemOptionUpgrade, IsAccUpgrade, IsNewUpgrade,
    ///   IsDurabilityUpgrade, or IsAnyEquipScroll — those have their own applicability rules.
    /// </summary>
    public bool CanRegularScrollApplyTo(ItemTemplateId equip)
    {
        if (!equip.IsEquip)
            return false;
        int sub = ScrollTargetSubType;
        if (sub < 0)
            return false;
        return equip.TypeIndex % 100 == sub;
    }

    // ── Bundle Check ──────────────────────────────────────────────────────

    /// <summary>
    /// True for items in the standard bundle (non-equip) format:
    /// category 2 (Consume), 3 (Install), 4 (Etc), or 5 (Cash).
    /// </summary>
    public bool IsBundle => Category is >= 2 and <= 5;

    // ── Notable Item ID Constants ─────────────────────────────────────────

    /// <summary>White Scroll — prevents equipment destruction on scroll failure. Game-wide special item.</summary>
    public static readonly ItemTemplateId WhiteScroll = new(2_340_000);

    /// <summary>Character sale item A</summary>
    public static readonly ItemTemplateId CharacterSaleA = new(5_431_000);

    /// <summary>Character sale item B</summary>
    public static readonly ItemTemplateId CharacterSaleB = new(5_432_000);

    /// <summary>
    /// Engagement ring item IDs (4_031_357 – 4_031_364).
    /// </summary>
    public static readonly FrozenSet<ItemTemplateId> EngagementRings = FrozenSet.ToFrozenSet<ItemTemplateId>([
        new(4_031_357),
        new(4_031_358),
        new(4_031_359),
        new(4_031_360),
        new(4_031_361),
        new(4_031_362),
        new(4_031_363),
        new(4_031_364),
    ]);

    /// <summary>
    /// Wedding ring item IDs.
    /// </summary>
    public static readonly FrozenSet<ItemTemplateId> WeddingRings = FrozenSet.ToFrozenSet<ItemTemplateId>([
        new(1_112_803),
        new(1_112_806),
        new(1_112_807),
        new(1_112_809),
    ]);

    /// <summary>
    /// Adventure ring item IDs.
    /// </summary>
    public static readonly FrozenSet<ItemTemplateId> AdventureRings = FrozenSet.ToFrozenSet<ItemTemplateId>([
        new(1_112_427),
        new(1_112_428),
        new(1_112_429),
        new(1_112_405),
        new(1_112_445),
    ]);

    /// <summary>Pet ring item IDs (cat and dog)</summary>
    public static readonly FrozenSet<ItemTemplateId> PetRings = FrozenSet.ToFrozenSet<ItemTemplateId>([
        new(1_822_000),
        new(1_832_000),
    ]);

    /// <summary>Evan dragon riding item IDs</summary>
    public static readonly FrozenSet<ItemTemplateId> EvanDragonRidingItems = FrozenSet.ToFrozenSet<ItemTemplateId>([
        new(1_902_040),
        new(1_902_041),
        new(1_902_042),
    ]);

    /// <summary>Wedding invitation bundle items</summary>
    public static readonly FrozenSet<ItemTemplateId> InvitationBundles = FrozenSet.ToFrozenSet<ItemTemplateId>([
        new(4_031_377),
        new(4_031_395),
    ]);

    /// <summary>Wedding invitation guest items</summary>
    public static readonly FrozenSet<ItemTemplateId> InvitationGuests = FrozenSet.ToFrozenSet<ItemTemplateId>([
        new(4_031_406),
        new(4_031_407),
    ]);

    /// <summary>Wedding receipt items</summary>
    public static readonly FrozenSet<ItemTemplateId> WeddingReceiptItems = FrozenSet.ToFrozenSet<ItemTemplateId>([
        new(4_031_481),
        new(4_031_376),
        new(4_031_480),
        new(4_031_375),
    ]);

    /// <summary>Wedding items that cannot be dropped</summary>
    public static readonly FrozenSet<ItemTemplateId> WeddingCantDropItems = FrozenSet.ToFrozenSet<ItemTemplateId>([
        new(4_031_373),
        new(4_031_374),
    ]);

    /// <summary>Mu Lung Dojo ticket IDs blocked for Evan (5_050_001–5_050_004).</summary>
    public static readonly FrozenSet<ItemTemplateId> DojoTicketsEvanBlocked = FrozenSet.ToFrozenSet<ItemTemplateId>([
        new(5_050_001),
        new(5_050_002),
        new(5_050_003),
        new(5_050_004),
    ]);

    /// <summary>Mu Lung Dojo ticket IDs blocked for non-Evan (5_050_005–5_050_009).</summary>
    public static readonly FrozenSet<ItemTemplateId> DojoTicketsNonEvanBlocked = FrozenSet.ToFrozenSet<ItemTemplateId>([
        new(5_050_005),
        new(5_050_006),
        new(5_050_007),
        new(5_050_008),
        new(5_050_009),
    ]);
}
