using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Maple.StrongId.Test;

/// <summary>
/// Tests for the source-generated interfaces (IComparable, ISpanFormattable, IUtf8SpanFormattable,
/// IParsable, ISpanParsable, JsonConverter) on a representative ID type (SetItemTemplateId).
/// CharacterId is used where GenerateJsonConverter = false needs verification.
/// </summary>
public sealed class GeneratedCodeTests
{
    // ── Implicit / Explicit Operators ──────────────────────────────────────

    [Test]
    public async Task ImplicitToInt_ReturnsValue()
    {
        int raw = new SetItemTemplateId(42);
        await Assert.That(raw).IsEqualTo(42);
    }

    [Test]
    public async Task ExplicitFromInt_RoundTrips()
    {
        var id = (SetItemTemplateId)99;
        await Assert.That(id.Value).IsEqualTo(99);
    }

    // ── IComparable<T> ────────────────────────────────────────────────────

    [Test]
    public async Task CompareTo_OrdersCorrectly()
    {
        var a = new SetItemTemplateId(1);
        var b = new SetItemTemplateId(2);

        await Assert.That(a.CompareTo(b)).IsNegative();
        await Assert.That(b.CompareTo(a)).IsPositive();
        await Assert.That(a.CompareTo(a)).IsEqualTo(0);
    }

    // ── IComparisonOperators<T,T,bool> ────────────────────────────────────

    [Test]
    public async Task LessThan_OrdersCorrectly()
    {
        var lo = new SetItemTemplateId(1);
        var hi = new SetItemTemplateId(2);
        var loSame = new SetItemTemplateId(1);

        await Assert.That(lo < hi).IsTrue();
        await Assert.That(hi < lo).IsFalse();
        await Assert.That(lo < loSame).IsFalse(); // equal values → not strictly less
    }

    [Test]
    public async Task GreaterThan_OrdersCorrectly()
    {
        var lo = new SetItemTemplateId(1);
        var hi = new SetItemTemplateId(2);
        var loSame = new SetItemTemplateId(1);

        await Assert.That(hi > lo).IsTrue();
        await Assert.That(lo > hi).IsFalse();
        await Assert.That(lo > loSame).IsFalse(); // equal values → not strictly greater
    }

    [Test]
    public async Task LessThanOrEqual_OrdersCorrectly()
    {
        var lo = new SetItemTemplateId(1);
        var hi = new SetItemTemplateId(2);
        var loSame = new SetItemTemplateId(1);

        await Assert.That(lo <= hi).IsTrue();
        await Assert.That(lo <= loSame).IsTrue(); // equal values → true
        await Assert.That(hi <= lo).IsFalse();
    }

    [Test]
    public async Task GreaterThanOrEqual_OrdersCorrectly()
    {
        var lo = new SetItemTemplateId(1);
        var hi = new SetItemTemplateId(2);
        var hiSame = new SetItemTemplateId(2);

        await Assert.That(hi >= lo).IsTrue();
        await Assert.That(hi >= hiSame).IsTrue(); // equal values → true
        await Assert.That(lo >= hi).IsFalse();
    }

    [Test]
    public async Task ComparisonOperators_SatisfyIComparisonOperatorsConstraint()
    {
        // Compile-time proof: if SetItemTemplateId does not implement
        // IComparisonOperators<T,T,bool> this call will not compile.
        static bool LessThan<T>(T a, T b)
            where T : System.Numerics.IComparisonOperators<T, T, bool> => a < b;

        await Assert.That(LessThan(new SetItemTemplateId(1), new SetItemTemplateId(2))).IsTrue();
        await Assert.That(LessThan(new SetItemTemplateId(2), new SetItemTemplateId(1))).IsFalse();
    }

    // ── ToString ──────────────────────────────────────────────────────────

    [Test]
    public async Task ToString_ReturnsDecimalString()
    {
        await Assert.That(new SetItemTemplateId(123).ToString()).IsEqualTo("123");
    }

    [Test]
    public async Task ToStringFormatProvider_RespectsFormat()
    {
        IFormattable f = new SetItemTemplateId(42);
        await Assert.That(f.ToString("D5", CultureInfo.InvariantCulture)).IsEqualTo("00042");
    }

    // ── ISpanFormattable (TryFormat char) ─────────────────────────────────

    [Test]
    public async Task TryFormatChar_WritesValue()
    {
        ISpanFormattable sf = new SetItemTemplateId(7);
        var buf = new char[10];
        var ok = sf.TryFormat(buf, out int written, "D3", CultureInfo.InvariantCulture);
        var result = new string(buf, 0, written);

        await Assert.That(ok).IsTrue();
        await Assert.That(result).IsEqualTo("007");
    }

