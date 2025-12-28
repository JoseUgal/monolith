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
    
    public static Workspace CreateWithInvitedMemberships(
        params (Guid MemberId, WorkspaceMembershipRole role)[] memberships
    )
    {
        Workspace workspace = Create();

        foreach ((Guid MemberId, WorkspaceMembershipRole Role) membership in memberships)
        {
            workspace.InviteMember(membership.MemberId, membership.Role);
        }

        return workspace;
    }

}
