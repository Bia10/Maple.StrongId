namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for a quest template ID.
/// Quest IDs are 16-bit unsigned integers in the server protocol (0–65535).
/// </summary>
[StrongIntId]
public readonly partial record struct QuestTemplateId(int Value)
{
    /// <summary>
    /// True for party quest IDs (1200 – 1399, inclusive).
    /// Formula: <c>(ushort)(id - 1200) &lt;= 0xC7</c> — standard unsigned-subtraction range trick.
    /// </summary>
    public bool IsPartyQuest => (uint)(Value - 1200) <= 0xC7u; // 0xC7 = 199 → range 1200–1399

    /// <summary>
    /// True for party quest info IDs in the party-quest manager catalog (1000 – 1999, inclusive).
    /// Formula: <c>nID / 1000 == 1</c>.
    /// Note: Broader than <see cref="IsPartyQuest"/> (1200–1399); all party quests also satisfy this predicate.
    /// </summary>
    public bool IsPartyQuestInfoId => Value / 1_000 == 1;

    /// <summary>
    /// True for expedition quest info IDs in the party-quest manager catalog (2000 – 2999, inclusive).
    /// Formula: <c>nID / 1000 == 2</c>.
    /// </summary>
    public bool IsExpeditionQuestInfoId => Value / 1_000 == 2;

    // ── Protocol Limits ───────────────────────────────────────────────────

    /// <summary>Maximum quest ID value — transmitted as an unsigned 16-bit integer in the server protocol (0–65535).</summary>
    public const int MaxQuestId = 65_535;

    // ── Well-Known Quest ID Constants ─────────────────────────────────────

    /// <summary>
    /// Quest ID explicitly excluded from NPC delivery-accept checks.
    /// This quest and <see cref="NpcTemplateId.EvanDragon"/> are the two hardcoded exclusions.
    /// </summary>
    public static readonly QuestTemplateId DeliveryExcluded = new(10_394);
}
