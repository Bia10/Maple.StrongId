; Unshipped analyzer releases
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|------
STRONGID001 | StrongId | Error | [StrongIntId] can only be applied to a readonly partial record struct
STRONGID002 | StrongId | Error | [StrongIntId] does not support nested types
STRONGID003 | StrongId | Error | [StrongIntId] requires an int Value property
