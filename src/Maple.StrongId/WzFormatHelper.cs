using System.Diagnostics;
using System.Globalization;

namespace Maple.StrongId;

/// <summary>
/// Shared zero-allocation helpers for writing WZ image filenames into caller-supplied spans.
/// Eliminates the verbatim-duplicate TryWriteWzImageName body that would otherwise appear
/// in every ID type that uses a fixed-width D7 or D8 format.
/// </summary>
internal static class WzFormatHelper
{
    /// <summary>
    /// Writes a 7-digit zero-padded ID followed by ".img" (11 chars total) into <paramref name="destination"/>.
    /// Returns <see langword="false"/> and sets <paramref name="charsWritten"/> to 0 if the span is too small.
    /// </summary>
    internal static bool TryWriteD7ImgName(int value, Span<char> destination, out int charsWritten)
    {
        // D7 (7) + ".img" (4) = 11 chars always.
        if (destination.Length < 11)
        {
            charsWritten = 0;
            return false;
        }

        bool ok = value.TryFormat(destination, out int n, "D7", CultureInfo.InvariantCulture);
        Debug.Assert(ok, "TryFormat D7 must succeed when destination.Length >= 7");
        ".img".AsSpan().CopyTo(destination[n..]);
        charsWritten = n + 4;
        return true;
    }

    /// <summary>
    /// Writes an 8-digit zero-padded ID followed by ".img" (12 chars total) into <paramref name="destination"/>.
    /// Returns <see langword="false"/> and sets <paramref name="charsWritten"/> to 0 if the span is too small.
    /// </summary>
    internal static bool TryWriteD8ImgName(int value, Span<char> destination, out int charsWritten)
    {
        // D8 (8) + ".img" (4) = 12 chars always.
        if (destination.Length < 12)
        {
            charsWritten = 0;
            return false;
        }

        bool ok = value.TryFormat(destination, out int n, "D8", CultureInfo.InvariantCulture);
        Debug.Assert(ok, "TryFormat D8 must succeed when destination.Length >= 8");
        ".img".AsSpan().CopyTo(destination[n..]);
        charsWritten = n + 4;
        return true;
    }
}
