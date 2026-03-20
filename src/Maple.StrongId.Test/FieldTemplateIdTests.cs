namespace Maple.StrongId.Test;

public sealed class FieldTemplateIdTests
{
    // ── Free Market ───────────────────────────────────────────────────────

    [Test]
    public async Task IsFreeMarket_MatchesRange()
    {
        await Assert.That(new FieldTemplateId(910_000_000).IsFreeMarket).IsTrue();
        await Assert.That(new FieldTemplateId(910_000_022).IsFreeMarket).IsTrue();
        await Assert.That(new FieldTemplateId(910_000_023).IsFreeMarket).IsFalse();
        await Assert.That(new FieldTemplateId(909_999_999).IsFreeMarket).IsFalse();
    }

    // ── IsEdelsteinSubArea ────────────────────────────────────────────────

    [Test]
    public async Task IsEdelsteinSubArea_MatchesExpectedRange()
    {
        await Assert.That(new FieldTemplateId(200_090_000).IsEdelsteinSubArea).IsTrue();
        await Assert.That(new FieldTemplateId(200_090_999).IsEdelsteinSubArea).IsTrue();
        await Assert.That(new FieldTemplateId(200_091_000).IsEdelsteinSubArea).IsFalse();
        await Assert.That(new FieldTemplateId(200_089_999).IsEdelsteinSubArea).IsFalse();
    }

    // ── IsUpgradeTombBlocked ──────────────────────────────────────────────

    [Test]
    public async Task IsUpgradeTombBlocked_CombinesThreeChecks()
    {
        await Assert.That(new FieldTemplateId(900_000_000).IsUpgradeTombBlocked).IsTrue(); // special instanced
        await Assert.That(new FieldTemplateId(390_000_000).IsUpgradeTombBlocked).IsTrue(); // Edelstein region
        await Assert.That(new FieldTemplateId(200_090_500).IsUpgradeTombBlocked).IsTrue(); // Edelstein sub-area
        await Assert.That(new FieldTemplateId(100_000_000).IsUpgradeTombBlocked).IsFalse(); // Henesys → not blocked
    }

    // ── Well-known constants ──────────────────────────────────────────────

    [Test]
    public async Task SleepywoodInn_IsNotRoundId()
    {
        // Critical: Sleepywood uses 105_040_300, NOT 105_000_000
        await Assert.That(FieldTemplateId.SleepywoodInn.Value).IsEqualTo(105_040_300);
        await Assert.That(new FieldTemplateId(105_000_000).IsTownId).IsFalse();
        await Assert.That(FieldTemplateId.SleepywoodInn.IsTownId).IsTrue();
    }

    // ── Digit Decomposition ───────────────────────────────────────────────

    [Test]
    [Arguments(100_000_000, 1, 100)] // Victoria Island
    [Arguments(200_000_000, 2, 200)] // Ossyria
    [Arguments(910_000_000, 9, 910)] // Special / Free Market
    [Arguments(390_000_000, 3, 390)] // Edelstein
    public async Task ContinentAndRegion_MatchExpected(int id, int continent, int region)
    {
        var field = new FieldTemplateId(id);
        await Assert.That(field.Continent).IsEqualTo(continent);
        await Assert.That(field.Region).IsEqualTo(region);
    }

    // ── WzGroupName ──────────────────────────────────────────────────────

    [Test]
    [Arguments(100_000_000, "Map1")]
    [Arguments(200_000_000, "Map2")]
    [Arguments(900_000_000, "Map9")]
    [Arguments(0, "Map0")]
    public async Task WzGroupName_MatchesExpected(int id, string expected)
    {
        await Assert.That(new FieldTemplateId(id).WzGroupName).IsEqualTo(expected);
    }

    // ── Continent Predicates ──────────────────────────────────────────────

    [Test]
    public async Task ContinentPredicates_MatchExpected()
    {
        await Assert.That(new FieldTemplateId(100_000_000).IsVictoriaIsland).IsTrue();
        await Assert.That(new FieldTemplateId(200_000_000).IsOssyria).IsTrue();
        await Assert.That(new FieldTemplateId(300_000_000).IsSingapore).IsTrue();
        await Assert.That(new FieldTemplateId(500_000_000).IsEllinforest).IsTrue();
        await Assert.That(new FieldTemplateId(600_000_000).IsNlcMasteria).IsTrue();
        await Assert.That(new FieldTemplateId(700_000_000).IsMushroomKingdom).IsTrue();
        await Assert.That(new FieldTemplateId(800_000_000).IsZipangu).IsTrue();
        await Assert.That(new FieldTemplateId(900_000_000).IsSpecialInstanced).IsTrue();

        // Cross-check negatives
        await Assert.That(new FieldTemplateId(200_000_000).IsVictoriaIsland).IsFalse();
        await Assert.That(new FieldTemplateId(100_000_000).IsOssyria).IsFalse();
    }

