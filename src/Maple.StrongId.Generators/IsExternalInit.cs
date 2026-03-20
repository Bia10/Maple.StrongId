// Required polyfill: 'init' setters (used by record types) rely on IsExternalInit,
// which is not defined in netstandard2.0. The compiler emits it as a reference;
// declaring it here as an internal no-op class satisfies the requirement.
namespace System.Runtime.CompilerServices;

internal static class IsExternalInit { }
