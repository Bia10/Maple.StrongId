# Maple.StrongId — V95 Source References

This file records the reverse-engineering sources behind each type and predicate in this library.
All citations are from the V95 PDB decompilation (`game_pseudocode.c`, `game_types.h`).

---

## AccountId

| Subject                                 | Source                                                                                 |
| --------------------------------------- | -------------------------------------------------------------------------------------- |
| `m_dwAccountId` field, `SetAccountInfo` | `game_types.h — CWvsContext`; `game_pseudocode.c:403544 — CWvsContext::SetAccountInfo` |

---

## CharacterId

| Subject                       | Source                                                                                 |
| ----------------------------- | -------------------------------------------------------------------------------------- |
| `GetCharacterId()` exposure   | `game_types.h — CWvsContext`; `game_pseudocode.c:116208 — CWvsContext::GetCharacterId` |
| `IsSummon`, `SummonSlotIndex` | `game_pseudocode.c:505903 — summon dispatch; 0x3FFFFFFF mask`                          |

---

## EmployeeTemplateId

| Subject                                            | Source                     |
| -------------------------------------------------- | -------------------------- |
| `CEmployeeTemplate::Load` catalog                  | `game_pseudocode.c:245700` |
| `RegisterEmployee`                                 | `game_pseudocode.c:245463` |
| `CActionMan::GetEmployeeImgEntry` (pool key 0xDA0) | `game_pseudocode.c:32817`  |

---

## FieldTemplateId

| Subject                                                | Source                                        |
| ------------------------------------------------------ | --------------------------------------------- |
| `Continent`, `IsUpgradeTombUsable`, related predicates | `game_pseudocode.c:164108`                    |
| `IsTown`, `IsHenesys`, town constants                  | `game_pseudocode.c:164149 — is_town_field_id` |
| Free Market channel count (23)                         | `game_pseudocode.c:1062718`                   |
| `IsPetAutoSpeechSuppressed`                            | `game_pseudocode.c:1062718`                   |
| `HasForcedReturn` (value != 999999999)                 | `CField::LoadMap`                             |
| WZ group name construction                             | `MapWzContext.FindMapNode`                    |

---

## ItemTemplateId

