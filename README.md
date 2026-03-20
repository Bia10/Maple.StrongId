# Maple.StrongId

![.NET](https://img.shields.io/badge/net10.0-5C2D91?logo=.NET&labelColor=gray)
![C#](https://img.shields.io/badge/C%23-14.0-239120?labelColor=gray)
[![Build Status](https://github.com/Bia10/Maple.StrongId/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/Bia10/Maple.StrongId/actions/workflows/dotnet.yml)
[![codecov](https://codecov.io/gh/Bia10/Maple.StrongId/branch/main/graph/badge.svg)](https://codecov.io/gh/Bia10/Maple.StrongId)
[![Nuget](https://img.shields.io/nuget/v/Maple.StrongId?color=purple)](https://www.nuget.org/packages/Maple.StrongId/)
[![License](https://img.shields.io/github/license/Bia10/Maple.StrongId)](https://github.com/Bia10/Maple.StrongId/blob/main/LICENSE)

Strongly-typed zero-overhead `int` wrapper types for MapleStory V95 domain objects, with a Roslyn source generator for boilerplate elimination. Cross-platform, trimmable and AOT/NativeAOT compatible.

⭐ Please star this project if you like it. ⭐

[Example](#example) | [Types](#types) | [Example Catalogue](#example-catalogue) | [Public API Reference](#public-api-reference)

## Example

```csharp
var item = new ItemTemplateId(1002140);
bool isHat = item.IsHat;
bool isWeapon = item.IsWeapon;
_ = isHat;
_ = isWeapon;
```

For more examples see [Example Catalogue](#example-catalogue).

## Types

| Type                  | Domain                                |
| --------------------- | ------------------------------------- |
| `AccountId`           | Player accounts                       |
| `CharacterId`         | Characters                            |
| `ItemTemplateId`      | Items (equip, consume, install, etc.) |
| `FieldTemplateId`     | Maps and fields                       |
| `JobId`               | Jobs and job branches                 |
| `MobTemplateId`       | Monsters                              |
| `NpcTemplateId`       | NPCs                                  |
| `PetTemplateId`       | Pets                                  |
| `QuestTemplateId`     | Quests                                |
| `ReactorTemplateId`   | Reactors                              |
| `SetItemTemplateId`   | Set item groups                       |
| `SkillTemplateId`     | Skills                                |
| `TamingMobTemplateId` | Taming/mount mobs                     |
| `EmployeeTemplateId`  | Crafting NPCs                         |
| `MorphTemplateId`     | Morph states                          |

## Benchmarks

Benchmarks.

### Detailed Benchmarks

#### Comparison Benchmarks

##### TestBench Benchmark Results


## Example Catalogue

The following examples are available in [ReadMeTest.cs](src/Maple.StrongId.XyzTest/ReadMeTest.cs).

### Example - ItemTemplateId

```csharp
var item = new ItemTemplateId(1002140);
bool isHat = item.IsHat; // true
bool isWeapon = item.IsWeapon; // false
_ = isHat;
_ = isWeapon;
```

### Example - JobId

```csharp
var skill = new SkillTemplateId(1000000);
bool isActive = skill.IsActive;
int root = skill.Root; // 100 (Warrior category root)
_ = isActive;
_ = root;
```

### Example - FieldTemplateId

```csharp
var field = new FieldTemplateId(100000000);
int continent = field.Continent; // 1 (Victoria Island)
bool isTown = field.IsTownId;
_ = continent;
_ = isTown;
```

## Public API Reference

```csharp
[assembly: System.Reflection.AssemblyMetadata("IsAotCompatible", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/Bia10/Maple.StrongId/")]
[assembly: System.Resources.NeutralResourcesLanguage("en")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Maple.StrongId.Benchmarks")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Maple.StrongId.ComparisonBenchmarks")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Maple.StrongId.Test")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Maple.StrongId.XyzTest")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v10.0", FrameworkDisplayName=".NET 10.0")]
namespace Maple.StrongId
{
    [Maple.StrongId.StrongIntId(GenerateJsonConverter=false)]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    public readonly struct AccountId : System.IComparable<Maple.StrongId.AccountId>, System.IEquatable<Maple.StrongId.AccountId>, System.IFormattable, System.IParsable<Maple.StrongId.AccountId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.AccountId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.AccountId, Maple.StrongId.AccountId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.AccountId, Maple.StrongId.AccountId, bool>
    {
        public AccountId(int Value) { }
        public int Value { get; init; }
        public int CompareTo(Maple.StrongId.AccountId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public static Maple.StrongId.AccountId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.AccountId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.AccountId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.AccountId result) { }
        public static Maple.StrongId.AccountId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.AccountId id) { }
        public static bool operator <(Maple.StrongId.AccountId left, Maple.StrongId.AccountId right) { }
        public static bool operator <=(Maple.StrongId.AccountId left, Maple.StrongId.AccountId right) { }
        public static bool operator >(Maple.StrongId.AccountId left, Maple.StrongId.AccountId right) { }
        public static bool operator >=(Maple.StrongId.AccountId left, Maple.StrongId.AccountId right) { }
    }
    [Maple.StrongId.StrongIntId(GenerateJsonConverter=false)]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    public readonly struct CharacterId : System.IComparable<Maple.StrongId.CharacterId>, System.IEquatable<Maple.StrongId.CharacterId>, System.IFormattable, System.IParsable<Maple.StrongId.CharacterId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.CharacterId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.CharacterId, Maple.StrongId.CharacterId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.CharacterId, Maple.StrongId.CharacterId, bool>
    {
        public const int MaxNaturalValue = 1073741823;
        public CharacterId(int Value) { }
        public int SummonIndex { get; }
        public int Value { get; init; }
        public int CompareTo(Maple.StrongId.CharacterId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public static Maple.StrongId.CharacterId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.CharacterId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.CharacterId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.CharacterId result) { }
        public static Maple.StrongId.CharacterId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.CharacterId id) { }
        public static bool operator <(Maple.StrongId.CharacterId left, Maple.StrongId.CharacterId right) { }
        public static bool operator <=(Maple.StrongId.CharacterId left, Maple.StrongId.CharacterId right) { }
        public static bool operator >(Maple.StrongId.CharacterId left, Maple.StrongId.CharacterId right) { }
        public static bool operator >=(Maple.StrongId.CharacterId left, Maple.StrongId.CharacterId right) { }
    }
    [Maple.StrongId.StrongIntId]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    [System.Text.Json.Serialization.JsonConverter(typeof(Maple.StrongId.EmployeeTemplateId.StrongIdJsonConverter))]
    public readonly struct EmployeeTemplateId : System.IComparable<Maple.StrongId.EmployeeTemplateId>, System.IEquatable<Maple.StrongId.EmployeeTemplateId>, System.IFormattable, System.IParsable<Maple.StrongId.EmployeeTemplateId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.EmployeeTemplateId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.EmployeeTemplateId, Maple.StrongId.EmployeeTemplateId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.EmployeeTemplateId, Maple.StrongId.EmployeeTemplateId, bool>
    {
        public EmployeeTemplateId(int Value) { }
        public int Value { get; init; }
        [System.Obsolete("Allocates. Use TryWriteWzImageName(Span<char>, out int) on hot paths.")]
        public string WzImageName { get; }
        public int CompareTo(Maple.StrongId.EmployeeTemplateId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryWriteWzImageName(System.Span<char> destination, out int charsWritten) { }
        public static Maple.StrongId.EmployeeTemplateId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.EmployeeTemplateId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.EmployeeTemplateId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.EmployeeTemplateId result) { }
        public static Maple.StrongId.EmployeeTemplateId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.EmployeeTemplateId id) { }
        public static bool operator <(Maple.StrongId.EmployeeTemplateId left, Maple.StrongId.EmployeeTemplateId right) { }
        public static bool operator <=(Maple.StrongId.EmployeeTemplateId left, Maple.StrongId.EmployeeTemplateId right) { }
        public static bool operator >(Maple.StrongId.EmployeeTemplateId left, Maple.StrongId.EmployeeTemplateId right) { }
        public static bool operator >=(Maple.StrongId.EmployeeTemplateId left, Maple.StrongId.EmployeeTemplateId right) { }
        public sealed class StrongIdJsonConverter : System.Text.Json.Serialization.JsonConverter<Maple.StrongId.EmployeeTemplateId>
        {
            public StrongIdJsonConverter() { }
            public override Maple.StrongId.EmployeeTemplateId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) { }
            public override void Write(System.Text.Json.Utf8JsonWriter writer, Maple.StrongId.EmployeeTemplateId value, System.Text.Json.JsonSerializerOptions options) { }
        }
    }
    [Maple.StrongId.StrongIntId]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    [System.Text.Json.Serialization.JsonConverter(typeof(Maple.StrongId.FieldTemplateId.StrongIdJsonConverter))]
    public readonly struct FieldTemplateId : System.IComparable<Maple.StrongId.FieldTemplateId>, System.IEquatable<Maple.StrongId.FieldTemplateId>, System.IFormattable, System.IParsable<Maple.StrongId.FieldTemplateId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.FieldTemplateId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.FieldTemplateId, Maple.StrongId.FieldTemplateId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.FieldTemplateId, Maple.StrongId.FieldTemplateId, bool>
    {
        public const int ForcedReturnSentinel = 999999999;
        public const int FreeMarketBase = 910000000;
        public const int FreeMarketChannelCount = 23;
        public static readonly Maple.StrongId.FieldTemplateId Ariant;
        public static readonly Maple.StrongId.FieldTemplateId ElNath;
        public static readonly Maple.StrongId.FieldTemplateId Ellinia;
        public static readonly Maple.StrongId.FieldTemplateId Henesys;
        public static readonly Maple.StrongId.FieldTemplateId KerningCity;
        public static readonly Maple.StrongId.FieldTemplateId Leafre;
        public static readonly Maple.StrongId.FieldTemplateId LithHarbor;
        public static readonly Maple.StrongId.FieldTemplateId Ludibrium;
        public static readonly Maple.StrongId.FieldTemplateId Magatia;
        public static readonly Maple.StrongId.FieldTemplateId MuLung;
        public static readonly Maple.StrongId.FieldTemplateId NautilusHarbor;
        public static readonly Maple.StrongId.FieldTemplateId NewLeafCity;
        public static readonly Maple.StrongId.FieldTemplateId Orbis;
        public static readonly Maple.StrongId.FieldTemplateId Perion;
        public static readonly Maple.StrongId.FieldTemplateId SleepywoodInn;
        public FieldTemplateId(int Value) { }
        public int Continent { get; }
        public bool IsEdelstein { get; }
        public bool IsEdelsteinSubArea { get; }
        public bool IsEllinforest { get; }
        public bool IsFreeMarket { get; }
        public bool IsMushroomKingdom { get; }
        public bool IsNlcMasteria { get; }
        public bool IsOssyria { get; }
        public bool IsSingapore { get; }
        public bool IsSpecialInstanced { get; }
        public bool IsTownId { get; }
        public bool IsUpgradeTombBlocked { get; }
        public bool IsVictoriaIsland { get; }
        public bool IsZipangu { get; }
        public int Region { get; }
        public int Value { get; init; }
        public string WzGroupName { get; }
        public int CompareTo(Maple.StrongId.FieldTemplateId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public static Maple.StrongId.FieldTemplateId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.FieldTemplateId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.FieldTemplateId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.FieldTemplateId result) { }
        public static Maple.StrongId.FieldTemplateId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.FieldTemplateId id) { }
        public static bool operator <(Maple.StrongId.FieldTemplateId left, Maple.StrongId.FieldTemplateId right) { }
        public static bool operator <=(Maple.StrongId.FieldTemplateId left, Maple.StrongId.FieldTemplateId right) { }
        public static bool operator >(Maple.StrongId.FieldTemplateId left, Maple.StrongId.FieldTemplateId right) { }
        public static bool operator >=(Maple.StrongId.FieldTemplateId left, Maple.StrongId.FieldTemplateId right) { }
        public sealed class StrongIdJsonConverter : System.Text.Json.Serialization.JsonConverter<Maple.StrongId.FieldTemplateId>
        {
            public StrongIdJsonConverter() { }
            public override Maple.StrongId.FieldTemplateId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) { }
            public override void Write(System.Text.Json.Utf8JsonWriter writer, Maple.StrongId.FieldTemplateId value, System.Text.Json.JsonSerializerOptions options) { }
        }
    }
    [Maple.StrongId.StrongIntId]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    [System.Text.Json.Serialization.JsonConverter(typeof(Maple.StrongId.ItemTemplateId.StrongIdJsonConverter))]
    public readonly struct ItemTemplateId : System.IComparable<Maple.StrongId.ItemTemplateId>, System.IEquatable<Maple.StrongId.ItemTemplateId>, System.IFormattable, System.IParsable<Maple.StrongId.ItemTemplateId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.ItemTemplateId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.ItemTemplateId, Maple.StrongId.ItemTemplateId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.ItemTemplateId, Maple.StrongId.ItemTemplateId, bool>
    {
        public static readonly System.Collections.Frozen.FrozenSet<Maple.StrongId.ItemTemplateId> AdventureRings;
        public static readonly Maple.StrongId.ItemTemplateId CharacterSaleA;
        public static readonly Maple.StrongId.ItemTemplateId CharacterSaleB;
        public static readonly System.Collections.Frozen.FrozenSet<Maple.StrongId.ItemTemplateId> DojoTicketsEvanBlocked;
        public static readonly System.Collections.Frozen.FrozenSet<Maple.StrongId.ItemTemplateId> DojoTicketsNonEvanBlocked;
        public static readonly System.Collections.Frozen.FrozenSet<Maple.StrongId.ItemTemplateId> EngagementRings;
        public static readonly System.Collections.Frozen.FrozenSet<Maple.StrongId.ItemTemplateId> EvanDragonRidingItems;
        public static readonly System.Collections.Frozen.FrozenSet<Maple.StrongId.ItemTemplateId> InvitationBundles;
        public static readonly System.Collections.Frozen.FrozenSet<Maple.StrongId.ItemTemplateId> InvitationGuests;
        public static readonly System.Collections.Frozen.FrozenSet<Maple.StrongId.ItemTemplateId> PetRings;
        public static readonly System.Collections.Frozen.FrozenSet<Maple.StrongId.ItemTemplateId> WeddingCantDropItems;
        public static readonly System.Collections.Frozen.FrozenSet<Maple.StrongId.ItemTemplateId> WeddingReceiptItems;
        public static readonly System.Collections.Frozen.FrozenSet<Maple.StrongId.ItemTemplateId> WeddingRings;
        public static readonly Maple.StrongId.ItemTemplateId WhiteScroll;
        public ItemTemplateId(int Value) { }
        public int CashSlotItemType { get; }
        public int Category { get; }
        public int EquipGender { get; }
        public int GenderDigit { get; }
        public int InventoryTab { get; }
        public bool IsAccUpgrade { get; }
        public bool IsAndroidEquip { get; }
        public bool IsAndroidPart { get; }
        public bool IsAntiMacro { get; }
        public bool IsAnyEquipScroll { get; }
        public bool IsBelt { get; }
        public bool IsBlackUpgrade { get; }
        public bool IsBook { get; }
        public bool IsBottom { get; }
        public bool IsBridle { get; }
        public bool IsBundle { get; }
        public bool IsCape { get; }
        public bool IsCash { get; }
        public bool IsCashMorph { get; }
        public bool IsCashPackage { get; }
        public bool IsCashPet { get; }
        public bool IsCashPetFood { get; }
        public bool IsChangeMaplePoint { get; }
        public bool IsCharSlotInc { get; }
        public bool IsConsume { get; }
        public bool IsCoupleEquip { get; }
        public bool IsDragonWing { get; }
        public bool IsDualMasteryBook { get; }
        public bool IsDurabilityUpgrade { get; }
        public bool IsEarring { get; }
        public bool IsEngagementRingBox { get; }
        public bool IsEngagementRingItem { get; }
        public bool IsEquip { get; }
        public bool IsEquipSlotExt { get; }
        public bool IsEtc { get; }
        public bool IsEvanDragonRidingItem { get; }
        public bool IsEventVehicleType1 { get; }
        public bool IsEventVehicleType2 { get; }
        public bool IsExpUp { get; }
        public bool IsExtendExpireDate { get; }
        public bool IsEyeAcc { get; }
        public bool IsFaceAcc { get; }
        public bool IsFriendshipEquip { get; }
        public bool IsGachaponBox { get; }
        public bool IsGloves { get; }
        public bool IsHat { get; }
        public bool IsHyperUpgrade { get; }
        public bool IsImmediateMobSummon { get; }
        public bool IsInvitationBundle { get; }
        public bool IsInvitationGuest { get; }
        public bool IsItemOptionUpgrade { get; }
        public bool IsLongCoat { get; }
        public bool IsMapTransfer { get; }
        public bool IsMasteryBook { get; }
        public bool IsMedal { get; }
        public bool IsMesoProtect { get; }
        public bool IsMinigame { get; }
        public bool IsMobSummon { get; }
        public bool IsMorph { get; }
        public bool IsNamingItem { get; }
        public bool IsNewUpgrade { get; }
        public bool IsNewYearCardConsume { get; }
        public bool IsNewYearCardEtc { get; }
        public bool IsNonCashEffect { get; }
        public bool IsNonStackable { get; }
        public bool IsOneHandedWeapon { get; }
        public bool IsOnlyForPrepaid { get; }
        public bool IsPendant { get; }
        public bool IsPetAbility { get; }
        public bool IsPetEquip { get; }
        public bool IsPetFood { get; }
        public bool IsPetRing { get; }
        public bool IsPigmyEgg { get; }
        public bool IsPortableChair { get; }
        public bool IsPortalScroll { get; }
        public bool IsRaise { get; }
        public bool IsRandomMorphOther { get; }
        public bool IsRechargeable { get; }
        public bool IsRelease { get; }
        public bool IsRing { get; }
        public bool IsSaddle { get; }
        public bool IsScriptRun { get; }
        public bool IsSelectNpc { get; }
        public bool IsSetup { get; }
        public bool IsShield { get; }
        public bool IsShoes { get; }
        public bool IsShopScanner { get; }
        public bool IsSkillEffectWeapon { get; }
        public bool IsSkillLearn { get; }
        public bool IsSkillReset { get; }
        public bool IsSlotInc { get; }
        public bool IsSpecialItem { get; }
        public bool IsStateChange { get; }
        public bool IsTamingMobFood { get; }
        public bool IsTamingMobItem { get; }
        public bool IsTop { get; }
        public bool IsTopOrOverall { get; }
        public bool IsTrunkCountInc { get; }
        public bool IsTwoHandedWeapon { get; }
        public bool IsUiOpen { get; }
        public bool IsVehicle { get; }
        public bool IsVehicleEquipB { get; }
        public bool IsWeapon { get; }
        public bool IsWeaponSticker { get; }
        public bool IsWeddingCantDrop { get; }
        public bool IsWeddingReceipt { get; }
        public bool IsWeddingRingItem { get; }
        public bool IsWhiteScrollNoConsume { get; }
        public int ScrollTargetSubType { get; }
        public int SubType { get; }
        public int TypeIndex { get; }
        public int Value { get; init; }
        public int WeaponSubType { get; }
        [System.Obsolete("Allocates. Use TryWriteWzImageName(Span<char>, out int) on hot paths.")]
        public string WzImageName { get; }
        public bool CanRegularScrollApplyTo(Maple.StrongId.ItemTemplateId equip) { }
        public int CompareTo(Maple.StrongId.ItemTemplateId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryWriteWzImageName(System.Span<char> destination, out int charsWritten) { }
        public static Maple.StrongId.ItemTemplateId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.ItemTemplateId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.ItemTemplateId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.ItemTemplateId result) { }
        public static Maple.StrongId.ItemTemplateId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.ItemTemplateId id) { }
        public static bool operator <(Maple.StrongId.ItemTemplateId left, Maple.StrongId.ItemTemplateId right) { }
        public static bool operator <=(Maple.StrongId.ItemTemplateId left, Maple.StrongId.ItemTemplateId right) { }
        public static bool operator >(Maple.StrongId.ItemTemplateId left, Maple.StrongId.ItemTemplateId right) { }
        public static bool operator >=(Maple.StrongId.ItemTemplateId left, Maple.StrongId.ItemTemplateId right) { }
        public sealed class StrongIdJsonConverter : System.Text.Json.Serialization.JsonConverter<Maple.StrongId.ItemTemplateId>
        {
            public StrongIdJsonConverter() { }
            public override Maple.StrongId.ItemTemplateId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) { }
            public override void Write(System.Text.Json.Utf8JsonWriter writer, Maple.StrongId.ItemTemplateId value, System.Text.Json.JsonSerializerOptions options) { }
        }
    }
    [Maple.StrongId.StrongIntId]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    [System.Text.Json.Serialization.JsonConverter(typeof(Maple.StrongId.JobId.StrongIdJsonConverter))]
    public readonly struct JobId : System.IComparable<Maple.StrongId.JobId>, System.IEquatable<Maple.StrongId.JobId>, System.IFormattable, System.IParsable<Maple.StrongId.JobId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.JobId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.JobId, Maple.StrongId.JobId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.JobId, Maple.StrongId.JobId, bool>
    {
        public const int MaxReqJobNames = 5;
        public const int MaxSkillRoots = 10;
        public const int ReqJobAll = 0;
        public const int ReqJobBeginner = -1;
        public JobId(int Value) { }
        public int Branch { get; }
        public int Category { get; }
        public int DualJobChangeLevel { get; }
        public int EquipBit { get; }
        public int FieldCategoryCode { get; }
        public int HpPerLevel { get; }
        public bool IsAdmin { get; }
        public bool IsAran { get; }
        public bool IsBattleMage { get; }
        public bool IsBeginner { get; }
        public bool IsCygnus { get; }
        public bool IsDualBlade { get; }
        public bool IsEvan { get; }
        public bool IsExplorer { get; }
        public bool IsExtendSP { get; }
        public bool IsMage { get; }
        public bool IsManager { get; }
        public bool IsMapleHero { get; }
        public bool IsMechanic { get; }
        public bool IsResistance { get; }
        public bool IsValidJob { get; }
        public bool IsWildHunter { get; }
        public int Lineage { get; }
        public int MpPerLevel { get; }
        public int Tier { get; }
        public int TierDigit { get; }
        public int Value { get; init; }
        public static System.Collections.Frozen.FrozenSet<Maple.StrongId.JobId> AllValidJobIds { get; }
        public bool CanEquip(int nrJob) { }
        public bool CanUseSkillRoot(int nSkillRoot) { }
        public int CompareTo(Maple.StrongId.JobId other) { }
        public int GetAdvanceLevel(short nSubJob, int nStep) { }
        public string GetJobIconPath(bool bOnline) { }
        public int GetSkillRoots(System.Span<int> destination) { }
        public bool IsDualBladeBorn(short nSubJob) { }
        public bool IsMatchedForMuLungItem(Maple.StrongId.ItemTemplateId item) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public static int DecodeReqJob(int nrJob, System.Span<string> destination) { }
        public static Maple.StrongId.JobId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.JobId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.JobId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.JobId result) { }
        public static Maple.StrongId.JobId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.JobId id) { }
        public static bool operator <(Maple.StrongId.JobId left, Maple.StrongId.JobId right) { }
        public static bool operator <=(Maple.StrongId.JobId left, Maple.StrongId.JobId right) { }
        public static bool operator >(Maple.StrongId.JobId left, Maple.StrongId.JobId right) { }
        public static bool operator >=(Maple.StrongId.JobId left, Maple.StrongId.JobId right) { }
        public sealed class StrongIdJsonConverter : System.Text.Json.Serialization.JsonConverter<Maple.StrongId.JobId>
        {
            public StrongIdJsonConverter() { }
            public override Maple.StrongId.JobId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) { }
            public override void Write(System.Text.Json.Utf8JsonWriter writer, Maple.StrongId.JobId value, System.Text.Json.JsonSerializerOptions options) { }
        }
    }
    [Maple.StrongId.StrongIntId]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    [System.Text.Json.Serialization.JsonConverter(typeof(Maple.StrongId.MobTemplateId.StrongIdJsonConverter))]
    public readonly struct MobTemplateId : System.IComparable<Maple.StrongId.MobTemplateId>, System.IEquatable<Maple.StrongId.MobTemplateId>, System.IFormattable, System.IParsable<Maple.StrongId.MobTemplateId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.MobTemplateId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.MobTemplateId, Maple.StrongId.MobTemplateId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.MobTemplateId, Maple.StrongId.MobTemplateId, bool>
    {
        public MobTemplateId(int Value) { }
        public int Family { get; }
        public bool IsJaguarBuffImmune { get; }
        public bool IsLevelVisible { get; }
        public bool IsNotCapturable { get; }
        public bool IsNotSwallowable { get; }
        public bool IsPetTemplate { get; }
        public bool IsTamingMob { get; }
        public int PetSerial { get; }
        public int Series { get; }
        public int Value { get; init; }
        [System.Obsolete("Allocates. Use TryWriteWzImageName(Span<char>, out int) on hot paths.")]
        public string WzImageName { get; }
        public int CompareTo(Maple.StrongId.MobTemplateId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryWriteWzImageName(System.Span<char> destination, out int charsWritten) { }
        public static Maple.StrongId.MobTemplateId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.MobTemplateId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.MobTemplateId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.MobTemplateId result) { }
        public static Maple.StrongId.MobTemplateId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.MobTemplateId id) { }
        public static bool operator <(Maple.StrongId.MobTemplateId left, Maple.StrongId.MobTemplateId right) { }
        public static bool operator <=(Maple.StrongId.MobTemplateId left, Maple.StrongId.MobTemplateId right) { }
        public static bool operator >(Maple.StrongId.MobTemplateId left, Maple.StrongId.MobTemplateId right) { }
        public static bool operator >=(Maple.StrongId.MobTemplateId left, Maple.StrongId.MobTemplateId right) { }
        public sealed class StrongIdJsonConverter : System.Text.Json.Serialization.JsonConverter<Maple.StrongId.MobTemplateId>
        {
            public StrongIdJsonConverter() { }
            public override Maple.StrongId.MobTemplateId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) { }
            public override void Write(System.Text.Json.Utf8JsonWriter writer, Maple.StrongId.MobTemplateId value, System.Text.Json.JsonSerializerOptions options) { }
        }
    }
    [Maple.StrongId.StrongIntId]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    [System.Text.Json.Serialization.JsonConverter(typeof(Maple.StrongId.MorphTemplateId.StrongIdJsonConverter))]
    public readonly struct MorphTemplateId : System.IComparable<Maple.StrongId.MorphTemplateId>, System.IEquatable<Maple.StrongId.MorphTemplateId>, System.IFormattable, System.IParsable<Maple.StrongId.MorphTemplateId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.MorphTemplateId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.MorphTemplateId, Maple.StrongId.MorphTemplateId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.MorphTemplateId, Maple.StrongId.MorphTemplateId, bool>
    {
        public static readonly Maple.StrongId.MorphTemplateId InstantTransitionId;
        public static readonly System.Collections.Frozen.FrozenSet<Maple.StrongId.MorphTemplateId> SuperManIds;
        public MorphTemplateId(int Value) { }
        public bool IsInstantTransition { get; }
        public bool IsSuperMan { get; }
        public int Value { get; init; }
        public int CompareTo(Maple.StrongId.MorphTemplateId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public static Maple.StrongId.MorphTemplateId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.MorphTemplateId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.MorphTemplateId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.MorphTemplateId result) { }
        public static Maple.StrongId.MorphTemplateId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.MorphTemplateId id) { }
        public static bool operator <(Maple.StrongId.MorphTemplateId left, Maple.StrongId.MorphTemplateId right) { }
        public static bool operator <=(Maple.StrongId.MorphTemplateId left, Maple.StrongId.MorphTemplateId right) { }
        public static bool operator >(Maple.StrongId.MorphTemplateId left, Maple.StrongId.MorphTemplateId right) { }
        public static bool operator >=(Maple.StrongId.MorphTemplateId left, Maple.StrongId.MorphTemplateId right) { }
        public sealed class StrongIdJsonConverter : System.Text.Json.Serialization.JsonConverter<Maple.StrongId.MorphTemplateId>
        {
            public StrongIdJsonConverter() { }
            public override Maple.StrongId.MorphTemplateId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) { }
            public override void Write(System.Text.Json.Utf8JsonWriter writer, Maple.StrongId.MorphTemplateId value, System.Text.Json.JsonSerializerOptions options) { }
        }
    }
    [Maple.StrongId.StrongIntId]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    [System.Text.Json.Serialization.JsonConverter(typeof(Maple.StrongId.NpcTemplateId.StrongIdJsonConverter))]
    public readonly struct NpcTemplateId : System.IComparable<Maple.StrongId.NpcTemplateId>, System.IEquatable<Maple.StrongId.NpcTemplateId>, System.IFormattable, System.IParsable<Maple.StrongId.NpcTemplateId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.NpcTemplateId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.NpcTemplateId, Maple.StrongId.NpcTemplateId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.NpcTemplateId, Maple.StrongId.NpcTemplateId, bool>
    {
        public static readonly Maple.StrongId.NpcTemplateId AranTutor;
        public static readonly Maple.StrongId.NpcTemplateId EvanDragon;
        public static readonly Maple.StrongId.NpcTemplateId Kin;
        public static readonly Maple.StrongId.NpcTemplateId Nimakin;
        public static readonly Maple.StrongId.NpcTemplateId NoblesseTutor;
        public NpcTemplateId(int Value) { }
        public int Value { get; init; }
        [System.Obsolete("Allocates. Use TryWriteWzImageName(Span<char>, out int) on hot paths.")]
        public string WzImageName { get; }
        public int CompareTo(Maple.StrongId.NpcTemplateId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryWriteWzImageName(System.Span<char> destination, out int charsWritten) { }
        public static Maple.StrongId.NpcTemplateId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.NpcTemplateId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.NpcTemplateId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.NpcTemplateId result) { }
        public static Maple.StrongId.NpcTemplateId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.NpcTemplateId id) { }
        public static bool operator <(Maple.StrongId.NpcTemplateId left, Maple.StrongId.NpcTemplateId right) { }
        public static bool operator <=(Maple.StrongId.NpcTemplateId left, Maple.StrongId.NpcTemplateId right) { }
        public static bool operator >(Maple.StrongId.NpcTemplateId left, Maple.StrongId.NpcTemplateId right) { }
        public static bool operator >=(Maple.StrongId.NpcTemplateId left, Maple.StrongId.NpcTemplateId right) { }
        public sealed class StrongIdJsonConverter : System.Text.Json.Serialization.JsonConverter<Maple.StrongId.NpcTemplateId>
        {
            public StrongIdJsonConverter() { }
            public override Maple.StrongId.NpcTemplateId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) { }
            public override void Write(System.Text.Json.Utf8JsonWriter writer, Maple.StrongId.NpcTemplateId value, System.Text.Json.JsonSerializerOptions options) { }
        }
    }
    [Maple.StrongId.StrongIntId]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    [System.Text.Json.Serialization.JsonConverter(typeof(Maple.StrongId.PetTemplateId.StrongIdJsonConverter))]
    public readonly struct PetTemplateId : System.IComparable<Maple.StrongId.PetTemplateId>, System.IEquatable<Maple.StrongId.PetTemplateId>, System.IFormattable, System.IParsable<Maple.StrongId.PetTemplateId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.PetTemplateId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.PetTemplateId, Maple.StrongId.PetTemplateId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.PetTemplateId, Maple.StrongId.PetTemplateId, bool>
    {
        public PetTemplateId(int Value) { }
        public bool IsPetRange { get; }
        public int PetSerial { get; }
        public int Value { get; init; }
        [System.Obsolete("Allocates. Use TryWriteWzImageName(Span<char>, out int) on hot paths.")]
        public string WzImageName { get; }
        public int CompareTo(Maple.StrongId.PetTemplateId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryWriteWzImageName(System.Span<char> destination, out int charsWritten) { }
        public static bool IsEquipTypeCompatible(Maple.StrongId.ItemTemplateId petEquipItem) { }
        public static Maple.StrongId.PetTemplateId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.PetTemplateId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.PetTemplateId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.PetTemplateId result) { }
        public static Maple.StrongId.PetTemplateId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.PetTemplateId id) { }
        public static bool operator <(Maple.StrongId.PetTemplateId left, Maple.StrongId.PetTemplateId right) { }
        public static bool operator <=(Maple.StrongId.PetTemplateId left, Maple.StrongId.PetTemplateId right) { }
        public static bool operator >(Maple.StrongId.PetTemplateId left, Maple.StrongId.PetTemplateId right) { }
        public static bool operator >=(Maple.StrongId.PetTemplateId left, Maple.StrongId.PetTemplateId right) { }
        public sealed class StrongIdJsonConverter : System.Text.Json.Serialization.JsonConverter<Maple.StrongId.PetTemplateId>
        {
            public StrongIdJsonConverter() { }
            public override Maple.StrongId.PetTemplateId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) { }
            public override void Write(System.Text.Json.Utf8JsonWriter writer, Maple.StrongId.PetTemplateId value, System.Text.Json.JsonSerializerOptions options) { }
        }
    }
    [Maple.StrongId.StrongIntId]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    [System.Text.Json.Serialization.JsonConverter(typeof(Maple.StrongId.QuestTemplateId.StrongIdJsonConverter))]
    public readonly struct QuestTemplateId : System.IComparable<Maple.StrongId.QuestTemplateId>, System.IEquatable<Maple.StrongId.QuestTemplateId>, System.IFormattable, System.IParsable<Maple.StrongId.QuestTemplateId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.QuestTemplateId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.QuestTemplateId, Maple.StrongId.QuestTemplateId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.QuestTemplateId, Maple.StrongId.QuestTemplateId, bool>
    {
        public const int MaxQuestId = 65535;
        public static readonly Maple.StrongId.QuestTemplateId DeliveryExcluded;
        public QuestTemplateId(int Value) { }
        public bool IsExpeditionQuestInfoId { get; }
        public bool IsPartyQuest { get; }
        public bool IsPartyQuestInfoId { get; }
        public int Value { get; init; }
        public int CompareTo(Maple.StrongId.QuestTemplateId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public static Maple.StrongId.QuestTemplateId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.QuestTemplateId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.QuestTemplateId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.QuestTemplateId result) { }
        public static Maple.StrongId.QuestTemplateId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.QuestTemplateId id) { }
        public static bool operator <(Maple.StrongId.QuestTemplateId left, Maple.StrongId.QuestTemplateId right) { }
        public static bool operator <=(Maple.StrongId.QuestTemplateId left, Maple.StrongId.QuestTemplateId right) { }
        public static bool operator >(Maple.StrongId.QuestTemplateId left, Maple.StrongId.QuestTemplateId right) { }
        public static bool operator >=(Maple.StrongId.QuestTemplateId left, Maple.StrongId.QuestTemplateId right) { }
        public sealed class StrongIdJsonConverter : System.Text.Json.Serialization.JsonConverter<Maple.StrongId.QuestTemplateId>
        {
            public StrongIdJsonConverter() { }
            public override Maple.StrongId.QuestTemplateId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) { }
            public override void Write(System.Text.Json.Utf8JsonWriter writer, Maple.StrongId.QuestTemplateId value, System.Text.Json.JsonSerializerOptions options) { }
        }
    }
    [Maple.StrongId.StrongIntId]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    [System.Text.Json.Serialization.JsonConverter(typeof(Maple.StrongId.ReactorTemplateId.StrongIdJsonConverter))]
    public readonly struct ReactorTemplateId : System.IComparable<Maple.StrongId.ReactorTemplateId>, System.IEquatable<Maple.StrongId.ReactorTemplateId>, System.IFormattable, System.IParsable<Maple.StrongId.ReactorTemplateId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.ReactorTemplateId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.ReactorTemplateId, Maple.StrongId.ReactorTemplateId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.ReactorTemplateId, Maple.StrongId.ReactorTemplateId, bool>
    {
        public ReactorTemplateId(int Value) { }
        public int Value { get; init; }
        [System.Obsolete("Allocates. Use TryWriteWzImageName(Span<char>, out int) on hot paths.")]
        public string WzImageName { get; }
        public int CompareTo(Maple.StrongId.ReactorTemplateId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryWriteWzImageName(System.Span<char> destination, out int charsWritten) { }
        public static Maple.StrongId.ReactorTemplateId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.ReactorTemplateId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.ReactorTemplateId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.ReactorTemplateId result) { }
        public static Maple.StrongId.ReactorTemplateId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.ReactorTemplateId id) { }
        public static bool operator <(Maple.StrongId.ReactorTemplateId left, Maple.StrongId.ReactorTemplateId right) { }
        public static bool operator <=(Maple.StrongId.ReactorTemplateId left, Maple.StrongId.ReactorTemplateId right) { }
        public static bool operator >(Maple.StrongId.ReactorTemplateId left, Maple.StrongId.ReactorTemplateId right) { }
        public static bool operator >=(Maple.StrongId.ReactorTemplateId left, Maple.StrongId.ReactorTemplateId right) { }
        public sealed class StrongIdJsonConverter : System.Text.Json.Serialization.JsonConverter<Maple.StrongId.ReactorTemplateId>
        {
            public StrongIdJsonConverter() { }
            public override Maple.StrongId.ReactorTemplateId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) { }
            public override void Write(System.Text.Json.Utf8JsonWriter writer, Maple.StrongId.ReactorTemplateId value, System.Text.Json.JsonSerializerOptions options) { }
        }
    }
    [Maple.StrongId.StrongIntId]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    [System.Text.Json.Serialization.JsonConverter(typeof(Maple.StrongId.SetItemTemplateId.StrongIdJsonConverter))]
    public readonly struct SetItemTemplateId : System.IComparable<Maple.StrongId.SetItemTemplateId>, System.IEquatable<Maple.StrongId.SetItemTemplateId>, System.IFormattable, System.IParsable<Maple.StrongId.SetItemTemplateId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.SetItemTemplateId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.SetItemTemplateId, Maple.StrongId.SetItemTemplateId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.SetItemTemplateId, Maple.StrongId.SetItemTemplateId, bool>
    {
        public const int MaxPartsCount = 60;
        public static readonly Maple.StrongId.SetItemTemplateId None;
        public SetItemTemplateId(int Value) { }
        public bool IsNoSet { get; }
        public int Value { get; init; }
        public int CompareTo(Maple.StrongId.SetItemTemplateId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public static Maple.StrongId.SetItemTemplateId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.SetItemTemplateId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.SetItemTemplateId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.SetItemTemplateId result) { }
        public static Maple.StrongId.SetItemTemplateId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.SetItemTemplateId id) { }
        public static bool operator <(Maple.StrongId.SetItemTemplateId left, Maple.StrongId.SetItemTemplateId right) { }
        public static bool operator <=(Maple.StrongId.SetItemTemplateId left, Maple.StrongId.SetItemTemplateId right) { }
        public static bool operator >(Maple.StrongId.SetItemTemplateId left, Maple.StrongId.SetItemTemplateId right) { }
        public static bool operator >=(Maple.StrongId.SetItemTemplateId left, Maple.StrongId.SetItemTemplateId right) { }
        public sealed class StrongIdJsonConverter : System.Text.Json.Serialization.JsonConverter<Maple.StrongId.SetItemTemplateId>
        {
            public StrongIdJsonConverter() { }
            public override Maple.StrongId.SetItemTemplateId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) { }
            public override void Write(System.Text.Json.Utf8JsonWriter writer, Maple.StrongId.SetItemTemplateId value, System.Text.Json.JsonSerializerOptions options) { }
        }
    }
    [Maple.StrongId.StrongIntId]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    [System.Text.Json.Serialization.JsonConverter(typeof(Maple.StrongId.SkillTemplateId.StrongIdJsonConverter))]
    public readonly struct SkillTemplateId : System.IComparable<Maple.StrongId.SkillTemplateId>, System.IEquatable<Maple.StrongId.SkillTemplateId>, System.IFormattable, System.IParsable<Maple.StrongId.SkillTemplateId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.SkillTemplateId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.SkillTemplateId, Maple.StrongId.SkillTemplateId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.SkillTemplateId, Maple.StrongId.SkillTemplateId, bool>
    {
        public SkillTemplateId(int Value) { }
        public bool IsActive { get; }
        public bool IsAdminSkill900 { get; }
        public bool IsAnyAdminSkill { get; }
        public bool IsAranCombo { get; }
        public bool IsBaseClassSkill { get; }
        public bool IsBattleMageAura { get; }
        public bool IsCommon { get; }
        public bool IsEventVehicleSkill { get; }
        public bool IsExtendSPOwner { get; }
        public bool IsGuildSkill { get; }
        public bool IsHeroWill { get; }
        public bool IsKeyDown { get; }
        public bool IsNoblesseSkill { get; }
        public bool IsNonSlot { get; }
        public bool IsNovice { get; }
        public bool IsPassive { get; }
        public bool IsPrepareBomb { get; }
        public bool IsSuperGmSkill { get; }
        public bool IsTeleport { get; }
        public bool IsTeleportMastery { get; }
        public bool IsUnregistered { get; }
        public bool IsVehicleSkill { get; }
        public int JobCategory { get; }
        public int JobLineage { get; }
        public bool NeedsMasterLevel { get; }
        public int OwnerJobDegree { get; }
        public int Root { get; }
        public int Serial { get; }
        public int Value { get; init; }
        [System.Obsolete("Allocates. Use TryWriteWzImageName(Span<char>, out int) on hot paths.")]
        public string WzImageName { get; }
        [System.Obsolete("Allocates. Use TryWriteWzSkillNodePath(Span<char>, out int) on hot paths.")]
        public string WzSkillNodePath { get; }
        public bool CanBeUsedByJob(Maple.StrongId.JobId job) { }
        public int CompareTo(Maple.StrongId.SkillTemplateId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryWriteWzImageName(System.Span<char> destination, out int charsWritten) { }
        public bool TryWriteWzSkillNodePath(System.Span<char> destination, out int charsWritten) { }
        public static Maple.StrongId.SkillTemplateId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.SkillTemplateId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.SkillTemplateId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.SkillTemplateId result) { }
        public static Maple.StrongId.SkillTemplateId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.SkillTemplateId id) { }
        public static bool operator <(Maple.StrongId.SkillTemplateId left, Maple.StrongId.SkillTemplateId right) { }
        public static bool operator <=(Maple.StrongId.SkillTemplateId left, Maple.StrongId.SkillTemplateId right) { }
        public static bool operator >(Maple.StrongId.SkillTemplateId left, Maple.StrongId.SkillTemplateId right) { }
        public static bool operator >=(Maple.StrongId.SkillTemplateId left, Maple.StrongId.SkillTemplateId right) { }
        public static class Admin
        {
            public static readonly Maple.StrongId.SkillTemplateId AntiMacro;
            public static readonly Maple.StrongId.SkillTemplateId Bless;
            public static readonly Maple.StrongId.SkillTemplateId Dispel;
            public static readonly Maple.StrongId.SkillTemplateId DragonRoar;
            public static readonly Maple.StrongId.SkillTemplateId Haste;
            public static readonly Maple.StrongId.SkillTemplateId Hide;
            public static readonly Maple.StrongId.SkillTemplateId HolySymbol;
            public static readonly Maple.StrongId.SkillTemplateId HyperBody;
            public static readonly Maple.StrongId.SkillTemplateId Resurrection;
            public static readonly Maple.StrongId.SkillTemplateId SuperHaste;
            public static readonly Maple.StrongId.SkillTemplateId Teleport;
        }
        public static class Guild
        {
            public static readonly Maple.StrongId.SkillTemplateId AgilityUp;
            public static readonly Maple.StrongId.SkillTemplateId AttMagUp;
            public static readonly Maple.StrongId.SkillTemplateId BusinessEfficacyUp;
            public static readonly Maple.StrongId.SkillTemplateId DefenceUp;
            public static readonly Maple.StrongId.SkillTemplateId ExperienceUp;
            public static readonly Maple.StrongId.SkillTemplateId MesoUp;
            public static readonly Maple.StrongId.SkillTemplateId RegularSupport;
        }
        public sealed class StrongIdJsonConverter : System.Text.Json.Serialization.JsonConverter<Maple.StrongId.SkillTemplateId>
        {
            public StrongIdJsonConverter() { }
            public override Maple.StrongId.SkillTemplateId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) { }
            public override void Write(System.Text.Json.Utf8JsonWriter writer, Maple.StrongId.SkillTemplateId value, System.Text.Json.JsonSerializerOptions options) { }
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Struct, AllowMultiple=false, Inherited=false)]
    public sealed class StrongIntIdAttribute : System.Attribute
    {
        public StrongIntIdAttribute() { }
        public bool GenerateJsonConverter { get; init; }
    }
    [Maple.StrongId.StrongIntId]
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    [System.Text.Json.Serialization.JsonConverter(typeof(Maple.StrongId.TamingMobTemplateId.StrongIdJsonConverter))]
    public readonly struct TamingMobTemplateId : System.IComparable<Maple.StrongId.TamingMobTemplateId>, System.IEquatable<Maple.StrongId.TamingMobTemplateId>, System.IFormattable, System.IParsable<Maple.StrongId.TamingMobTemplateId>, System.ISpanFormattable, System.ISpanParsable<Maple.StrongId.TamingMobTemplateId>, System.IUtf8SpanFormattable, System.Numerics.IComparisonOperators<Maple.StrongId.TamingMobTemplateId, Maple.StrongId.TamingMobTemplateId, bool>, System.Numerics.IEqualityOperators<Maple.StrongId.TamingMobTemplateId, Maple.StrongId.TamingMobTemplateId, bool>
    {
        public static readonly Maple.StrongId.TamingMobTemplateId Mir1;
        public static readonly Maple.StrongId.TamingMobTemplateId Mir2;
        public static readonly Maple.StrongId.TamingMobTemplateId Mir3;
        public static readonly Maple.StrongId.TamingMobTemplateId RedDraco;
        public static readonly Maple.StrongId.TamingMobTemplateId Ryuho100;
        public static readonly Maple.StrongId.TamingMobTemplateId Ryuho150;
        public static readonly Maple.StrongId.TamingMobTemplateId Ryuho200;
        public static readonly Maple.StrongId.TamingMobTemplateId Ryuho50;
        public TamingMobTemplateId(int Value) { }
        public bool IsEvanDragon { get; }
        public bool IsRedDraco { get; }
        public bool IsRyuho { get; }
        public bool IsTamingMobRange { get; }
        public int Value { get; init; }
        [System.Obsolete("Allocates. Use TryWriteWzImageName(Span<char>, out int) on hot paths.")]
        public string WzImageName { get; }
        public int CompareTo(Maple.StrongId.TamingMobTemplateId other) { }
        public override string ToString() { }
        public string ToString(string? format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<byte> utf8Destination, out int bytesWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider) { }
        public bool TryWriteWzImageName(System.Span<char> destination, out int charsWritten) { }
        public static Maple.StrongId.TamingMobTemplateId Parse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider) { }
        public static Maple.StrongId.TamingMobTemplateId Parse(string s, System.IFormatProvider? provider) { }
        public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? provider, out Maple.StrongId.TamingMobTemplateId result) { }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? s, System.IFormatProvider? provider, out Maple.StrongId.TamingMobTemplateId result) { }
        public static Maple.StrongId.TamingMobTemplateId op_Explicit(int value) { }
        public static int op_Implicit(Maple.StrongId.TamingMobTemplateId id) { }
        public static bool operator <(Maple.StrongId.TamingMobTemplateId left, Maple.StrongId.TamingMobTemplateId right) { }
        public static bool operator <=(Maple.StrongId.TamingMobTemplateId left, Maple.StrongId.TamingMobTemplateId right) { }
        public static bool operator >(Maple.StrongId.TamingMobTemplateId left, Maple.StrongId.TamingMobTemplateId right) { }
        public static bool operator >=(Maple.StrongId.TamingMobTemplateId left, Maple.StrongId.TamingMobTemplateId right) { }
        public sealed class StrongIdJsonConverter : System.Text.Json.Serialization.JsonConverter<Maple.StrongId.TamingMobTemplateId>
        {
            public StrongIdJsonConverter() { }
            public override Maple.StrongId.TamingMobTemplateId Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) { }
            public override void Write(System.Text.Json.Utf8JsonWriter writer, Maple.StrongId.TamingMobTemplateId value, System.Text.Json.JsonSerializerOptions options) { }
        }
    }
}
```
