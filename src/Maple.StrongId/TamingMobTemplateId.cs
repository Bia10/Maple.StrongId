namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for a taming-mob (mount) template ID.
/// Taming-mob templates are loaded from <c>Mob.wz</c> and occupy the 1_900_000 – 1_909_999 range
/// (see <see cref="MobTemplateId.IsTamingMob"/>). No arithmetic is applied to the ID itself.
/// </summary>
[StrongIntId]
public readonly partial record struct TamingMobTemplateId(int Value)
{
    // ── Validation ────────────────────────────────────────────────────────

    /// <summary>
    /// True for valid taming-mob template IDs (1_900_000 – 1_909_999).
    /// Formula: <c>nMobID / 10_000 == 190</c>.
    /// Mirrors <see cref="MobTemplateId.IsTamingMob"/> for the mob catalog,
    /// but applied here as a self-validation predicate on the taming-mob catalog key.
    /// </summary>
    public bool IsTamingMobRange => Value / 10_000 == 190;

    // ── Red Draco ─────────────────────────────────────────────────────────

    /// <summary>True for the Red Draco dragon mount (1_902_002).</summary>
    public bool IsRedDraco => Value == 1_902_002;

    // ── Ryuho Mounts ──────────────────────────────────────────────────────

    /// <summary>
    /// True for any of the four Ryuho stamina-tier mounts (1_902_015 – 1_902_018).
    /// Formula: <c>(uint)(Value - 1_902_015) &lt;= 3</c>.
    /// </summary>
    public bool IsRyuho => (uint)(Value - 1_902_015) <= 3u;

    // ── Evan Dragon (Mir) Mounts ──────────────────────────────────────────

    /// <summary>
    /// True for the three Evan dragon Mir taming-mob template IDs: 1_902_040, 1_902_041, 1_902_042.
    /// These are dispatched with special vehicle handling in the summon/vehicle system,
    /// distinct from ordinary taming mobs despite sharing the same IsTamingMobRange.
    /// </summary>
    public bool IsEvanDragon => Value is 1_902_040 or 1_902_041 or 1_902_042;

    // ── Well-Known Taming-Mob Constants ───────────────────────────────────

    /// <summary>Red Draco mount (1_902_002).</summary>
    public static readonly TamingMobTemplateId RedDraco = new(1_902_002);

    /// <summary>Ryuho stamina-50 mount (1_902_015).</summary>
    public static readonly TamingMobTemplateId Ryuho50 = new(1_902_015);

    /// <summary>Ryuho stamina-100 mount (1_902_016).</summary>
    public static readonly TamingMobTemplateId Ryuho100 = new(1_902_016);

    /// <summary>Ryuho stamina-150 mount (1_902_017).</summary>
    public static readonly TamingMobTemplateId Ryuho150 = new(1_902_017);

    /// <summary>Ryuho stamina-200 mount (1_902_018).</summary>
    public static readonly TamingMobTemplateId Ryuho200 = new(1_902_018);

    /// <summary>Evan dragon Mir, first evolution (1_902_040).</summary>
    public static readonly TamingMobTemplateId Mir1 = new(1_902_040);

    /// <summary>Evan dragon Mir, second evolution (1_902_041).</summary>
    public static readonly TamingMobTemplateId Mir2 = new(1_902_041);

    /// <summary>Evan dragon Mir, third evolution (1_902_042).</summary>
    public static readonly TamingMobTemplateId Mir3 = new(1_902_042);

    // ── WZ Path ───────────────────────────────────────────────────────────

    /// <summary>
    /// WZ image filename for this taming mob, as it appears in <c>Mob.wz</c>.
    /// Format: <c>"{id:D7}.img"</c> (7 digits, zero-padded).
    /// Full path example: <c>Mob.wz/1902000.img</c>.
    /// </summary>
    [Obsolete("Allocates. Use TryWriteWzImageName(Span<char>, out int) on hot paths.")]
    public string WzImageName => $"{Value:D7}.img";

    /// <summary>
    /// Writes the WZ image name (<c>"{id:D7}.img"</c>) into the provided span without heap allocation.
    /// </summary>
    public bool TryWriteWzImageName(Span<char> destination, out int charsWritten)
    {
        return WzFormatHelper.TryWriteD7ImgName(Value, destination, out charsWritten);
    }
}
