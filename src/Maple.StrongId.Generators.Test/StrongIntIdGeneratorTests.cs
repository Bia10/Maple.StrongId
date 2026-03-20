using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Maple.StrongId.Generators.Test;

/// <summary>
/// Unit tests for <see cref="StrongIntIdGenerator"/> using <see cref="CSharpGeneratorDriver"/>.
/// Covers diagnostic emission (STRONGID001–003), generated source content, and output compilation correctness.
/// </summary>
public sealed class StrongIntIdGeneratorTests
{
    // ── Infrastructure ────────────────────────────────────────────────────

    // Built once per test session. BuildReferences() walks TRUSTED_PLATFORM_ASSEMBLIES
    // (100+ File.Exists checks) — calling it per-test would be wasteful I/O.
    private static readonly IReadOnlyList<MetadataReference> s_references = BuildReferences();

    /// <summary>
    /// Runs <see cref="StrongIntIdGenerator"/> against <paramref name="source"/> and returns
    /// the driver run result alongside the fully updated output compilation.
    /// </summary>
    private static (GeneratorDriverRunResult Run, Compilation Output) Execute(string source)
    {
        var parseOptions = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest);

        var compilation = CSharpCompilation.Create(
            assemblyName: "GeneratorTestAssembly",
            syntaxTrees: [CSharpSyntaxTree.ParseText(source, parseOptions)],
            references: s_references,
            options: new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                nullableContextOptions: NullableContextOptions.Enable
            )
        );

        GeneratorDriver driver = CSharpGeneratorDriver.Create(new StrongIntIdGenerator());
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out _);
        return (driver.GetRunResult(), output);
    }

    /// <summary>
    /// Builds the reference set for test compilations using <c>TRUSTED_PLATFORM_ASSEMBLIES</c>.
    /// This is the runtime's own resolved list of platform assemblies (BCL + framework), already
    /// deduplicated and pointing to the correct implementation/reference DLLs. Loading the whole
    /// runtime directory instead causes duplicate-type conflicts between <c>System.Runtime.dll</c>
    /// and <c>System.Private.CoreLib.dll</c>.
    /// </summary>
    private static IReadOnlyList<MetadataReference> BuildReferences()
    {
        // TRUSTED_PLATFORM_ASSEMBLIES: semicolon-separated list of all trusted assemblies
        // set by the .NET host. Includes all BCL, STJ, numerics, etc. — correctly deduplicated.
        var tpa = (AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string) ?? string.Empty;
        var refs = tpa.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Where(File.Exists)
            .Select(p => (MetadataReference)MetadataReference.CreateFromFile(p))
            .ToList();

        // Maple.StrongId is a project reference and may not be in TPA — add it explicitly.
        string strongIdPath = typeof(StrongIntIdAttribute).Assembly.Location;
        if (!string.IsNullOrEmpty(strongIdPath) && File.Exists(strongIdPath))
            refs.Add(MetadataReference.CreateFromFile(strongIdPath));

        return refs;
    }

    // ── STRONGID001: not a readonly partial record struct ─────────────────

    [Test]
    [Arguments("[StrongIntId] public partial class TestId { public int Value { get; init; } }")]
    [Arguments("[StrongIntId] public readonly partial struct TestId { public int Value { get; init; } }")]
    [Arguments("[StrongIntId] public partial record struct TestId(int Value) { }")] // not readonly
    [Arguments("[StrongIntId] public readonly record struct TestId(int Value) { }")] // not partial
    public async Task STRONGID001_EmittedForNonReadonlyPartialRecordStruct(string declaration)
    {
        var (run, _) = Execute($"using Maple.StrongId;\n{declaration}");

        await Assert
            .That(run.Diagnostics.Any(d => string.Equals(d.Id, "STRONGID001", StringComparison.Ordinal)))
            .IsTrue();
        await Assert.That(run.GeneratedTrees.IsEmpty).IsTrue();
    }

    // ── STRONGID002: nested types ─────────────────────────────────────────

    [Test]
    public async Task STRONGID002_EmittedForNestedType()
    {
        const string source = """
            using Maple.StrongId;
            public class Outer
            {
                [StrongIntId]
                public readonly partial record struct TestId(int Value) { }
            }
            """;

        var (run, _) = Execute(source);

        await Assert
            .That(run.Diagnostics.Any(d => string.Equals(d.Id, "STRONGID002", StringComparison.Ordinal)))
            .IsTrue();
        await Assert.That(run.GeneratedTrees.IsEmpty).IsTrue();
    }

    // ── STRONGID003: missing int Value ────────────────────────────────────

    [Test]
    [Arguments("[StrongIntId] public readonly partial record struct TestId(string Name) { }")]
    [Arguments("[StrongIntId] public readonly partial record struct TestId(int Id) { }")]
    [Arguments("[StrongIntId] public readonly partial record struct TestId { }")]
    public async Task STRONGID003_EmittedForMissingIntValue(string declaration)
    {
        var (run, _) = Execute($"using Maple.StrongId;\n{declaration}");

        await Assert
            .That(run.Diagnostics.Any(d => string.Equals(d.Id, "STRONGID003", StringComparison.Ordinal)))
            .IsTrue();
        await Assert.That(run.GeneratedTrees.IsEmpty).IsTrue();
    }

    // ── Valid type: diagnostics and file count ────────────────────────────

    [Test]
    public async Task ValidType_ProducesNoDiagnostics()
    {
        const string source = """
            using Maple.StrongId;
            namespace TestNs;
            [StrongIntId]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (run, output) = Execute(source);

        await Assert.That(run.Diagnostics.IsEmpty).IsTrue();
        await Assert.That(output.GetDiagnostics().Any(d => d.Severity == DiagnosticSeverity.Error)).IsFalse();
    }

    [Test]
    public async Task ValidType_GeneratesExactlyOneFile()
    {
        const string source = """
            using Maple.StrongId;
            namespace TestNs;
            [StrongIntId]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (run, _) = Execute(source);

        await Assert.That(run.Results[0].GeneratedSources.Length).IsEqualTo(1);
        await Assert.That(run.Results[0].GeneratedSources[0].HintName).IsEqualTo("TestId.g.cs");
    }

    [Test]
    public async Task MultipleTypes_EachGetsOwnFile()
    {
        const string source = """
            using Maple.StrongId;
            namespace TestNs;
            [StrongIntId] public readonly partial record struct FooId(int Value) { }
            [StrongIntId] public readonly partial record struct BarId(int Value) { }
            """;

        var (run, _) = Execute(source);

        await Assert.That(run.GeneratedTrees.Length).IsEqualTo(2);
    }

    // ── Valid type: interface declarations ────────────────────────────────

    [Test]
    public async Task ValidType_GeneratesExpectedInterfaces()
    {
        const string source = """
            using Maple.StrongId;
            namespace TestNs;
            [StrongIntId]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (run, _) = Execute(source);
        var text = run.Results[0].GeneratedSources[0].SourceText.ToString();

        await Assert.That(text.Contains("System.IComparable<TestId>")).IsTrue();
        await Assert.That(text.Contains("System.ISpanFormattable")).IsTrue();
        await Assert.That(text.Contains("System.IUtf8SpanFormattable")).IsTrue();
        await Assert.That(text.Contains("System.IParsable<TestId>")).IsTrue();
        await Assert.That(text.Contains("System.ISpanParsable<TestId>")).IsTrue();
        await Assert.That(text.Contains("System.Numerics.IComparisonOperators<TestId, TestId, bool>")).IsTrue();
    }

    [Test]
    public async Task ValidType_GeneratesComparisonOperators()
    {
        const string source = """
            using Maple.StrongId;
            namespace TestNs;
            [StrongIntId]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (run, _) = Execute(source);
        var text = run.Results[0].GeneratedSources[0].SourceText.ToString();

        await Assert.That(text.Contains("operator <(TestId")).IsTrue();
        await Assert.That(text.Contains("operator >(TestId")).IsTrue();
        await Assert.That(text.Contains("operator <=(TestId")).IsTrue();
        await Assert.That(text.Contains("operator >=(TestId")).IsTrue();
    }

    [Test]
    public async Task ValidType_DoesNotContainExcludeFromCodeCoverage()
    {
        const string source = """
            using Maple.StrongId;
            namespace TestNs;
            [StrongIntId]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (run, _) = Execute(source);
        var text = run.Results[0].GeneratedSources[0].SourceText.ToString();

        await Assert.That(text.Contains("ExcludeFromCodeCoverage")).IsFalse();
    }

    [Test]
    public async Task ValidType_ContainsDebuggerDisplay()
    {
        const string source = """
            using Maple.StrongId;
            namespace TestNs;
            [StrongIntId]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (run, _) = Execute(source);
        var text = run.Results[0].GeneratedSources[0].SourceText.ToString();

        await Assert.That(text.Contains("DebuggerDisplay")).IsTrue();
    }

    // ── Valid type: namespace handling ────────────────────────────────────

    [Test]
    public async Task ValidType_EmitsCorrectNamespace()
    {
        const string source = """
            using Maple.StrongId;
            namespace My.Domain.Ids;
            [StrongIntId]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (run, _) = Execute(source);
        var text = run.Results[0].GeneratedSources[0].SourceText.ToString();

        await Assert.That(text.Contains("namespace My.Domain.Ids;")).IsTrue();
    }

    [Test]
    public async Task ValidType_GlobalNamespace_EmitsNoNamespaceDecl()
    {
        const string source = """
            using Maple.StrongId;
            [StrongIntId]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (run, _) = Execute(source);
        var text = run.Results[0].GeneratedSources[0].SourceText.ToString();

        await Assert.That(text.Contains("namespace ")).IsFalse();
    }

    // ── JSON converter ────────────────────────────────────────────────────

    [Test]
    public async Task GenerateJsonConverter_Default_IncludesConverter()
    {
        const string source = """
            using Maple.StrongId;
            namespace TestNs;
            [StrongIntId]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (run, _) = Execute(source);
        var text = run.Results[0].GeneratedSources[0].SourceText.ToString();

        await Assert.That(text.Contains("StrongIdJsonConverter")).IsTrue();
        await Assert.That(text.Contains("JsonConverter<TestId>")).IsTrue();
        await Assert.That(text.Contains("[JsonConverter(typeof(TestId.StrongIdJsonConverter))]")).IsTrue();
    }

    [Test]
    public async Task GenerateJsonConverter_ExplicitTrue_IncludesConverter()
    {
        const string source = """
            using Maple.StrongId;
            namespace TestNs;
            [StrongIntId(GenerateJsonConverter = true)]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (run, _) = Execute(source);
        var text = run.Results[0].GeneratedSources[0].SourceText.ToString();

        await Assert.That(text.Contains("StrongIdJsonConverter")).IsTrue();
    }

    [Test]
    public async Task GenerateJsonConverter_False_ExcludesConverter()
    {
        const string source = """
            using Maple.StrongId;
            namespace TestNs;
            [StrongIntId(GenerateJsonConverter = false)]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (run, _) = Execute(source);
        var text = run.Results[0].GeneratedSources[0].SourceText.ToString();

        await Assert.That(text.Contains("StrongIdJsonConverter")).IsFalse();
        await Assert.That(text.Contains("JsonConverter")).IsFalse();
    }

    // ── Output compilation correctness ────────────────────────────────────

    [Test]
    public async Task OutputCompilation_NoErrors_ValidType()
    {
        const string source = """
            using Maple.StrongId;
            namespace TestNs;
            [StrongIntId]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (_, output) = Execute(source);
        var errors = output.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToList();

        await Assert.That(errors.Count).IsEqualTo(0);
    }

    [Test]
    public async Task OutputCompilation_NoErrors_WithJsonConverter()
    {
        const string source = """
            using Maple.StrongId;
            namespace TestNs;
            [StrongIntId(GenerateJsonConverter = true)]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (_, output) = Execute(source);
        var errors = output.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToList();

        await Assert.That(errors.Count).IsEqualTo(0);
    }

    [Test]
    public async Task OutputCompilation_NoErrors_WithoutJsonConverter()
    {
        const string source = """
            using Maple.StrongId;
            namespace TestNs;
            [StrongIntId(GenerateJsonConverter = false)]
            public readonly partial record struct TestId(int Value) { }
            """;

        var (_, output) = Execute(source);
        var errors = output.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToList();

        await Assert.That(errors.Count).IsEqualTo(0);
    }
}
