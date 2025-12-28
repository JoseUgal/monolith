using Domain.Errors;

namespace Modules.Workspaces.Domain.Workspaces;

/// <summary>
/// Contains the workspace errors.
/// </summary>
public static class WorkspaceErrors
{
    /// <summary>
    /// Indicates that a workspace with the specified identifier was not found.
    /// </summary>
    /// <param name="workspaceId">The unique identifier of the workspace that could not be found.</param>
    public static Error NotFound(WorkspaceId workspaceId) => Error.NotFound(
        "Workspace.NotFound",
        $"The workspace with the identifier '{workspaceId.Value}' was not found."
    );
    
    /// <summary>
    /// Indicates that the owner can only be added at workspace creation.
    /// </summary>
    public static Error OwnerAlreadyExist => Error.Conflict(
        "Workspace.OwnerAlreadyExist",
        "The owner can only be added at workspace creation."
    );
    
    /// <summary>
    /// Indicates that the user is already a member of this workspace.
    /// </summary>
    public static Error MemberAlreadyExist => Error.Conflict(
        "Workspace.MemberAlreadyExist",
        "The user is already a member of this workspace."
    );
    
    /// <summary>
    /// Contains validation errors related to the workspace's name.
    /// </summary>
    public static class Name
    {
        /// <summary>
        /// Indicates that the workspace's name is required.
        /// </summary>
        public static Error IsRequired => Error.Failure(
            "Workspace.Name.IsRequired",
            "The workspace name cannot be null or empty."
        );
        
        /// <summary>
        /// Indicates that the workspace's name exceeds the maximum allowed length.
        /// </summary>
        /// <param name="maxLength">The maximum allowed length for the name.</param>
        public static Error TooLong(int maxLength) => Error.Failure(
            "Workspace.Name.TooLong",
            $"The workspace name cannot be longer than {maxLength} characters."
        );
    }
}
