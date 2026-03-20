namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for a reactor template ID.
/// Reactor templates are loaded lazily from <c>Reactor.wz</c> on first use — not bulk-preloaded.
/// </summary>
[StrongIntId]
public readonly partial record struct ReactorTemplateId(int Value)
{
    // ── WZ Path ───────────────────────────────────────────────────────────

    /// <summary>
    /// WZ image filename for this reactor, as it appears in <c>Reactor.wz</c>.
    /// Format: <c>"{id:D7}.img"</c> (7 digits, zero-padded).
    /// Full path example: <c>Reactor.wz/0100100.img</c>.
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
