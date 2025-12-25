namespace Modules.Tenants.Endpoints.Tenants;

internal static class TenantRoutes
{
    internal const string Tag = "Tenants";

    internal const string BaseUri = "tenants";

    internal const string ResourceId = "tenantId";
    
    internal const string MembershipId = "membershipId";

    internal const string Create = $"{BaseUri}";

    internal const string GetMine = $"{BaseUri}/me";

    internal const string GetMembers = $"{BaseUri}/{{{ResourceId}:guid}}/members";
    
    internal const string InviteMember = $"{BaseUri}/{{{ResourceId}:guid}}/members/invite";
    
    internal const string AcceptInvitation = $"{BaseUri}/{{{ResourceId}:guid}}/invitations/{{{MembershipId}:guid}}/accept";
}
