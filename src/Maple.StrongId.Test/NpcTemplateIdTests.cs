namespace Maple.StrongId.Test;

public sealed class NpcTemplateIdTests
{
    // ── WzImageName ───────────────────────────────────────────────────────

    [Test]
    public async Task WzImageName_IsPaddedTo7Digits()
    {
#pragma warning disable CS0618 // Testing convenience API
        await Assert.That(new NpcTemplateId(9_010_000).WzImageName).IsEqualTo("9010000.img");
        await Assert.That(new NpcTemplateId(100).WzImageName).IsEqualTo("0000100.img");
#pragma warning restore CS0618
    }

    // ── TryWriteWzImageName ───────────────────────────────────────────────

    [Test]
    public async Task TryWriteWzImageName_WritesCorrectly()
    {
        var buf = new char[11];
        var ok = new NpcTemplateId(9_010_000).TryWriteWzImageName(buf, out int written);
        var result = new string(buf, 0, written);

        await Assert.That(ok).IsTrue();
        await Assert.That(written).IsEqualTo(11);
        await Assert.That(result).IsEqualTo("9010000.img");
    }

    [Test]
    public async Task TryWriteWzImageName_FailsWhenBufferTooSmall()
    {
        var buf = new char[10];
        var ok = new NpcTemplateId(1).TryWriteWzImageName(buf, out int written);

        await Assert.That(ok).IsFalse();
        await Assert.That(written).IsEqualTo(0);
    }

    // ── Well-Known Constants ──────────────────────────────────────────────

    [Test]
    public async Task WellKnownConstants_HaveCorrectValues()
    {
        await Assert.That(NpcTemplateId.EvanDragon.Value).IsEqualTo(1_013_000);
        await Assert.That(NpcTemplateId.NoblesseTutor.Value).IsEqualTo(1_101_008);
        await Assert.That(NpcTemplateId.AranTutor.Value).IsEqualTo(1_202_000);
        await Assert.That(NpcTemplateId.Kin.Value).IsEqualTo(9_900_000);
        await Assert.That(NpcTemplateId.Nimakin.Value).IsEqualTo(9_900_001);
    }
}
