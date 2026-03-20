namespace Maple.StrongId.Test;

public sealed class MobTemplateIdTests
{
    // ── IsNotCapturable ───────────────────────────────────────────────────

    [Test]
    [Arguments(9_000_000, true)] // family 90 → boss → not capturable
    [Arguments(9_500_000, true)] // family 95 → boss → not capturable
    [Arguments(9_700_000, true)] // family 97 → boss → not capturable
    [Arguments(9_304_000, false)] // hardcoded exception → capturable despite boss range
    [Arguments(9_304_005, false)] // last exception ID → capturable
    [Arguments(9_304_006, true)] // just outside exception → not capturable
    [Arguments(1_000_000, false)] // normal mob → capturable
    public async Task IsNotCapturable_MatchesExpected(int id, bool expected)
    {
        await Assert.That(new MobTemplateId(id).IsNotCapturable).IsEqualTo(expected);
    }

    [Test]
    public async Task IsNotSwallowable_HasNoExceptionForCapturable()
    {
        // 9_304_000-9_304_005 are capturable but NOT swallowable
        for (int id = 9_304_000; id <= 9_304_005; id++)
            await Assert.That(new MobTemplateId(id).IsNotSwallowable).IsTrue();
    }

    // ── IsLevelVisible ────────────────────────────────────────────────────

    [Test]
    [Arguments(1_000_000, true)] // normal mob → visible
    [Arguments(9_400_000, true)] // family 94 in 9M range → visible
    [Arguments(9_600_000, true)] // family 96 in 9M range → visible
    [Arguments(9_000_000, false)] // family 90 in 9M range → hidden
    [Arguments(9_500_000, false)] // family 95 in 9M range → hidden
    public async Task IsLevelVisible_MatchesExpected(int id, bool expected)
    {
        await Assert.That(new MobTemplateId(id).IsLevelVisible).IsEqualTo(expected);
    }

    // ── Pet / Taming ──────────────────────────────────────────────────────

    [Test]
    public async Task IsPetTemplate_MatchesSeries500()
    {
        await Assert.That(new MobTemplateId(5_000_000).IsPetTemplate).IsTrue();
        await Assert.That(new MobTemplateId(5_009_999).IsPetTemplate).IsTrue();
        await Assert.That(new MobTemplateId(5_010_000).IsPetTemplate).IsFalse();
    }

    [Test]
    public async Task IsTamingMob_MatchesSeries190()
    {
        await Assert.That(new MobTemplateId(1_900_000).IsTamingMob).IsTrue();
        await Assert.That(new MobTemplateId(1_909_999).IsTamingMob).IsTrue();
        await Assert.That(new MobTemplateId(1_910_000).IsTamingMob).IsFalse();
    }

    [Test]
    public async Task WzImageName_IsPaddedTo7Digits()
    {
#pragma warning disable CS0618 // Testing convenience API
        await Assert.That(new MobTemplateId(100_100).WzImageName).IsEqualTo("0100100.img");
        await Assert.That(new MobTemplateId(9_999_999).WzImageName).IsEqualTo("9999999.img");
#pragma warning restore CS0618
    }

    // ── Family / Series ───────────────────────────────────────────────────

    [Test]
    [Arguments(9_000_000, 90, 900)]
    [Arguments(5_000_000, 50, 500)]
    [Arguments(1_900_000, 19, 190)]
    [Arguments(8_820_000, 88, 882)]
    public async Task FamilyAndSeries_MatchExpected(int id, int family, int series)
    {
        var mob = new MobTemplateId(id);
        await Assert.That(mob.Family).IsEqualTo(family);
        await Assert.That(mob.Series).IsEqualTo(series);
    }

    // ── IsJaguarBuffImmune ────────────────────────────────────────────────

    [Test]
    public async Task IsJaguarBuffImmune_MatchesSeries882()
    {
        await Assert.That(new MobTemplateId(8_820_000).IsJaguarBuffImmune).IsTrue();
        await Assert.That(new MobTemplateId(8_829_999).IsJaguarBuffImmune).IsTrue();
        await Assert.That(new MobTemplateId(8_830_000).IsJaguarBuffImmune).IsFalse();
        await Assert.That(new MobTemplateId(8_810_000).IsJaguarBuffImmune).IsFalse();
    }

    // ── PetSerial ─────────────────────────────────────────────────────────

    [Test]
    public async Task PetSerial_ReturnsLast3Digits()
    {
        await Assert.That(new MobTemplateId(5_000_000).PetSerial).IsEqualTo(0);
        await Assert.That(new MobTemplateId(5_000_123).PetSerial).IsEqualTo(123);
        await Assert.That(new MobTemplateId(5_000_999).PetSerial).IsEqualTo(999);
    }

    // ── TryWriteWzImageName ───────────────────────────────────────────────

    [Test]
    public async Task TryWriteWzImageName_WritesCorrectly()
    {
        var buf = new char[11];
        var ok = new MobTemplateId(100_100).TryWriteWzImageName(buf, out int written);
        var result = new string(buf, 0, written);
        await Assert.That(ok).IsTrue();
        await Assert.That(written).IsEqualTo(11);
        await Assert.That(result).IsEqualTo("0100100.img");
    }

    [Test]
    public async Task TryWriteWzImageName_FailsWhenBufferTooSmall()
    {
        var buf = new char[10];
        var ok = new MobTemplateId(100_100).TryWriteWzImageName(buf, out int written);
        await Assert.That(ok).IsFalse();
        await Assert.That(written).IsEqualTo(0);
    }
}