| Subject                                                               | Source                                                                                                        |
| --------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------- |
| `Category` (/ 1_000_000)                                              | `game_pseudocode.c:58003`                                                                                     |
| `TypeIndex` (/ 10_000), equip slot predicates                         | `game_pseudocode.c:105597 — is_correct_bodypart`                                                              |
| `GenderDigit`                                                         | `game_pseudocode.c:105569 — get_gender_from_id`                                                               |
| `IsLongCoat` (typeindex 105)                                          | `game_pseudocode.c:105754`                                                                                    |
| `IsShield` (typeindex 109)                                            | `game_pseudocode.c:0x47CE50`                                                                                  |
| `IsSaddle` (typeindex 191)                                            | `game_pseudocode.c:0x47CE70`                                                                                  |
| `IsPetEquip`                                                          | `game_pseudocode.c — is_pet_equip_item`                                                                       |
| `IsTamingMobItem`                                                     | `game_pseudocode.c — is_tamedmob / CAvatar::IsRidingTamedMob`                                                 |
| `IsVehicle`                                                           | `game_pseudocode.c — is_vehicle`                                                                              |
| `IsEvanDragonRidingItem`                                              | `game_pseudocode.c — is_evan_dragon_riding_item`                                                              |
| `IsEventVehicleType1` (IDs 1_932_001–1_932_002)                       | `game_pseudocode.c:11273 — is_event_vehicle_type1`                                                            |
| `s_eventVehicleType2Ids` (28 IDs)                                     | `game_pseudocode.c:11278 — is_event_vehicle_type2`; `game_pseudocode.c:638806 — get_skill_id_from_vehicle_id` |
| `IsCoupleEquip`                                                       | `game_pseudocode.c:0x47CFA0`                                                                                  |
| `IsFriendshipEquip`                                                   | `game_pseudocode.c — is_friendship_equip_item`                                                                |
| `IsWeapon`, `IsOneHandedWeapon`, `IsTwoHandedWeapon`, `WeaponSubType` | `game_pseudocode.c:105535 — get_weapon_type`                                                                  |
| `IsWeaponSticker`                                                     | `game_pseudocode.c:0x406590 — is_weapon_sticker`                                                              |
| `IsSkillEffectWeapon`                                                 | `game_pseudocode.c — is_skill_effect_weapon`                                                                  |
| `IsPetFood`                                                           | `game_pseudocode.c — is_pet_food_item`                                                                        |
| `IsPetAbility`                                                        | `game_pseudocode.c — is_pet_ability_item`                                                                     |
| `IsPetRing`                                                           | `game_pseudocode.c — is_petring_item`                                                                         |
| `IsCashPetFood`                                                       | `game_pseudocode.c:0x47CF40 — is_cash_pet_food_item`                                                          |
| `IsRechargeable` (javelin/pellet)                                     | `game_pseudocode.c — is_javelin_item, is_pellet_item`                                                         |
| `IsStateChange`                                                       | `game_pseudocode.c:0x4FF170 — is_state_change_item`                                                           |
| `IsPortalScroll`                                                      | `game_pseudocode.c:0x4FF1D0`                                                                                  |
| `IsMasteryBook`                                                       | `game_pseudocode.c:0x47CEE0 — is_masterybook_item`                                                            |
| `IsDualMasteryBook`                                                   | `game_pseudocode.c:0x47CEA0 — is_dual_masterybook_item`                                                       |
| `IsSkillLearn`                                                        | `game_pseudocode.c:0x4FF440 — is_skill_learn_item`                                                            |
| `IsSkillReset`                                                        | `game_pseudocode.c:0x4FF470`                                                                                  |
| `IsMobSummon`                                                         | `game_pseudocode.c:0x4FF2F0`                                                                                  |
| `IsImmediateMobSummon`                                                | `game_pseudocode.c:0x4FFB70 — is_immediate_mobsummon_item`                                                    |
| `IsBridle`                                                            | `game_pseudocode.c:0x4FF3E0`                                                                                  |
| `IsTamingMobFood`                                                     | `game_pseudocode.c:0x4FF3B0`                                                                                  |
| `IsMorph`                                                             | `game_pseudocode.c:0x4FF590`                                                                                  |
| `IsRandomMorphOther`                                                  | `game_pseudocode.c:0x4FF6B0 — is_random_morph_item_other`                                                     |
| `IsShopScanner`                                                       | `game_pseudocode.c:0x4FF5C0`                                                                                  |
| `IsMapTransfer`                                                       | `game_pseudocode.c:0x4FF5F0`                                                                                  |
| `IsEngagementRingBox`                                                 | `game_pseudocode.c:0x4FF620`                                                                                  |
| `IsNewYearCardConsume`                                                | `game_pseudocode.c:0x4FF650`                                                                                  |
| `IsNewYearCardEtc`                                                    | `game_pseudocode.c:0x4FF680`                                                                                  |
| `IsExpUp`                                                             | `game_pseudocode.c:0x4FFCB0`                                                                                  |
| `IsGachaponBox`                                                       | `game_pseudocode.c:0x4FFC80`                                                                                  |
| `IsBook`                                                              | `game_pseudocode.c:0x4FF560`                                                                                  |
| `IsPigmyEgg`                                                          | `game_pseudocode.c:0x4FFB40`                                                                                  |
| `IsUiOpen`                                                            | `game_pseudocode.c:0x4FFC30`                                                                                  |
| `IsSelectNpc`                                                         | `game_pseudocode.c:0x4FF4A0`                                                                                  |
| `IsMinigame`                                                          | `game_pseudocode.c:0x4FF4D0`                                                                                  |
| `IsNonCashEffect`                                                     | `game_pseudocode.c:0x4FF500`                                                                                  |
| `IsScriptRun`                                                         | `game_pseudocode.c:0x4FF410`                                                                                  |
| `IsAntiMacro`                                                         | `game_pseudocode.c:0x4FF380`                                                                                  |
| `IsRaise`                                                             | `game_pseudocode.c:0x4FF810 — is_raise_item`                                                                  |
| `IsMesoProtect`                                                       | `game_pseudocode.c:0x4FF860 — is_meso_protect_item`                                                           |
| `IsPortableChair`                                                     | `game_pseudocode.c:0x4FF530`                                                                                  |
| `IsNewUpgrade`                                                        | `game_pseudocode.c:0x4FF910 — is_new_upgrade_item`                                                            |
| `IsDurabilityUpgrade`                                                 | `game_pseudocode.c:0x4FF940 — is_durability_upgrade_item`                                                     |
| `IsHyperUpgrade`                                                      | `game_pseudocode.c:0x4FF200 — is_hyper_upgrade_item`                                                          |
| `IsItemOptionUpgrade`                                                 | `game_pseudocode.c:0x4FF230 — is_itemoption_upgrade_item`                                                     |
| `IsAccUpgrade`                                                        | `game_pseudocode.c:0x4FF290 — is_acc_upgrade_item`                                                            |
| `IsBlackUpgrade`                                                      | `game_pseudocode.c:0x4FF2C0 — is_black_upgrade_item`                                                          |
| `IsRelease`                                                           | `game_pseudocode.c:0x4FF260 — is_release_item`                                                                |
| `IsWhiteScrollNoConsume`                                              | `game_pseudocode.c:0x4FFCE0`                                                                                  |
| `IsCashPet`                                                           | `game_pseudocode.c:58003 — get_item_slottype_from_id`                                                         |
| `IsCashMorph`                                                         | `game_pseudocode.c:0x4FF350`                                                                                  |
| `IsCashPackage`                                                       | `game_pseudocode.c:0x47CF70`                                                                                  |
| `IsCharSlotInc`                                                       | `game_pseudocode.c:0x47CFD0 — is_charslot_inc_item`                                                           |
| `IsEquipSlotExt`                                                      | `game_pseudocode.c:0x47D020 — is_equipslot_ext_item`                                                          |
| `IsSlotInc`                                                           | `game_pseudocode.c — is_slot_inc_item`                                                                        |
| `IsTrunkCountInc`                                                     | `game_pseudocode.c:0x47D0A0 — is_trunkcount_inc_item`                                                         |
| `IsNamingItem`                                                        | `game_pseudocode.c — is_correct_naming_equip`                                                                 |
| `IsExtendExpireDate`                                                  | `game_pseudocode.c:0x4FFBD0`                                                                                  |
| `IsChangeMaplePoint`                                                  | `game_pseudocode.c — is_changemaplepoint`                                                                     |
| `IsOnlyForPrepaid`                                                    | `game_pseudocode.c — is_only_for_prepaid`                                                                     |
| `CashSlotItemType` switch                                             | `game_pseudocode.c:126557 — get_cashslot_item_type (addr 0x488C70)`                                           |
| Wedding/invitation predicates                                         | `game_pseudocode.c:0x4FF780–0x4FF7F0`                                                                         |
| `IsEngagementRing`                                                    | `game_pseudocode.c:0x4FF700 — is_engagement_ring_item`                                                        |
| `IsWeddingRing`                                                       | `game_pseudocode.c:0x4FF750 — is_wedding_ring_item`                                                           |
| `IsAdventureRing`                                                     | `game_pseudocode.c:0x4FFC00 — is_adventure_ring_item`                                                         |
| `IsSpecial`                                                           | `game_pseudocode.c:0x47D0D0 — is_special_item`                                                                |
| `IsTreatSingly`                                                       | `game_pseudocode.c:45268 — is_treat_singly`                                                                   |
| `Gender` enum                                                         | `game_pseudocode.c:105569 — get_gender_from_id`                                                               |
| `InventoryTab` / `WzFolder`                                           | WZ archive convention; `game_pseudocode.c item-load paths`                                                    |
| `InventoryTab.Get`                                                    | `game_pseudocode.c:224091 — get_tab_from_item_typeindex`                                                      |
| `CanUpgradeEquip`                                                     | `game_pseudocode.c:224358 — is_correct_upgrade_equip`                                                         |
| `WhiteScroll` constant (2_340_000)                                    | `CharacterData::HasWhiteScroll()` — hardcoded ID check                                                        |

