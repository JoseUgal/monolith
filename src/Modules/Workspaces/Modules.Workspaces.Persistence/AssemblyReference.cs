using System.Reflection;

namespace Modules.Workspaces.Persistence;

/// <summary>
/// Represents the workspaces module persistence assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
