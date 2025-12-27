using Domain.Errors;

namespace Modules.Workspaces.Domain.Workspaces;

/// <summary>
/// Contains the workspace errors.
/// </summary>
public static class WorkspaceErrors
{
    /// <summary>
    /// Indicates that the owner can only be added at workspace creation.
    /// </summary>
    public static Error OwnerAlreadyExist => Error.Conflict(
        "Workspace.OwnerAlreadyExist",
        "The owner can only be added at workspace creation."
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
