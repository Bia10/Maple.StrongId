namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for a pet template ID (5_000_000 – 5_009_999).
/// Pet templates are loaded from <c>Item.wz/Pet/</c> into a catalog separate from <see cref="MobTemplateId"/>.
/// The same integer is used as both the pet item ID in inventory (cash-pet slot) and the pet template key.
/// </summary>
[StrongIntId]
public readonly partial record struct PetTemplateId(int Value)
{
    // ── Validation ────────────────────────────────────────────────────────

    /// <summary>
    /// True for valid pet template IDs (5_000_000 – 5_009_999).
    /// Formula: <c>nPetID / 10_000 == 500</c>.
    /// </summary>
    public bool IsPetRange => Value / 10_000 == 500;

    // ── Equip Compatibility Bitmap ────────────────────────────────────────

    /// <summary>
    /// Serial index (0–999) — the bit position <c>N</c> in a pet equip item's 128-bit
    /// compatibility bitmap. Bit N set means the equip is compatible with pet template <c>5_000_000 + N</c>.
    /// Formula: <c>Value % 1_000</c>.
    /// </summary>
    public int PetSerial => Value % 1_000;

    /// <summary>
    /// True when the given equip item is the correct type for pet-equip compatibility checking.
    /// The equip must have typeindex 180 (1_800_000 – 1_809_999).
    /// This validates the item type only; the actual per-pet compatibility bit must be verified from WZ data.
    /// </summary>
    public static bool IsEquipTypeCompatible(ItemTemplateId petEquipItem) => petEquipItem.TypeIndex == 180;

    // ── WZ Path ───────────────────────────────────────────────────────────

    /// <summary>
    /// WZ image filename for this pet, as it appears in <c>Item.wz/Pet/</c>.
    /// Format: <c>"{id:D8}.img"</c> (8 digits, zero-padded).
    /// Icon path: <c>Item.wz/Pet/{id:D8}.img/info/icon</c>.
    /// Sprite path: <c>Character.wz/{id:D8}.img/...</c> (pet animation/appearance).
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
}