---

## JobId

| Subject                                                       | Source                                                                                                   |
| ------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------- |
| `Lineage` (thousands digit)                                   | `game_pseudocode.c:116218`                                                                               |
| `Category` (hundreds digit)                                   | `game_pseudocode.c:116213`                                                                               |
| `FieldCategoryCode`                                           | `game_pseudocode.c:307558 — Field::JobCategoryCond::IsTrue`                                              |
| `IsCygnus`, `IsAran`, `IsEvan`, `IsResistance`, `IsDualBlade` | `game_pseudocode.c:116218–116238`                                                                        |
| `IsBattleMage`, `IsWildHunter`, `IsMechanic`                  | `game_pseudocode.c:164194–164204`                                                                        |
| `IsExtendSP`                                                  | `game_pseudocode.c:212696`                                                                               |
| `IsAdmin`, `IsManager`                                        | `game_pseudocode.c:223989–223994`                                                                        |
| `GetTier`                                                     | `game_pseudocode.c:116260`                                                                               |
| `IsThirdJobOrAbove`                                           | `game_pseudocode.c:165888`                                                                               |
| `CanUseSkillRoot`                                             | `game_pseudocode.c:638008 — is_correct_job_for_skill_root`                                               |
| `GetJobChangeLevelRequirement`                                | `game_pseudocode.c:914556 — get_job_change_level`; `game_pseudocode.c:914586 — get_dualjob_change_level` |
| `IsMatchedItemIdJob`                                          | `game_pseudocode.c:116340 — is_matched_itemid_job`                                                       |
| `GetJobIcon`                                                  | `game_pseudocode.c:786067 — GetJobIcon`                                                                  |
| `IsKnownJob`, valid job set                                   | `game_pseudocode.c:164857 — get_job_name`                                                                |
| Base HP/MP gain formulas                                      | `game_pseudocode.c:940829 — IncHPVal`; `game_pseudocode.c:940861 — IncMPVal`; `game_pseudocode.c:940922` |
| Job bitmask constants (0=all, -1=beginner)                    | `game_pseudocode.c:116340`                                                                               |
| `IsDualJobBorn`                                               | `game_pseudocode.c — is_dual_job_born`                                                                   |

