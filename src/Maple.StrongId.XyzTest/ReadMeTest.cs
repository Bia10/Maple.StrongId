#pragma warning disable CA2007 // ConfigureAwait
#pragma warning disable CA1822 // Mark as static

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using PublicApiGenerator;

namespace Maple.StrongId.XyzTest;

[NotInParallel]
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class ReadMeTest
{
    static readonly string s_testSourceFilePath = SourceFile();

    // Navigate from src/Maple.StrongId.XyzTest/ up to repo root (2 levels: XyzTest → src → root)
    static readonly string s_rootDirectory =
        Path.GetFullPath(Path.Combine(Path.GetDirectoryName(s_testSourceFilePath)!, "..", ".."))
        + Path.DirectorySeparatorChar;

    static readonly string s_readmeFilePath = s_rootDirectory + "README.md";

    // ─────────────────────────────────────────────────────────────
    // SECTION 1: README example code
    // ─────────────────────────────────────────────────────────────

    [Test]
    public void ReadMeTest_()
    {
        var item = new ItemTemplateId(1002140);
        bool isHat = item.IsHat;
        bool isWeapon = item.IsWeapon;
        _ = isHat;
        _ = isWeapon;
    }

    [Test]
    public void ReadMeTest_ItemTemplateId()
    {
        var item = new ItemTemplateId(1002140);
        bool isHat = item.IsHat; // true
        bool isWeapon = item.IsWeapon; // false
        _ = isHat;
        _ = isWeapon;
    }

    [Test]
    public void ReadMeTest_JobId()
    {
        var skill = new SkillTemplateId(1000000);
        bool isActive = skill.IsActive;
        int root = skill.Root; // 100 (Warrior category root)
        _ = isActive;
        _ = root;
    }

    [Test]
    public void ReadMeTest_FieldTemplateId()
    {
        var field = new FieldTemplateId(100000000);
        int continent = field.Continent; // 1 (Victoria Island)
        bool isTown = field.IsTownId;
        _ = continent;
        _ = isTown;
    }

    // ─────────────────────────────────────────────────────────────
    // SECTION 2: README sync tests — run only on net10.0
    // ─────────────────────────────────────────────────────────────

#if NET10_0
    [Test]
#endif
    public void ReadMeTest_UpdateExampleCodeInMarkdown()
    {
        if (!File.Exists(s_readmeFilePath))
        {
            return;
        } // CallerFilePath is a deterministic path on CI — skip
        var readmeLines = File.ReadAllLines(s_readmeFilePath);
        var testSourceLines = File.ReadAllLines(s_testSourceFilePath);

        var testBlocksToUpdate = new (string StartLineContains, string ReadmeLineBeforeCodeBlock)[]
        {
            (nameof(ReadMeTest_) + "()", "## Example"),
            (nameof(ReadMeTest_ItemTemplateId) + "()", "### Example - ItemTemplateId"),
            (nameof(ReadMeTest_JobId) + "()", "### Example - JobId"),
            (nameof(ReadMeTest_FieldTemplateId) + "()", "### Example - FieldTemplateId"),
        };

        readmeLines = UpdateReadme(
            testSourceLines,
            readmeLines,
            testBlocksToUpdate,
            sourceStartLineOffset: 2,
            "    }",
            sourceEndLineOffset: 0,
            sourceWhitespaceToRemove: 8
        );

        var newReadme = string.Join(Environment.NewLine, readmeLines) + Environment.NewLine;
        File.WriteAllText(s_readmeFilePath, newReadme, System.Text.Encoding.UTF8);
    }

#if NET10_0
    [Test]
#endif
    public void ReadMeTest_UpdateBenchmarksInMarkdown()
    {
        var readmeFilePath = s_readmeFilePath;
        var benchmarkFileNameToConfig = new Dictionary<
            string,
            (string Description, string ReadmeBefore, string ReadmeEnd, string SectionPrefix)
        >(StringComparer.Ordinal)
        {
            {
                "TestBench.md",
                ("TestBench Benchmark Results", "##### TestBench Benchmark Results", "## Example Catalogue", "######")
            },
        };

        var benchmarksDirectory = Path.Combine(s_rootDirectory, "benchmarks");
        if (!Directory.Exists(benchmarksDirectory))
        {
            return;
        } // not run yet

        var processorDirectories = Directory.EnumerateDirectories(benchmarksDirectory).ToArray();
        var readmeLines = File.ReadAllLines(readmeFilePath);

        foreach (var (fileName, config) in benchmarkFileNameToConfig)
        {
            var all = "";
            foreach (var processorDirectory in processorDirectories)
            {
                var contentsFilePath = Path.Combine(processorDirectory, fileName);
                if (!File.Exists(contentsFilePath))
                {
                    continue;
                }

                var versionsFilePath = Path.Combine(processorDirectory, "Versions.txt");
                var versions = File.ReadAllText(versionsFilePath);
                var contents = File.ReadAllText(contentsFilePath);
                var processor = processorDirectory
                    .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    .Last();
                var section = $"{config.SectionPrefix}{processor} - {config.Description} ({versions})";
                var benchmarkTable = contents.Substring(contents.IndexOf('|'));
                all += $"{section}{Environment.NewLine}{Environment.NewLine}{benchmarkTable}{Environment.NewLine}";
            }

            readmeLines = ReplaceReadmeLines(
                readmeLines,
                [all],
                config.ReadmeBefore,
                config.SectionPrefix,
                0,
                config.ReadmeEnd,
                0
            );
        }

        var newReadme = string.Join(Environment.NewLine, readmeLines) + Environment.NewLine;
        File.WriteAllText(s_readmeFilePath, newReadme, System.Text.Encoding.UTF8);
    }

#if NET10_0
    [Test]
#endif
    public void ReadMeTest_PublicApi()
    {
        if (!File.Exists(s_readmeFilePath))
        {
            return;
        } // CallerFilePath is a deterministic path on CI — skip
        var publicApi = typeof(ItemTemplateId).Assembly.GeneratePublicApi();
        var readmeLines = File.ReadAllLines(s_readmeFilePath);
        readmeLines = ReplaceReadmeLines(readmeLines, [publicApi], "## Public API Reference", "```csharp", 1, "```", 0);
        var newReadme = string.Join(Environment.NewLine, readmeLines) + Environment.NewLine;
        File.WriteAllText(s_readmeFilePath, newReadme, System.Text.Encoding.UTF8);
    }

    // ─────────────────────────────────────────────────────────────
    // INFRASTRUCTURE — do not modify
    // ─────────────────────────────────────────────────────────────

    static string[] UpdateReadme(
        string[] sourceLines,
        string[] readmeLines,
        (string StartLineContains, string ReadmeLineBefore)[] blocksToUpdate,
        int sourceStartLineOffset,
        string sourceEndLineStartsWith,
        int sourceEndLineOffset,
        int sourceWhitespaceToRemove,
        string readmeStartLineStartsWith = "```csharp",
        int readmeStartLineOffset = 1,
        string readmeEndLineStartsWith = "```",
        int readmeEndLineOffset = 0
    )
    {
        foreach (var (startLineContains, readmeLineBeforeBlock) in blocksToUpdate)
        {
            var sourceExampleLines = SnipLines(
                sourceLines,
                startLineContains,
                sourceStartLineOffset,
                sourceEndLineStartsWith,
                sourceEndLineOffset,
                sourceWhitespaceToRemove
            );
            readmeLines = ReplaceReadmeLines(
                readmeLines,
                sourceExampleLines,
                readmeLineBeforeBlock,
                readmeStartLineStartsWith,
                readmeStartLineOffset,
                readmeEndLineStartsWith,
                readmeEndLineOffset
            );
        }
        return readmeLines;
    }

    static string[] ReplaceReadmeLines(
        string[] readmeLines,
        string[] newLines,
        string readmeLineBeforeBlock,
        string readmeStartLineStartsWith,
        int readmeStartLineOffset,
        string readmeEndLineStartsWith,
        int readmeEndLineOffset
    )
    {
        var beforeIndex = Array.FindIndex(
            readmeLines,
            l => l.StartsWith(readmeLineBeforeBlock, StringComparison.Ordinal)
        );
        if (beforeIndex < 0)
        {
            throw new ArgumentException(
                $"README line '{readmeLineBeforeBlock}' not found.",
                nameof(readmeLineBeforeBlock)
            );
        }

        var replaceStart =
            Array.FindIndex(
                readmeLines,
                beforeIndex,
                l => l.StartsWith(readmeStartLineStartsWith, StringComparison.Ordinal)
            ) + readmeStartLineOffset;
        Debug.Assert(replaceStart >= 0);
        var replaceEnd =
            Array.FindIndex(
                readmeLines,
                replaceStart,
                l => l.StartsWith(readmeEndLineStartsWith, StringComparison.Ordinal)
            ) + readmeEndLineOffset;

        return readmeLines[..replaceStart].AsEnumerable().Concat(newLines).Concat(readmeLines[replaceEnd..]).ToArray();
    }

    static string[] SnipLines(
        string[] sourceLines,
        string startLineContains,
        int startLineOffset,
        string endLineStartsWith,
        int endLineOffset,
        int whitespaceToRemove = 8
    )
    {
        var start =
            Array.FindIndex(sourceLines, l => l.Contains(startLineContains, StringComparison.Ordinal))
            + startLineOffset;
        var end =
            Array.FindIndex(sourceLines, start, l => l.StartsWith(endLineStartsWith, StringComparison.Ordinal))
            + endLineOffset;
        return sourceLines[start..end]
            .Select(l => l.Length > whitespaceToRemove ? l.Remove(0, whitespaceToRemove) : l.TrimStart())
            .ToArray();
    }

    static string SourceFile([CallerFilePath] string sourceFilePath = "") => sourceFilePath;
}
