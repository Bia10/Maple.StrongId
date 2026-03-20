namespace Maple.StrongId;

/// <summary>
/// Marks a <c>readonly partial record struct</c> as a strongly-typed wrapper for an <see langword="int"/> ID.
/// The <c>StrongIntIdGenerator</c> source generator will emit the full
/// interface implementation (<c>IComparable</c>, <c>ISpanFormattable</c>, <c>IParsable</c>, etc.)
/// and, when <see cref="GenerateJsonConverter"/> is <see langword="true"/>, a private nested
/// <c>StrongIdJsonConverter</c> class with the corresponding <c>[JsonConverter]</c> attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class StrongIntIdAttribute : Attribute
{
    /// <summary>
    /// Whether to generate a private nested <c>StrongIdJsonConverter</c> and apply
    /// <c>[JsonConverter(typeof(T.StrongIdJsonConverter))]</c> to the struct.
    /// Defaults to <see langword="true"/>. Set to <see langword="false"/> for IDs
    /// that are never serialized directly to/from JSON (e.g. <c>AccountId</c>, <c>CharacterId</c>).
    /// </summary>
    public bool GenerateJsonConverter { get; init; } = true;
}
