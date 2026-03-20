namespace Maple.StrongId.Test;

public sealed class ReactorTemplateIdTests
{
    // ── WzImageName ───────────────────────────────────────────────────────

    [Test]
    public async Task WzImageName_IsPaddedTo7Digits()
    {
#pragma warning disable CS0618 // Testing convenience API
        await Assert.That(new ReactorTemplateId(100_100).WzImageName).IsEqualTo("0100100.img");
        await Assert.That(new ReactorTemplateId(9_999_999).WzImageName).IsEqualTo("9999999.img");
#pragma warning restore CS0618
    }

    // ── TryWriteWzImageName ───────────────────────────────────────────────

    [Test]
    public async Task TryWriteWzImageName_WritesCorrectly()
    {
        var buf = new char[11];
        var ok = new ReactorTemplateId(100_100).TryWriteWzImageName(buf, out int written);
        var result = new string(buf, 0, written);

        await Assert.That(ok).IsTrue();
        await Assert.That(written).IsEqualTo(11);
        await Assert.That(result).IsEqualTo("0100100.img");
    }

    [Test]
    public async Task TryWriteWzImageName_FailsWhenBufferTooSmall()
    {
        var buf = new char[10];
        var ok = new ReactorTemplateId(1).TryWriteWzImageName(buf, out int written);

        await Assert.That(ok).IsFalse();
        await Assert.That(written).IsEqualTo(0);
    }
}