---

## MobTemplateId

| Subject            | Source                             |
| ------------------ | ---------------------------------- |
| `IsNotCapturable`  | `game_pseudocode.c:164126`         |
| `IsNotSwallowable` | `game_pseudocode.c:164136`         |
| Level visibility   | `game_pseudocode.c:511173`         |
| Boss determination | `game_pseudocode.c:687742, 689824` |
| Pet suitability    | `game_pseudocode.c:117082`         |

---

## MorphTemplateId

| Subject                                                          | Source                                              |
| ---------------------------------------------------------------- | --------------------------------------------------- |
| `CMorphTemplate` struct fields; catalog `ms_mMorphTemplate`     | `game_types.h:41225`                                |
| Morph predicates; `CMorphTemplate::IsSuperMan` (IDs 1000/1001/1100/1101) | `game_pseudocode.c:93071–93074`             |
| `CAvatar::SetMorphed` — `tDelay = 0` for ID 1002, 500 ms for all others | `game_pseudocode.c:93071–93074`             |

---

## NpcTemplateId

| Subject                                                | Source                                                          |
| ------------------------------------------------------ | --------------------------------------------------------------- |
| `EvanDragon` constant (`NPC_ID_EVAN_DRAGON = 0xF7508`) | `game_types.h — NPC_ID_EVAN_DRAGON`; `game_pseudocode.c:592943` |
| `NoblesseTutor` (`NPC_ID_NOBLESSE_TUTOR = 0x10CCD0`)   | `game_types.h — NPC_ID_NOBLESSE_TUTOR`                          |
| `AranTutor` (`NPC_ID_ARAN_TUTOR = 0x125750`)           | `game_types.h — NPC_ID_ARAN_TUTOR`                              |
| `Kin` (`NPC_ID_KIN = 0x970FE0`)                        | `game_types.h — NPC_ID_KIN`                                     |
| `Nimakin` (`NPC_ID_NIMAKIN = 0x970FE1`)                | `game_types.h — NPC_ID_NIMAKIN`                                 |

---

