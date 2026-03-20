namespace Maple.StrongId.Test;

public sealed class CharacterIdTests
{
    // ── SummonIndex ───────────────────────────────────────────────────────

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 1)]
    [Arguments(0x3FFF_FFFF, 0x3FFF_FFFF)] // max natural → unchanged
    [Arguments(unchecked((int)0xC000_0001), 1)] // upper flags set → stripped to 1
    [Arguments(unchecked((int)0xFFFF_FFFF), 0x3FFF_FFFF)] // all bits → lower 30
    public async Task SummonIndex_MasksUpperTwoBits(int value, int expected)
    {
        await Assert.That(new CharacterId(value).SummonIndex).IsEqualTo(expected);
    }

    // ── MaxNaturalValue ───────────────────────────────────────────────────

    [Test]
    public async Task MaxNaturalValue_Equals2Power30Minus1()
    {
        await Assert.That(CharacterId.MaxNaturalValue).IsEqualTo(0x3FFF_FFFF);
    }
}
