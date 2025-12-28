using System.Reflection;

namespace Modules.Workspaces.Endpoints;

/// <summary>
/// Represents the workspaces module endpoints assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
