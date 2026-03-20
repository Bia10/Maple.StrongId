using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Maple.StrongId.Generators;

[Generator]
public sealed class StrongIntIdGenerator : IIncrementalGenerator
{
    private const string AttributeFullName = "Maple.StrongId.StrongIntIdAttribute";
    private const string GenerateConverterArgName = "GenerateJsonConverter";

    private static readonly DiagnosticDescriptor s_notARecordStruct = new(
        id: "STRONGID001",
        title: "[StrongIntId] requires a readonly partial record struct",
        messageFormat: "'{0}' must be declared as a readonly partial record struct to use [StrongIntId]",
        category: "StrongId",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor s_nestedTypeNotSupported = new(
        id: "STRONGID002",
        title: "[StrongIntId] does not support nested types",
        messageFormat: "'{0}' is a nested type; [StrongIntId] can only be applied to top-level or namespace-scoped types",
        category: "StrongId",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor s_missingValueProperty = new(
        id: "STRONGID003",
        title: "[StrongIntId] requires an int Value property",
        messageFormat: "'{0}' must declare a primary constructor parameter or property named 'Value' of type int",
        category: "StrongId",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    // Classification tag used to branch a single ForAttributeWithMetadataName scan
    // into diagnostic outputs and code generation.
    private enum TargetKind : byte
    {
        NotRecordStruct,
        NestedType,
        MissingValue,
        Valid,
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Single syntax scan — classify each attributed type, then branch into
        // diagnostic outputs and code generation. Previous implementation ran four
        // separate ForAttributeWithMetadataName passes over the same attribute.
        IncrementalValuesProvider<(TargetKind Kind, Location Location, string Name, StrongIdModel? Model)> classified =
            context.SyntaxProvider.ForAttributeWithMetadataName(
                AttributeFullName,
                predicate: static (_, _) => true,
                transform: static (ctx, ct) => Classify(ctx, ct)
            );

        // ── STRONGID001: not a readonly partial record struct ──────────────
        context.RegisterSourceOutput(
            classified
                .Where(static r => r.Kind == TargetKind.NotRecordStruct)
                .Select(static (r, _) => (r.Location, r.Name)),
            static (spc, data) => spc.ReportDiagnostic(Diagnostic.Create(s_notARecordStruct, data.Location, data.Name))
        );

        // ── STRONGID002: nested types not supported ───────────────────────
        context.RegisterSourceOutput(
            classified.Where(static r => r.Kind == TargetKind.NestedType).Select(static (r, _) => (r.Location, r.Name)),
            static (spc, data) =>
                spc.ReportDiagnostic(Diagnostic.Create(s_nestedTypeNotSupported, data.Location, data.Name))
        );

        // ── STRONGID003: missing int Value property ───────────────────────
        context.RegisterSourceOutput(
            classified
                .Where(static r => r.Kind == TargetKind.MissingValue)
                .Select(static (r, _) => (r.Location, r.Name)),
            static (spc, data) =>
                spc.ReportDiagnostic(Diagnostic.Create(s_missingValueProperty, data.Location, data.Name))
        );

        // ── Generate code only for valid types ────────────────────────────
        context.RegisterSourceOutput(
            classified.Where(static r => r.Kind == TargetKind.Valid).Select(static (r, _) => r.Model!.Value),
            static (spc, model) => Emit(spc, model)
        );
    }

    private static bool IsReadOnlyPartialRecordStruct(SyntaxNode node) =>
        node is RecordDeclarationSyntax rds
        && rds.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword)
        && rds.Modifiers.Any(SyntaxKind.ReadOnlyKeyword)
        && rds.Modifiers.Any(SyntaxKind.PartialKeyword);

    private static (TargetKind Kind, Location Location, string Name, StrongIdModel? Model) Classify(
        GeneratorAttributeSyntaxContext ctx,
        CancellationToken ct
    )
    {
        ct.ThrowIfCancellationRequested();

        Location location = ctx.TargetNode.GetLocation();
        string name = ctx.TargetSymbol.Name;

        // STRONGID001: must be a readonly partial record struct
        if (!IsReadOnlyPartialRecordStruct(ctx.TargetNode))
            return (TargetKind.NotRecordStruct, location, name, null);

        if (ctx.TargetSymbol is not INamedTypeSymbol symbol)
            return (TargetKind.NotRecordStruct, location, name, null);

        // STRONGID002: nested types not supported
        if (symbol.ContainingType is not null)
            return (TargetKind.NestedType, location, name, null);

        // STRONGID003: must have an int Value property
        bool hasIntValue = false;
        foreach (ISymbol member in symbol.GetMembers("Value"))
        {
            if (member is IPropertySymbol prop && prop.Type.SpecialType == SpecialType.System_Int32)
            {
                hasIntValue = true;
                break;
            }
        }
        if (!hasIntValue)
            return (TargetKind.MissingValue, location, name, null);

        // Valid — build the code-generation model.
        // AllowMultiple = false guarantees exactly one attribute instance here.
        bool generateConverter = true;
        foreach (KeyValuePair<string, TypedConstant> arg in ctx.Attributes[0].NamedArguments)
        {
            if (arg.Key == GenerateConverterArgName && arg.Value.Value is bool b)
            {
                generateConverter = b;
                break;
            }
        }

        string ns = symbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : symbol.ContainingNamespace.ToDisplayString();

        StrongIdModel model = new(ns, name, generateConverter);
        return (TargetKind.Valid, location, name, model);
    }

    private static void Emit(SourceProductionContext spc, StrongIdModel model)
    {
        spc.AddSource($"{model.Name}.g.cs", SourceText.From(BuildSource(model), Encoding.UTF8));
    }

    private static string BuildSource(StrongIdModel model)
    {
        string typeName = model.Name;
        bool generateConverter = model.GenerateJsonConverter;

        // Conditional fragments — plain strings, no braces to escape
        string jsonUsings = generateConverter
            ? "\nusing System.Text.Json;\nusing System.Text.Json.Serialization;"
            : string.Empty;

        string nsDecl = model.Namespace.Length > 0 ? $"\nnamespace {model.Namespace};\n" : string.Empty;

        string jsonAttr = generateConverter
            ? $"[JsonConverter(typeof({typeName}.StrongIdJsonConverter))]\n"
            : string.Empty;

        // $$""" = single { } are literal; {{ expr }} is interpolation
        string jsonConverter = generateConverter
            ? $$"""

                    public sealed class StrongIdJsonConverter : JsonConverter<{{typeName}}>
                    {
                        public override {{typeName}} Read(
                            ref Utf8JsonReader reader,
                            System.Type typeToConvert,
                            JsonSerializerOptions options
                        ) => new(reader.GetInt32());

                        public override void Write(
                            Utf8JsonWriter writer,
                            {{typeName}} value,
                            JsonSerializerOptions options
                        ) => writer.WriteNumberValue(value.Value);
                    }
                """
            : string.Empty;

        return $$"""
            // <auto-generated/>
            #nullable enable
            #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            using System.Diagnostics.CodeAnalysis;{{jsonUsings}}{{nsDecl}}
            [System.Diagnostics.DebuggerDisplay("{Value}")]
            {{jsonAttr}}public readonly partial record struct {{typeName}} :
                System.IComparable<{{typeName}}>,
                System.ISpanFormattable,
                System.IUtf8SpanFormattable,
                System.IParsable<{{typeName}}>,
                System.ISpanParsable<{{typeName}}>,
                System.Numerics.IComparisonOperators<{{typeName}}, {{typeName}}, bool>
            {
                public static implicit operator int({{typeName}} id) => id.Value;
                public static explicit operator {{typeName}}(int value) => new(value);
                public static bool operator <({{typeName}} left, {{typeName}} right) => left.Value < right.Value;
                public static bool operator >({{typeName}} left, {{typeName}} right) => left.Value > right.Value;
                public static bool operator <=({{typeName}} left, {{typeName}} right) => left.Value <= right.Value;
                public static bool operator >=({{typeName}} left, {{typeName}} right) => left.Value >= right.Value;
                public int CompareTo({{typeName}} other) => Value.CompareTo(other.Value);
                public override string ToString() => Value.ToString();

                public string ToString(string? format, System.IFormatProvider? provider) =>
                    Value.ToString(format, provider);

                public bool TryFormat(
                    System.Span<char> destination,
                    out int charsWritten,
                    System.ReadOnlySpan<char> format,
                    System.IFormatProvider? provider
                ) => Value.TryFormat(destination, out charsWritten, format, provider);

                public bool TryFormat(
                    System.Span<byte> utf8Destination,
                    out int bytesWritten,
                    System.ReadOnlySpan<char> format,
                    System.IFormatProvider? provider
                ) => Value.TryFormat(utf8Destination, out bytesWritten, format, provider);

                public static {{typeName}} Parse(string s, System.IFormatProvider? provider) =>
                    new(int.Parse(s, provider));

                public static bool TryParse(
                    [NotNullWhen(true)] string? s,
                    System.IFormatProvider? provider,
                    out {{typeName}} result
                )
                {
                    if (int.TryParse(s, provider, out int v))
                    {
                        result = new(v);
                        return true;
                    }
                    result = default;
                    return false;
                }

                public static {{typeName}} Parse(
                    System.ReadOnlySpan<char> s,
                    System.IFormatProvider? provider
                ) => new(int.Parse(s, provider));

                public static bool TryParse(
                    System.ReadOnlySpan<char> s,
                    System.IFormatProvider? provider,
                    out {{typeName}} result
                )
                {
                    if (int.TryParse(s, provider, out int v))
                    {
                        result = new(v);
                        return true;
                    }
                    result = default;
                    return false;
                }{{jsonConverter}}
            }
            #nullable restore
            """;
    }
}
