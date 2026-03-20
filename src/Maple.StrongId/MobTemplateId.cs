namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for a mob template ID.
/// </summary>
[StrongIntId]
public readonly partial record struct MobTemplateId(int Value)
{
    // ── Digit Decomposition ───────────────────────────────────────────────

    /// <summary>
    /// Top 2 digits of a 7-digit mob ID (nMobID / 100_000).
    /// Used for boss-tier classification.
    /// </summary>
    public int Family => Value / 100_000;

    /// <summary>
    /// Top 3 digits (nMobID / 10_000). Identifies specific boss series.
    /// </summary>
    public int Series => Value / 10_000;

    // ── Capture / Swallow ─────────────────────────────────────────────────

    // Shared boss-family check used by both IsNotCapturable and IsNotSwallowable.
    // Covers families 90–95, 97, and the 999 series (Zakum/Horntail/etc.).
    private bool IsBossFamily
    {
        get
        {
            int fam = Family;
            return (fam >= 90 && fam <= 95) || fam == 97 || Series == 999;
        }
    }

    /// <summary>
    /// True when this mob cannot be captured by Wild Hunter.
    /// Hardcoded exception: IDs 9_304_000–9_304_005 ARE capturable despite being in boss range.
    /// </summary>
    public bool IsNotCapturable =>
        (uint)(Value - 9_304_000) > 5 // hardcoded capturable exception
        && IsBossFamily;

    /// <summary>
    /// True when this mob cannot be swallowed by Wild Hunter Jaguar.
    /// ⚠ No capturable exception — 9_304_000–9_304_005 cannot be swallowed.
    /// </summary>
    public bool IsNotSwallowable => IsBossFamily;

    // ── Level Visibility ──────────────────────────────────────────────────

    /// <summary>
    /// True when the mob's level is shown on screen (template ID arithmetic portion only).
    /// Note: bBoss/bHideLevel/bHideHP/bHideName flags also suppress display.
    /// </summary>
    public bool IsLevelVisible
    {
        get
        {
            if (Value / 1_000_000 != 9)
                return true; // Non-9M range: always visible
            int fam = Family;
            return fam == 94 || fam == 96; // Only families 94 and 96 show within 9M
        }
    }

    // ── Special Families ──────────────────────────────────────────────────

    /// <summary>
    /// True for mob family 882 (8_820_000 – 8_829_999).
    /// These mobs are immune to Jaguar Riding's multiplicative PAD/PDR rate bonuses.
    /// </summary>
    public bool IsJaguarBuffImmune => Series == 882;

    /// <summary>
    /// True for all pet template IDs (5_000_000 – 5_009_999).
    /// Formula: nMobID / 10_000 == 500.
    /// </summary>
    public bool IsPetTemplate => Series == 500;

    /// <summary>
    /// Serial index (0–999) = bit position in an equipment item's uPetTemplateFlag bitmap.
    /// Only meaningful when IsPetTemplate.
    /// </summary>
    public int PetSerial => Value % 1_000;

    /// <summary>
    /// True for all taming/mount mob IDs (1_900_000 – 1_909_999).
    /// Formula: nMobID / 10_000 == 190.
    /// </summary>
    public bool IsTamingMob => Series == 190;

    // ── WZ Path ───────────────────────────────────────────────────────────

    /// <summary>
    /// WZ image filename for this mob, as it appears in <c>Mob.wz</c>.
    /// Format: <c>"{id:D7}.img"</c> (7 digits, zero-padded).
    /// Full path example: <c>Mob.wz/0100100.img</c>.
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