## PetTemplateId

| Subject                                                                | Source                                          |
| ---------------------------------------------------------------------- | ----------------------------------------------- |
| Pet equip slot arithmetic                                              | `game_pseudocode.c:117082 — IsItemSuitedForPet` |
| `CPetTemplate::ms_mPetTemplate` catalog (separate from `CMobTemplate`) | `game_types.h — CPetTemplate`                   |

---

## QuestTemplateId

| Subject                                 | Source                                                                   |
| --------------------------------------- | ------------------------------------------------------------------------ |
| `IsPartyQuest`                          | `game_pseudocode.c:233138 — CQuestMan::IsPartyQuest`                     |
| `IsPartyQuestId`                        | `game_pseudocode.c:562988 — CPartyQuestInfoManager::IsPartyQuestID`      |
| `IsExpeditionQuestId`                   | `game_pseudocode.c:562993 — CPartyQuestInfoManager::IsExpeditionQuestID` |
| Packet `usQuestID` field type (0–65535) | V95 client protocol packet decode                                        |
| `IsDeliveryAcceptQuest`                 | `game_pseudocode.c:592943 — CQuestMan::IsDeliveryAcceptQuest`            |

---

## ReactorTemplateId

| Subject                                                                                          | Source                                                                             |
| ------------------------------------------------------------------------------------------------ | ---------------------------------------------------------------------------------- |
| `GetReactorTemplate` lookup; catalog `ZMap<unsigned long, ZRef<CReactorTemplate>, unsigned long>` | `game_types.h — CReactorTemplate`; `game_pseudocode.c:615960 — GetReactorTemplate` |
| `CReactorTemplate` struct fields: `m_nStateCount`, `m_bMove`, `m_nLayer`, `m_bNotHitable`, `m_bActivateByTouch`, `m_nQuestID` | `game_types.h — CReactorTemplate`            |
| `CReactorTemplate::Load()` is an empty stub — templates not bulk-preloaded, loaded lazily on first use | `game_pseudocode.c:615960`                                                   |

---

## SetItemTemplateId

| Subject                                                | Source                                  |
| ------------------------------------------------------ | --------------------------------------- |
| `SETITEMINFO`, `EQUIPPED_SETITEM` struct               | `game_types.h — CItemInfo::SETITEMINFO` |
| Set-bonus stat accumulation loop                       | `game_pseudocode.c:691070`              |
| Null-ID guard (`if (nSetItemID)`)                      | `game_pseudocode.c:697581`              |
| Tooltip height formula (`16 * nSetCompleteCount + 40`) | `game_pseudocode.c:988697`              |

---

## SkillTemplateId

