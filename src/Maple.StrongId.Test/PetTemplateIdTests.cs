namespace Maple.StrongId.Test;

public sealed class PetTemplateIdTests
{
    // ── IsPetRange ────────────────────────────────────────────────────────

    [Test]
    [Arguments(5_000_000, true)]
    [Arguments(5_009_999, true)]
    [Arguments(5_010_000, false)]
    [Arguments(4_999_999, false)]
    public async Task IsPetRange_MatchesExpected(int id, bool expected)
    {
        await Assert.That(new PetTemplateId(id).IsPetRange).IsEqualTo(expected);
    }

    // ── PetSerial ─────────────────────────────────────────────────────────

    [Test]
    [Arguments(5_000_000, 0)]
    [Arguments(5_000_042, 42)]
    [Arguments(5_000_999, 999)]
    public async Task PetSerial_MatchesExpected(int id, int expected)
    {
        await Assert.That(new PetTemplateId(id).PetSerial).IsEqualTo(expected);
    }

    // ── IsEquipTypeCompatible ─────────────────────────────────────────────

    [Test]
    public async Task IsEquipTypeCompatible_TrueForTypeIndex180()
    {
        await Assert.That(PetTemplateId.IsEquipTypeCompatible(new ItemTemplateId(1_800_000))).IsTrue();
        await Assert.That(PetTemplateId.IsEquipTypeCompatible(new ItemTemplateId(1_809_999))).IsTrue();
    }

    [Test]
    public async Task IsEquipTypeCompatible_FalseForOtherTypeIndex()
    {
        await Assert.That(PetTemplateId.IsEquipTypeCompatible(new ItemTemplateId(1_000_000))).IsFalse();
    }

    // ── WzImageName ───────────────────────────────────────────────────────

    [Test]
    public async Task WzImageName_IsPaddedTo8Digits()
    {
#pragma warning disable CS0618 // Testing convenience API
        await Assert.That(new PetTemplateId(5_000_000).WzImageName).IsEqualTo("05000000.img");
        await Assert.That(new PetTemplateId(5_000_042).WzImageName).IsEqualTo("05000042.img");
#pragma warning restore CS0618
    }

    // ── TryWriteWzImageName ───────────────────────────────────────────────

    [Test]
    public async Task TryWriteWzImageName_WritesCorrectly()
    {
        var buf = new char[12];
        var ok = new PetTemplateId(5_000_001).TryWriteWzImageName(buf, out int written);
        var result = new string(buf, 0, written);

        await Assert.That(ok).IsTrue();
        await Assert.That(written).IsEqualTo(12);
        await Assert.That(result).IsEqualTo("05000001.img");
    }

    [Test]
    public async Task TryWriteWzImageName_FailsWhenBufferTooSmall()
    {
        var buf = new char[11];
        var ok = new PetTemplateId(5_000_000).TryWriteWzImageName(buf, out int written);

        await Assert.That(ok).IsFalse();
        await Assert.That(written).IsEqualTo(0);
    }
}
