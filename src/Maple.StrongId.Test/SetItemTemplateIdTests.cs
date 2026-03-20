namespace Maple.StrongId.Test;

public sealed class SetItemTemplateIdTests
{
    // ── None Sentinel ─────────────────────────────────────────────────────

    [Test]
    public async Task None_HasValueZero()
    {
        await Assert.That(SetItemTemplateId.None.Value).IsEqualTo(0);
    }

    // ── IsNoSet ───────────────────────────────────────────────────────────

    [Test]
    [Arguments(0, true)]
    [Arguments(1, false)]
    [Arguments(42, false)]
    public async Task IsNoSet_MatchesExpected(int id, bool expected)
    {
        await Assert.That(new SetItemTemplateId(id).IsNoSet).IsEqualTo(expected);
    }

    // ── MaxPartsCount ─────────────────────────────────────────────────────

    [Test]
    public async Task MaxPartsCount_Is60()
    {
        await Assert.That(SetItemTemplateId.MaxPartsCount).IsEqualTo(60);
    }
}