| Subject                                        | Source                                                                                                 |
| ---------------------------------------------- | ------------------------------------------------------------------------------------------------------ |
| `Root` (`/ 10_000`), `WzImageName`             | `game_pseudocode.c:116255`                                                                             |
| `WzSkillNodePath` (`{Root}.img/skill/{Value}`) | `game_pseudocode.c:662611 — CSkillInfo::LoadSkill`                                                     |
| `IsGuildSkill` (root 9100)                     | `game_pseudocode.c:212701`                                                                             |
| `IsSuperGmSkill` (root 910)                    | `game_types.h:2784`                                                                                    |
| `IsUnregistered`                               | `game_pseudocode.c:1034901`                                                                            |
| `IsPassive`, `IsActive`, `IsCommon`            | `game_pseudocode.c:232532 — is_active_skill` (verbatim: `v1 = nSkillID/1000%10; return v1 && v1 != 9`) |
| `IsBaseClassSkill`                             | `game_pseudocode.c:116316 — master level check`                                                        |
| `s_ignoreMasterLevelCommon` (15 IDs)           | `game_pseudocode.c:116278 — is_ignore_master_level_for_common`                                         |
| `NeedsMasterLevel`                             | `game_pseudocode.c:116278, 116316 — is_skill_need_master_level`                                        |
| `s_keyDownSkills` (19 IDs)                     | `game_pseudocode.c:232540 — is_keydown_skill`                                                          |
| `s_teleportSkills` (8 IDs)                     | `game_pseudocode.c:637869 — is_teleport_skill`                                                         |
| `9_001_002` Admin teleport constant            | `game_types.h:2786`                                                                                    |
| `s_teleportMasterySkills` (4 IDs)              | `game_pseudocode.c:637881 — is_teleport_mastery_skill`                                                 |
| `s_vehicleSkills` (8 IDs)                      | `game_pseudocode.c:1101997 — is_vehicle_skill`                                                         |
| `s_heroWillSkills` (18 IDs)                    | `game_pseudocode.c:638074 — is_heros_will_skill`                                                       |
| Hero's Will — Dual Blade (4_341_008)           | Hex: `unk_423D10 = 0x423D10 = 4,341,008`                                                               |
| Hero's Will — Buccaneer (5_121_008)            | Hex: `loc_4E23EC+4 = 0x4E23F0 = 5,121,008`                                                             |
| `IsAranCombo`                                  | `game_types.h — ARAN_COMBO_ABILITY, ARAN_DOUBLE_SWING, ARAN_COMBAT_STEP`                               |
| `s_battleMageAuraSkills` (7 IDs)               | `game_pseudocode.c:1034906 — is_bmage_aura_skill`                                                      |
| `s_nonSlotSkills` (12 IDs)                     | `game_pseudocode.c:914527 — is_nonslot_skill`                                                          |
| `s_prepareBombSkills` (3 IDs)                  | `game_pseudocode.c:638915 — is_prepare_bomb_skill`                                                     |
| `s_eventVehicleSerials` (28 serials)           | `game_pseudocode.c:638765 — is_event_vehicle_skill`                                                    |
| `IsExtendSPOwner`                              | `game_pseudocode.c:212696 — is_extendsp_job`                                                           |
| `OwnerJobDegree`                               | `game_pseudocode.c:1034925 — get_skill_degree_from_skill_root`                                         |
| `CanBeUsedByJob`                               | `game_pseudocode.c:638008 — is_correct_job_for_skill_root`                                             |
| Admin skill constants                          | `game_types.h:2784–2794`                                                                               |
| Guild skill constants                          | `game_pseudocode.c:212701`                                                                             |
| Guild `BUSINESSEFFICENYUP` (typo confirmed)    | Original PDB symbol has missing 'I' in "EFFICIENCY"                                                    |

---

## TamingMobTemplateId

| Subject                                                                                             | Source                                                                                                     |
| --------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------- |
| Loaded from `Mob.wz` into `ZMap<unsigned long, ZRef<CTamingMobTemplate>, unsigned long>` catalog    | V95 client — separate from MobTemplate catalog                                                             |
| `CTamingMobTemplate` struct fields include `nSwim` and `nFatigue`                                   | `game_types.h — CTamingMobTemplate`                                                                        |
| `IsRedDraco` (1_902_002)                                                                            | `game_types.h:8189 — TAMINGMOB_RED_DRACO = 0x1D05B2`; `game_pseudocode.c:1003589`                          |
| Seat offsets — Red Draco: x=70, y=-25, z=-25                                                        | `game_pseudocode.c:1003589 — CUIUserInfo riding-avatar switch`                                             |
| `IsRyuho` (1_902_015–1_902_018)                                                                     | `game_types.h:8190–8193 — TAMINGMOB_RYUHO_50/100/150/200 = 0x1D05BF–0x1D05C2`; `game_pseudocode.c:1003594` |
| Seat offsets — Ryuho-50: x=80,y=0,z=-12; Ryuho-100/150: x=80,y=-5,z=-15; Ryuho-200: x=75,y=-5,z=-20 | `game_pseudocode.c:1003594`                                                                                |
| `IsEvanDragon` / IsMir (1_902_040–1_902_042)                                                        | `game_types.h:8194–8196 — TAMINGMOB_MIR_1/2/3 = 0x1D05D8–0x1D05DA`; `game_pseudocode.c:11255, 11265`       |
| MIR_1 seat offset: reads from equip data; MIR_2/3: x=70, y=-25, z=-35                               | `game_pseudocode.c:11255, 11265`                                                                           |
| Static constants `RedDraco`, `Ryuho50–200`, `Mir1–3`                                                | PDB enum values above                                                                                      |