    [Test]
    public async Task TryFormatChar_FailsWhenTooSmall()
    {
        ISpanFormattable sf = new SetItemTemplateId(12345);
        var buf = new char[2];
        var ok = sf.TryFormat(buf, out int written, default, CultureInfo.InvariantCulture);

        await Assert.That(ok).IsFalse();
        await Assert.That(written).IsEqualTo(0);
    }

    // ── IUtf8SpanFormattable (TryFormat UTF-8) ────────────────────────────

    [Test]
    public async Task TryFormatUtf8_WritesValue()
    {
        IUtf8SpanFormattable usf = new SetItemTemplateId(7);
        var buf = new byte[10];
        var ok = usf.TryFormat(buf, out int written, "D3", CultureInfo.InvariantCulture);
        var result = Encoding.UTF8.GetString(buf, 0, written);

        await Assert.That(ok).IsTrue();
        await Assert.That(result).IsEqualTo("007");
    }

    [Test]
    public async Task TryFormatUtf8_FailsWhenTooSmall()
    {
        IUtf8SpanFormattable usf = new SetItemTemplateId(12345);
        var buf = new byte[2];
        var ok = usf.TryFormat(buf, out int written, default, CultureInfo.InvariantCulture);

        await Assert.That(ok).IsFalse();
        await Assert.That(written).IsEqualTo(0);
    }

    // ── IParsable<T> ──────────────────────────────────────────────────────

    [Test]
    public async Task Parse_String_RoundTrips()
    {
        var id = SetItemTemplateId.Parse("42", CultureInfo.InvariantCulture);
        await Assert.That(id.Value).IsEqualTo(42);
    }

    [Test]
    public async Task TryParse_String_Succeeds()
    {
        var ok = SetItemTemplateId.TryParse("99", CultureInfo.InvariantCulture, out var id);
        await Assert.That(ok).IsTrue();
        await Assert.That(id.Value).IsEqualTo(99);
    }

    [Test]
    public async Task TryParse_String_FailsOnBadInput()
    {
        var ok = SetItemTemplateId.TryParse("abc", CultureInfo.InvariantCulture, out var id);
        await Assert.That(ok).IsFalse();
        await Assert.That(id).IsEqualTo(default(SetItemTemplateId));
    }

    [Test]
    public async Task TryParse_NullString_ReturnsFalse()
    {
        var ok = SetItemTemplateId.TryParse(null, CultureInfo.InvariantCulture, out _);
        await Assert.That(ok).IsFalse();
    }

    // ── ISpanParsable<T> ──────────────────────────────────────────────────

    [Test]
    public async Task Parse_Span_RoundTrips()
    {
        var id = SetItemTemplateId.Parse("42".AsSpan(), CultureInfo.InvariantCulture);
        await Assert.That(id.Value).IsEqualTo(42);
    }

    [Test]
    public async Task TryParse_Span_Succeeds()
    {
        var ok = SetItemTemplateId.TryParse("99".AsSpan(), CultureInfo.InvariantCulture, out var id);
        await Assert.That(ok).IsTrue();
        await Assert.That(id.Value).IsEqualTo(99);
    }

    [Test]
    public async Task TryParse_Span_FailsOnBadInput()
    {
        var ok = SetItemTemplateId.TryParse("abc".AsSpan(), CultureInfo.InvariantCulture, out var id);
        await Assert.That(ok).IsFalse();
        await Assert.That(id).IsEqualTo(default(SetItemTemplateId));
    }

    // ── Record Equality ───────────────────────────────────────────────────

    [Test]
    public async Task Equality_SameValue_AreEqual()
    {
        var a = new SetItemTemplateId(5);
        var b = new SetItemTemplateId(5);
        await Assert.That(a).IsEqualTo(b);
        await Assert.That(a.GetHashCode()).IsEqualTo(b.GetHashCode());
    }

    [Test]
    public async Task Equality_DifferentValue_AreNotEqual()
    {
        var a = new SetItemTemplateId(5);
        var b = new SetItemTemplateId(6);
        await Assert.That(a).IsNotEqualTo(b);
    }

    // ── JSON Round-Trip (with converter) ──────────────────────────────────

    [Test]
    public async Task Json_RoundTrips_WithConverter()
    {
        var original = new SetItemTemplateId(42);
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<SetItemTemplateId>(json);

        await Assert.That(json).IsEqualTo("42");
        await Assert.That(deserialized).IsEqualTo(original);
    }

    // ── No JSON Converter (GenerateJsonConverter = false) ─────────────────

    [Test]
    public async Task Json_WithoutConverter_SerializesAsObject()
    {
        // CharacterId has GenerateJsonConverter = false, so it serializes as {"Value":42}
        var original = new CharacterId(42);
        var json = JsonSerializer.Serialize(original);
        await Assert.That(json).Contains("42");
    }
}
