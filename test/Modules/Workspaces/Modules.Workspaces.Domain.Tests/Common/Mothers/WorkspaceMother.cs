using Modules.Workspaces.Domain.WorkspaceMemberships;
using Modules.Workspaces.Domain.Workspaces;

namespace Modules.Workspaces.Domain.Tests.Common.Mothers;

public static class WorkspaceMother
{
    public static Workspace Create(
        Guid? tenantId = null,
        string? name = null,
        Guid? ownerId = null
    )
    {
        tenantId ??= Guid.NewGuid();
        ownerId ??= Guid.NewGuid();
        
        WorkspaceName workspaceName = WorkspaceName.Create(name ?? "My Workspace").Value;
        
        return Workspace.Create(tenantId.Value, workspaceName, ownerId.Value).Value;
    }
    
    public static Workspace CreateWithMemberships(
        params (Guid MemberId, WorkspaceMembershipRole role, WorkspaceMembershipStatus Status)[] memberships
    )
    {
        Workspace workspace = Create();

        foreach ((Guid MemberId, WorkspaceMembershipRole Role, WorkspaceMembershipStatus Status) membership in memberships)
        {
            WorkspaceMembership member = workspace.InviteMember(membership.MemberId, membership.Role).Value;

            if (membership.Status == WorkspaceMembershipStatus.Active)
            {
                workspace.AcceptInvitation(member.Id, member.UserId);
            }
        }

        return workspace;
    }

}
