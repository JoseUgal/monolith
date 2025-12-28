using System.Reflection;

namespace Modules.Workspaces.Infrastructure;

/// <summary>
/// Represents the workspaces module infrastructure assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}

