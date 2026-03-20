namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for a NPC template ID.
/// NPC templates are loaded from <c>Npc.wz</c> into the <c>CNpcTemplate</c> catalog.
/// No arithmetic operations beyond WZ path lookup are performed on NPC IDs.
/// </summary>
[StrongIntId]
public readonly partial record struct NpcTemplateId(int Value)
{
    // ── WZ Path ───────────────────────────────────────────────────────────

    /// <summary>
    /// WZ image filename for this NPC, as it appears in <c>Npc.wz</c>.
    /// Format: <c>"{id:D7}.img"</c> (7 digits, zero-padded).
    /// Full path example: <c>Npc.wz/9010000.img</c>.
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

    // ── Well-Known NPC ID Constants ───────────────────────────────────────

    /// <summary>
    /// Evan Dragon NPC. Excluded as a sentinel from quest delivery-accept checks.
    /// </summary>
    public static readonly NpcTemplateId EvanDragon = new(1_013_000);

    /// <summary>Cygnus Noblesse tutorial NPC.</summary>
    public static readonly NpcTemplateId NoblesseTutor = new(1_101_008);

    /// <summary>Aran tutorial NPC.</summary>
    public static readonly NpcTemplateId AranTutor = new(1_202_000);

    /// <summary>Special GM/event NPC (Kin).</summary>
    public static readonly NpcTemplateId Kin = new(9_900_000);

    /// <summary>Special GM/event NPC (Nimakin), companion to Kin.</summary>
    public static readonly NpcTemplateId Nimakin = new(9_900_001);
}
