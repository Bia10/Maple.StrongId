namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for a character ID.
/// <para>
/// Character IDs are opaque server-assigned keys in general use.
/// One exception: the summon dispatch system masks the ID with <c>0x3FFFFFFF</c>
/// (lower 30 bits) to obtain a summon slot index; the upper 2 bits (0xC0000000)
/// are reserved for summon ownership flags set by the summon system.
/// </para>
/// </summary>
[StrongIntId(GenerateJsonConverter = false)]
public readonly partial record struct CharacterId(int Value)
{
    // ── Summon Slot Extraction ─────────────────────────────────────────────

    /// <summary>
    /// Lower 30 bits of the character ID, used as a summon slot index.
    /// Formula: <c>nCharacterID &amp; 0x3FFFFFFF</c>.
    /// The upper 2 bits (mask 0xC0000000) are reserved for summon ownership flags
    /// and are stripped before slot lookup.
    /// </summary>
    public int SummonIndex => Value & 0x3FFF_FFFF;

    // ── Range Constant ────────────────────────────────────────────────────

    /// <summary>
    /// Maximum value of a "natural" character ID (no summon flags set): 2^30 − 1 = 1,073,741,823.
    /// Server-assigned IDs that fit within 30 bits satisfy <c>Value &lt;= MaxNaturalValue</c>.
    /// </summary>
    public const int MaxNaturalValue = 0x3FFF_FFFF;
}
