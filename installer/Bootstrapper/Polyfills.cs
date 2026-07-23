// .NET Framework 4.8's BCL predates C# 9's `init` accessors and records, both of
// which the compiler implements in terms of this marker type. The .NET SDK auto-
// injects it for some target/SDK combinations but not reliably for net48 WPF
// projects, so it's declared explicitly here.
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit;
}
