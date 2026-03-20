using System.Collections.Frozen;

namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for a morph template ID.
/// Morph templates are loaded from <c>Morph.wz</c> into a dedicated catalog, separate
/// from <see cref="MobTemplateId"/>. Known IDs are small sequential integers (e.g., 1000, 1001, 1100, 1101).
/// Morph attributes (movability, superman, hide, etc.) are WZ data fields — not derivable from the ID.
/// No arithmetic is applied to morph IDs; only equality checks are meaningful.
/// </summary>
[StrongIntId]
public readonly partial record struct MorphTemplateId(int Value)
{
    // ── Predicates ────────────────────────────────────────────────────────

    /// <summary>True for the four "SuperMan" morph IDs (1000, 1001, 1100, 1101).</summary>
    public bool IsSuperMan => SuperManIds.Contains(this);

    /// <summary>
    /// True when this morph uses an instant (zero-delay) transition animation.
    /// Only morph ID 1002 has zero delay; all other morphs use a 500 ms fade-in.
    /// </summary>
    public bool IsInstantTransition => Value == 1002;

    // ── Well-Known IDs ────────────────────────────────────────────────────

    /// <summary>The four SuperMan morph IDs.</summary>
    public static readonly FrozenSet<MorphTemplateId> SuperManIds = FrozenSet.ToFrozenSet<MorphTemplateId>([
        new(1000),
        new(1001),
        new(1100),
        new(1101),
    ]);

    /// <summary>Morph ID with an instant (zero-delay) transition instead of the normal 500 ms fade-in.</summary>
    public static readonly MorphTemplateId InstantTransitionId = new(1002);
}
