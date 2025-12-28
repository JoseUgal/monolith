using Domain.Errors;

namespace Modules.Workspaces.Domain.WorkspaceMemberships;

/// <summary>
/// Contains the workspace membership errors.
/// </summary>
public static class WorkspaceMembershipErrors
{
    /// <summary>
    /// Contains the role errors.
    /// </summary>
    public static class Role
    {
        /// <summary>
        /// Indicates that the workspace membership role is invalid.
        /// </summary>
        public static Error IsInvalid => Error.Failure(
            "WorkspaceMembership.Role.IsInvalid",
            "The workspace membership role is not defined."
        );
    }
}
