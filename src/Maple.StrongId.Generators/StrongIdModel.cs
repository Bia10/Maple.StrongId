namespace Maple.StrongId.Generators;

internal readonly record struct StrongIdModel(string Namespace, string Name, bool GenerateJsonConverter);
