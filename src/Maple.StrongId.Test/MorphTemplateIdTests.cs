namespace Maple.StrongId.Test;

public sealed class MorphTemplateIdTests
{
    // ── IsSuperMan ────────────────────────────────────────────────────────

    [Test]
    [Arguments(1000, true)]
    [Arguments(1001, true)]
    [Arguments(1100, true)]
    [Arguments(1101, true)]
    [Arguments(1002, false)]
    [Arguments(999, false)]
    [Arguments(1102, false)]
    public async Task IsSuperMan_MatchesExpected(int id, bool expected)
    {
        await Assert.That(new MorphTemplateId(id).IsSuperMan).IsEqualTo(expected);
    }

    // ── IsInstantTransition ───────────────────────────────────────────────

    [Test]
    [Arguments(1002, true)]
    [Arguments(1000, false)]
    [Arguments(1003, false)]
    public async Task IsInstantTransition_MatchesExpected(int id, bool expected)
    {
        await Assert.That(new MorphTemplateId(id).IsInstantTransition).IsEqualTo(expected);
    }

    // ── Well-Known IDs ────────────────────────────────────────────────────

    [Test]
    public async Task SuperManIds_ContainsExactly4Ids()
    {
        await Assert.That(MorphTemplateId.SuperManIds.Count).IsEqualTo(4);
        await Assert.That(MorphTemplateId.SuperManIds.Contains(new MorphTemplateId(1000))).IsTrue();
        await Assert.That(MorphTemplateId.SuperManIds.Contains(new MorphTemplateId(1001))).IsTrue();
        await Assert.That(MorphTemplateId.SuperManIds.Contains(new MorphTemplateId(1100))).IsTrue();
        await Assert.That(MorphTemplateId.SuperManIds.Contains(new MorphTemplateId(1101))).IsTrue();
    }

    [Test]
    public async Task InstantTransitionId_Is1002()
    {
        await Assert.That(MorphTemplateId.InstantTransitionId).IsEqualTo(new MorphTemplateId(1002));
    }
}
