using System.Reflection;

namespace Modules.Workspaces.Application;

/// <summary>
/// Represents the workspaces module application assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
