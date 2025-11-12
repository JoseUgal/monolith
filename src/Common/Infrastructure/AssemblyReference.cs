using System.Reflection;

namespace Infrastructure;

/// <summary>
/// Represents the infrastructure assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
