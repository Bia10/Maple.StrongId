namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for an employee template ID.
/// Employee templates represent crafting-system NPCs (professions/recipe sellers).
/// </summary>
[StrongIntId]
public readonly partial record struct EmployeeTemplateId(int Value)
{
    // ── WZ Path ───────────────────────────────────────────────────────────

    /// <summary>
    /// WZ image filename for this employee NPC, as it appears in <c>Npc.wz</c>.
    /// Format: <c>"{id:D7}.img"</c> (7 digits, zero-padded), consistent with the NPC template WZ path convention.
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
