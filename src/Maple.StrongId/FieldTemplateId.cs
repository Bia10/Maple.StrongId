using System.Collections.Frozen;

namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for a field/map template ID.
/// </summary>
[StrongIntId]
public readonly partial record struct FieldTemplateId(int Value)
{
    // ── Digit Decomposition ───────────────────────────────────────────────

    /// <summary>
    /// Continent index: fieldID / 100_000_000.
    /// 1=Victoria Island, 2=Ossyria, 5=Ellinforest, 6=NLC/Masteria,
    /// 7=Mushroom Kingdom/Resistance, 8=Zipangu, 9=Special/instanced.
    /// Used in WZ group name: "Map{Continent}".
    /// </summary>
    public int Continent => Value / 100_000_000;

    /// <summary>
    /// Region prefix: fieldID / 1_000_000. Used for Edelstein (region 390) and sub-regions.
    /// </summary>
    public int Region => Value / 1_000_000;

    // Pre-allocated continent directory names — avoids heap allocation for valid indices (0–9).
    private static readonly string[] s_wzGroupNames =
    [
        "Map0",
        "Map1",
        "Map2",
        "Map3",
        "Map4",
        "Map5",
        "Map6",
        "Map7",
        "Map8",
        "Map9",
    ];

    /// <summary>
    /// WZ group directory name used in Map.wz navigation.
    /// Returns a pre-allocated string for continent indices 0–9 (zero allocation).
    /// </summary>
    public string WzGroupName
    {
        get
        {
            int c = Continent;
            return (uint)c < (uint)s_wzGroupNames.Length ? s_wzGroupNames[c] : $"Map{c}";
        }
    }

    // ── Continent Predicates ──────────────────────────────────────────────

    /// <summary>Victoria Island maps (100_000_000 – 199_999_999).</summary>
    public bool IsVictoriaIsland => Continent == 1;

    /// <summary>Ossyria maps (200_000_000 – 299_999_999). Includes El Nath, Ludibrium, Leafre, etc.</summary>
    public bool IsOssyria => Continent == 2;

    /// <summary>Ellinforest maps (500_000_000 – 599_999_999).</summary>
    public bool IsEllinforest => Continent == 5;

    /// <summary>New Leaf City / Masteria maps (600_000_000 – 699_999_999). GMS continent.</summary>
    public bool IsNlcMasteria => Continent == 6;

    /// <summary>Mushroom Kingdom / Resistance area maps (700_000_000 – 799_999_999).</summary>
    public bool IsMushroomKingdom => Continent == 7;

    /// <summary>Singapore / Mu Lung Dojo area maps (300_000_000 – 399_999_999).</summary>
    public bool IsSingapore => Continent == 3;

    /// <summary>Zipangu / Ninja Castle maps (800_000_000 – 899_999_999).</summary>
    public bool IsZipangu => Continent == 8;

    // ── Special Map Ranges ────────────────────────────────────────────────

    /// <summary>
    /// True for special/instanced map IDs (900_000_000 – 999_999_999).
    /// Upgrade tomb items are blocked in these maps.
    /// </summary>
    public bool IsSpecialInstanced => Continent == 9;

    /// <summary>
    /// True for Edelstein continent maps (390_000_000 – 390_999_999).
    /// Upgrade tomb items are blocked in these maps.
    /// </summary>
    public bool IsEdelstein => Region == 390;

    /// <summary>
    /// True for the Edelstein sub-area inside the Ossyria ID space (200_090_000 – 200_090_999).
    /// Upgrade tomb items are blocked here too.
    /// </summary>
    public bool IsEdelsteinSubArea => Value / 1_000 == 200_090;

    /// <summary>
    /// True when upgrade tomb items are blocked by map ID arithmetic (ignoring field type checks).
    /// </summary>
    public bool IsUpgradeTombBlocked => IsSpecialInstanced || IsEdelstein || IsEdelsteinSubArea;

    /// <summary>
    /// True for Free Market channels (910_000_000 – 910_000_022, 23 rooms).
    /// </summary>
    public bool IsFreeMarket => (uint)(Value - 910_000_000) <= 22;

    // ── Town Predicate ────────────────────────────────────────────────────

    // The 30 hardcoded town IDs.
    // Controls pet "love" vs "angry" animation on user interaction.
    private static readonly FrozenSet<FieldTemplateId> s_townIds = FrozenSet.ToFrozenSet<FieldTemplateId>([
        new(100_000_000), // Henesys
        new(101_000_000), // Ellinia
        new(102_000_000), // Perion
        new(103_000_000), // Kerning City
        new(104_000_000), // Lith Harbor
        new(105_040_300), // Sleepywood Inn (non-round ID — NOT 105000000)
        new(120_000_000), // Nautilus Harbor
        new(200_000_000), // Orbis
        new(211_000_000), // El Nath
        new(220_000_000), // Ludibrium
        new(221_000_000), // Korean Folk Town
        new(222_000_000), // Aqua Road entrance
        new(230_000_000), // Minar Forest / Paper Mill
        new(240_000_000), // Leafre
        new(250_000_000), // Mu Lung
        new(251_000_000), // Herb Town
        new(260_000_000), // Ariant
        new(261_000_000), // Magatia
        new(500_000_000), // Ellinforest hub
        new(600_000_000), // New Leaf City (NLC)
        new(680_000_000), // Masteria / Crimsonwood
        new(701_000_000), // Mushroom Kingdom main
        new(701_000_200), // Mushroom Kingdom secondary
        new(702_000_000), // Continent 7 area 2
        new(702_100_000), // Continent 7 area 2-100
        new(740_000_000), // Edelstein region hub 40
        new(741_000_000), // Edelstein region hub 41
        new(742_000_000), // Edelstein region hub 42
        new(800_000_000), // Ninja Castle / Zipangu
        new(801_000_000), // Continent 8 area 1
    ]);

    /// <summary>
    /// True if this map is one of the 30 hardcoded town IDs.
    /// Controls pet animation: town → "love" animation, non-town → "angry" animation.
    /// ⚠ Edelstein maps (390_000_000+) are NOT included here — those use the WZ <c>m_bTown</c> property.
    /// ⚠ Sleepywood uses 105_040_300 (Sleepywood Inn), NOT 105_000_000.
    /// </summary>
    public bool IsTownId => s_townIds.Contains(this);

    // ── Well-Known Field ID Constants ─────────────────────────────────────

    /// <summary>Henesys (Victoria Island).</summary>
    public static readonly FieldTemplateId Henesys = new(100_000_000);

    /// <summary>Ellinia (Victoria Island).</summary>
    public static readonly FieldTemplateId Ellinia = new(101_000_000);

    /// <summary>Perion (Victoria Island).</summary>
    public static readonly FieldTemplateId Perion = new(102_000_000);

    /// <summary>Kerning City (Victoria Island).</summary>
    public static readonly FieldTemplateId KerningCity = new(103_000_000);

    /// <summary>Lith Harbor (Victoria Island).</summary>
    public static readonly FieldTemplateId LithHarbor = new(104_000_000);

    /// <summary>Sleepywood Inn — the hardcoded town ID; NOT 105_000_000.</summary>
    public static readonly FieldTemplateId SleepywoodInn = new(105_040_300);

    /// <summary>Nautilus Harbor (Resistance homeland).</summary>
    public static readonly FieldTemplateId NautilusHarbor = new(120_000_000);

    /// <summary>Orbis (Ossyria).</summary>
    public static readonly FieldTemplateId Orbis = new(200_000_000);

    /// <summary>El Nath (Ossyria).</summary>
    public static readonly FieldTemplateId ElNath = new(211_000_000);

    /// <summary>Ludibrium (Ossyria).</summary>
    public static readonly FieldTemplateId Ludibrium = new(220_000_000);

    /// <summary>Leafre (Ossyria).</summary>
    public static readonly FieldTemplateId Leafre = new(240_000_000);

    /// <summary>Mu Lung (Ossyria).</summary>
    public static readonly FieldTemplateId MuLung = new(250_000_000);

    /// <summary>Ariant (Nihal Desert).</summary>
    public static readonly FieldTemplateId Ariant = new(260_000_000);

    /// <summary>Magatia (Nihal Desert).</summary>
    public static readonly FieldTemplateId Magatia = new(261_000_000);

    /// <summary>New Leaf City (GMS continent 6).</summary>
    public static readonly FieldTemplateId NewLeafCity = new(600_000_000);

    /// <summary>
    /// Free Market channel IDs (910_000_000 – 910_000_022, 23 channels).
    /// </summary>
    public const int FreeMarketBase = 910_000_000;

    /// <summary>Number of Free Market channels (23).</summary>
    public const int FreeMarketChannelCount = 23;

    /// <summary>
    /// WZ sentinel value for the <c>forcedReturn</c> map property meaning "no forced return".
    /// Any other integer value in WZ is the actual return field ID, and <c>m_bForcedReturn</c> is set.
    /// </summary>
    public const int ForcedReturnSentinel = 999_999_999;
}
