using Domain.Errors;

namespace Modules.Workspaces.Domain.WorkspaceMemberships;

/// <summary>
/// Contains the workspace membership errors.
/// </summary>
public static class WorkspaceMembershipErrors
{
    /// <summary>
    /// Indicates that a workspace membership with the specified identifier was not found.
    /// </summary>
    /// <param name="membershipId">The unique identifier of the workspace membership that could not be found.</param>
    public static Error NotFound(WorkspaceMembershipId membershipId) => Error.NotFound(
        "WorkspaceMembership.NotFound",
        $"The workspace membership with the identifier '{membershipId.Value}' was not found."
    );
    
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
    
    /// <summary>
    /// Contains the invitation errors.
    /// </summary>
    public static class Invitation
    {
        /// <summary>
        /// Indicates that the workspace membership invitation is already accepted.
        /// </summary>
        public static Error AlreadyAccepted => Error.Conflict(
            "WorkspaceMembership.Invitation.AlreadyAccepted",
            "The workspace membership invitation is already accepted."
        );
        
        /// <summary>
        /// Indicates that the workspace membership invitation is not valid.
        /// </summary>
        public static Error InvalidStatus => Error.Conflict(
            "WorkspaceMembership.Invitation.InvalidStatus",
            "The workspace membership invitation is not valid."
        );

        /// <summary>
        /// Indicates that the workspace membership invitation is not for the specified user.
        /// </summary>
        public static Error NotForUser => Error.Forbidden(
            "WorkspaceMembership.Invitation.NotForUser",
            "The workspace membership invitation is not for the specified user."
        );
    }
}
