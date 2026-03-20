# Maple.StrongId.Generators

Roslyn incremental source generator that produces the boilerplate for `Maple.StrongId` strongly-typed int ID structs.

## What it generates

For any `readonly partial record struct` decorated with `[StrongIntId]`:

- `IComparable<T>`, `ISpanFormattable`, `IUtf8SpanFormattable`, `IParsable<T>`, `ISpanParsable<T>` implementations
- `implicit operator int` (unwrap) and `explicit operator T(int)` (wrap)
- `Parse` / `TryParse` delegating to `int`
- An optional nested `StrongIdJsonConverter : JsonConverter<T>` when `GenerateJsonConverter = true`

## Usage

```csharp
[StrongIntId]
public readonly partial record struct MobTemplateId(int Value);

[StrongIntId(GenerateJsonConverter = true)]
public readonly partial record struct ItemTemplateId(int Value);
```

This package is consumed automatically as a dependency of `Maple.StrongId`. It is not typically referenced directly.
