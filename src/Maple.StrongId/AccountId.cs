namespace Maple.StrongId;

/// <summary>
/// Strongly-typed wrapper for an account ID.
/// No arithmetic or range decomposition is applied to account IDs in the client —
/// they are treated as opaque server-assigned keys.
/// </summary>
[StrongIntId(GenerateJsonConverter = false)]
public readonly partial record struct AccountId(int Value);
