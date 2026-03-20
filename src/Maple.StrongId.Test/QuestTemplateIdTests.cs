namespace Maple.StrongId.Test;

public sealed class QuestTemplateIdTests
{
    // ── IsPartyQuest ──────────────────────────────────────────────────────

    [Test]
    [Arguments(1200, true)] // first party quest ID
    [Arguments(1399, true)] // last party quest ID
    [Arguments(1199, false)] // just below range
    [Arguments(1400, false)] // just above range
    [Arguments(2000, false)] // expedition quest, not party quest
    public async Task IsPartyQuest_MatchesRange(int id, bool expected)
    {
        await Assert.That(new QuestTemplateId(id).IsPartyQuest).IsEqualTo(expected);
    }

    [Test]
    public async Task IsPartyQuestInfoId_IsBroaderThanIsPartyQuest()
    {
        // 1000–1999 satisfy IsPartyQuestInfoId
        await Assert.That(new QuestTemplateId(1000).IsPartyQuestInfoId).IsTrue();
        await Assert.That(new QuestTemplateId(1999).IsPartyQuestInfoId).IsTrue();
        // But only 1200–1399 satisfy IsPartyQuest
        await Assert.That(new QuestTemplateId(1000).IsPartyQuest).IsFalse();
        await Assert.That(new QuestTemplateId(1200).IsPartyQuest).IsTrue();
    }

    [Test]
    public async Task MaxQuestId_FitsInUInt16()
    {
        await Assert.That(QuestTemplateId.MaxQuestId).IsEqualTo(65_535);
        await Assert.That(QuestTemplateId.MaxQuestId <= ushort.MaxValue).IsTrue();
    }
}
