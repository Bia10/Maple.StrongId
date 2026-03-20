namespace Maple.StrongId.Test;

public sealed class TamingMobTemplateIdTests
{
    // ── IsTamingMobRange ──────────────────────────────────────────────────

    [Test]
    [Arguments(1_900_000, true)]
    [Arguments(1_909_999, true)]
    [Arguments(1_910_000, false)]
    [Arguments(1_899_999, false)]
    public async Task IsTamingMobRange_MatchesExpected(int id, bool expected)
    {
        await Assert.That(new TamingMobTemplateId(id).IsTamingMobRange).IsEqualTo(expected);
    }

    // ── IsRedDraco ────────────────────────────────────────────────────────

    [Test]
    [Arguments(1_902_002, true)]
    [Arguments(1_902_001, false)]
    [Arguments(1_902_003, false)]
    public async Task IsRedDraco_MatchesExpected(int id, bool expected)
    {
        await Assert.That(new TamingMobTemplateId(id).IsRedDraco).IsEqualTo(expected);
    }

    // ── IsRyuho ───────────────────────────────────────────────────────────

    [Test]
    [Arguments(1_902_015, true)]
    [Arguments(1_902_016, true)]
    [Arguments(1_902_017, true)]
    [Arguments(1_902_018, true)]
    [Arguments(1_902_014, false)]
    [Arguments(1_902_019, false)]
    public async Task IsRyuho_MatchesExpected(int id, bool expected)
    {
        await Assert.That(new TamingMobTemplateId(id).IsRyuho).IsEqualTo(expected);
    }

    // ── IsEvanDragon ──────────────────────────────────────────────────────

    [Test]
    [Arguments(1_902_040, true)]
    [Arguments(1_902_041, true)]
    [Arguments(1_902_042, true)]
    [Arguments(1_902_039, false)]
    [Arguments(1_902_043, false)]
    public async Task IsEvanDragon_MatchesExpected(int id, bool expected)
    {
        var tmId = new TamingMobTemplateId(id);
        await Assert.That(tmId.IsEvanDragon).IsEqualTo(expected);
    }

    // ── Well-Known Constants ──────────────────────────────────────────────

    [Test]
    public async Task WellKnownConstants_HaveCorrectValues()
    {
        await Assert.That(TamingMobTemplateId.RedDraco.Value).IsEqualTo(1_902_002);
        await Assert.That(TamingMobTemplateId.Ryuho50.Value).IsEqualTo(1_902_015);
        await Assert.That(TamingMobTemplateId.Ryuho100.Value).IsEqualTo(1_902_016);
        await Assert.That(TamingMobTemplateId.Ryuho150.Value).IsEqualTo(1_902_017);
        await Assert.That(TamingMobTemplateId.Ryuho200.Value).IsEqualTo(1_902_018);
        await Assert.That(TamingMobTemplateId.Mir1.Value).IsEqualTo(1_902_040);
        await Assert.That(TamingMobTemplateId.Mir2.Value).IsEqualTo(1_902_041);
        await Assert.That(TamingMobTemplateId.Mir3.Value).IsEqualTo(1_902_042);
    }

    // ── WzImageName ───────────────────────────────────────────────────────

    [Test]
    public async Task WzImageName_IsPaddedTo7Digits()
    {
#pragma warning disable CS0618 // Testing convenience API
        await Assert.That(new TamingMobTemplateId(1_902_000).WzImageName).IsEqualTo("1902000.img");
        await Assert.That(new TamingMobTemplateId(1_900_001).WzImageName).IsEqualTo("1900001.img");
#pragma warning restore CS0618
    }

    // ── TryWriteWzImageName ───────────────────────────────────────────────

    [Test]
    public async Task TryWriteWzImageName_WritesCorrectly()
    {
        var buf = new char[11];
        var ok = new TamingMobTemplateId(1_902_000).TryWriteWzImageName(buf, out int written);
        var result = new string(buf, 0, written);

        await Assert.That(ok).IsTrue();
        await Assert.That(written).IsEqualTo(11);
        await Assert.That(result).IsEqualTo("1902000.img");
    }

    [Test]
    public async Task TryWriteWzImageName_FailsWhenBufferTooSmall()
    {
        var buf = new char[10];
        var ok = new TamingMobTemplateId(1).TryWriteWzImageName(buf, out int written);

        await Assert.That(ok).IsFalse();
        await Assert.That(written).IsEqualTo(0);
    }
}