    // ── IsEdelstein ───────────────────────────────────────────────────────

    [Test]
    public async Task IsEdelstein_MatchesRegion390()
    {
        await Assert.That(new FieldTemplateId(390_000_000).IsEdelstein).IsTrue();
        await Assert.That(new FieldTemplateId(390_999_999).IsEdelstein).IsTrue();
        await Assert.That(new FieldTemplateId(391_000_000).IsEdelstein).IsFalse();
        await Assert.That(new FieldTemplateId(389_999_999).IsEdelstein).IsFalse();
    }

    // ── IsTownId ──────────────────────────────────────────────────────────

    [Test]
    public async Task IsTownId_AllHardcodedTowns_ReturnTrue()
    {
        // Spot-check several known town IDs
        await Assert.That(FieldTemplateId.Henesys.IsTownId).IsTrue();
        await Assert.That(FieldTemplateId.Ellinia.IsTownId).IsTrue();
        await Assert.That(FieldTemplateId.Perion.IsTownId).IsTrue();
        await Assert.That(FieldTemplateId.KerningCity.IsTownId).IsTrue();
        await Assert.That(FieldTemplateId.LithHarbor.IsTownId).IsTrue();
        await Assert.That(FieldTemplateId.NautilusHarbor.IsTownId).IsTrue();
        await Assert.That(FieldTemplateId.Orbis.IsTownId).IsTrue();
        await Assert.That(FieldTemplateId.ElNath.IsTownId).IsTrue();
        await Assert.That(FieldTemplateId.Ludibrium.IsTownId).IsTrue();
        await Assert.That(FieldTemplateId.Leafre.IsTownId).IsTrue();
        await Assert.That(FieldTemplateId.MuLung.IsTownId).IsTrue();
        await Assert.That(FieldTemplateId.Ariant.IsTownId).IsTrue();
        await Assert.That(FieldTemplateId.Magatia.IsTownId).IsTrue();
        await Assert.That(FieldTemplateId.NewLeafCity.IsTownId).IsTrue();
    }

    [Test]
    public async Task IsTownId_NonTown_ReturnsFalse()
    {
        await Assert.That(new FieldTemplateId(100_000_001).IsTownId).IsFalse();
        await Assert.That(new FieldTemplateId(999_999_999).IsTownId).IsFalse();
    }

    // ── Well-Known Constants ──────────────────────────────────────────────

    [Test]
    public async Task WellKnownConstants_HaveCorrectValues()
    {
        await Assert.That(FieldTemplateId.Henesys.Value).IsEqualTo(100_000_000);
        await Assert.That(FieldTemplateId.Ellinia.Value).IsEqualTo(101_000_000);
        await Assert.That(FieldTemplateId.Perion.Value).IsEqualTo(102_000_000);
        await Assert.That(FieldTemplateId.KerningCity.Value).IsEqualTo(103_000_000);
        await Assert.That(FieldTemplateId.LithHarbor.Value).IsEqualTo(104_000_000);
        await Assert.That(FieldTemplateId.NautilusHarbor.Value).IsEqualTo(120_000_000);
        await Assert.That(FieldTemplateId.Orbis.Value).IsEqualTo(200_000_000);
        await Assert.That(FieldTemplateId.ElNath.Value).IsEqualTo(211_000_000);
        await Assert.That(FieldTemplateId.Ludibrium.Value).IsEqualTo(220_000_000);
        await Assert.That(FieldTemplateId.Leafre.Value).IsEqualTo(240_000_000);
        await Assert.That(FieldTemplateId.MuLung.Value).IsEqualTo(250_000_000);
        await Assert.That(FieldTemplateId.Ariant.Value).IsEqualTo(260_000_000);
        await Assert.That(FieldTemplateId.Magatia.Value).IsEqualTo(261_000_000);
        await Assert.That(FieldTemplateId.NewLeafCity.Value).IsEqualTo(600_000_000);
    }

    [Test]
    public async Task FreeMarketConstants_HaveCorrectValues()
    {
        await Assert.That(FieldTemplateId.FreeMarketBase).IsEqualTo(910_000_000);
        await Assert.That(FieldTemplateId.FreeMarketChannelCount).IsEqualTo(23);
    }

    [Test]
    public async Task ForcedReturnSentinel_HasCorrectValue()
    {
        await Assert.That(FieldTemplateId.ForcedReturnSentinel).IsEqualTo(999_999_999);
    }
}
