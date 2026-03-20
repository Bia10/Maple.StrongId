namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for a set item ID.
/// Set item IDs are small sequential integers used as keys in
/// <c>Etc.wz/SetItemInfo.img/{setId}/</c>.
/// Set bonuses activate as more set pieces are equipped; the WZ data defines bonus stats
/// for each threshold (1 piece, 2 pieces, …) up to <see cref="MaxPartsCount"/> slots.
/// No arithmetic or range decomposition is applied to the ID itself.
/// </summary>
[StrongIntId]
public readonly partial record struct SetItemTemplateId(int Value)
{
    // ── Null Sentinel ─────────────────────────────────────────────────────

    /// <summary>
    /// The null / "no set" sentinel (value 0) for equip items that do not belong to any set.
    /// Set-bonus processing is skipped for items with this value.
    /// </summary>
    public static readonly SetItemTemplateId None = new(0);

    // ── Predicates ────────────────────────────────────────────────────────

    /// <summary>
    /// True when this ID is the null / "no set" sentinel (Value == 0).
    /// Equip items not belonging to any set have this value; set-bonus processing is skipped for them.
    /// </summary>
    public bool IsNoSet => Value == 0;

    // ── Well-Known Constants ──────────────────────────────────────────────

    /// <summary>Maximum number of item slots in a single set definition (60).</summary>
    public const int MaxPartsCount = 60;
}
