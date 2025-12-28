namespace Modules.Workspaces.Endpoints.Workspaces;

internal static class WorkspaceRoutes
{
    internal const string Tag = "Workspaces";

    internal const string BaseUri = "workspaces";

    internal const string ResourceId = "workspaceId";
    
    internal const string MembershipId = "membershipId";
    
    internal const string TenantId = "tenantId";
    
    internal const string Create = $"tenants/{{{TenantId}:guid}}/{BaseUri}";

    internal const string GetForTenant = $"tenants/{{{TenantId}:guid}}/{BaseUri}";
    
    internal const string GetById = $"{BaseUri}/{{{ResourceId}:guid}}";

    internal const string GetMembers = $"{BaseUri}/{{{ResourceId}:guid}}/members";
    
    internal const string InviteMember = $"{BaseUri}/{{{ResourceId}:guid}}/members/invite";
    
    internal const string AcceptInvitation = $"{BaseUri}/{{{ResourceId}:guid}}/invitations/{{{MembershipId}:guid}}/accept";
}
