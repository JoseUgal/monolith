namespace Modules.Workspaces.Application.Workspaces.GetMembers;

/// <summary>
/// Represents the workspace member response.
/// </summary>
public sealed class WorkspaceMemberResponse
{
    /// <summary>
    /// Gets the identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Gets the role.
    /// </summary>
    public string Role { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets the status.
    /// </summary>
    public string Status { get; init; } = string.Empty;
}
