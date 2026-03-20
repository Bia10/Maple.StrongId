namespace Maple.StrongId.Test;

public sealed class EmployeeTemplateIdTests
{
    // ── WzImageName ───────────────────────────────────────────────────────

    [Test]
    public async Task WzImageName_IsPaddedTo7Digits()
    {
#pragma warning disable CS0618 // Testing convenience API
        await Assert.That(new EmployeeTemplateId(1).WzImageName).IsEqualTo("0000001.img");
        await Assert.That(new EmployeeTemplateId(9_999_999).WzImageName).IsEqualTo("9999999.img");
#pragma warning restore CS0618
    }

    // ── TryWriteWzImageName ───────────────────────────────────────────────

    [Test]
    public async Task TryWriteWzImageName_WritesCorrectly()
    {
        var buf = new char[11];
        var id = new EmployeeTemplateId(42);
        var ok = id.TryWriteWzImageName(buf, out int written);
        var result = new string(buf, 0, written);

        await Assert.That(ok).IsTrue();
        await Assert.That(written).IsEqualTo(11);
        await Assert.That(result).IsEqualTo("0000042.img");
    }

    [Test]
    public async Task TryWriteWzImageName_FailsWhenBufferTooSmall()
    {
        var buf = new char[10];
        var ok = new EmployeeTemplateId(1).TryWriteWzImageName(buf, out int written);

        await Assert.That(ok).IsFalse();
        await Assert.That(written).IsEqualTo(0);
    }
}
