using Domain.Results;
using FluentAssertions;
using Modules.Tenants.Domain.TenantMemberships;
using Modules.Tenants.Domain.Tenants;
using Modules.Tenants.Domain.Tests.Common.Mothers;

namespace Modules.Tenants.Domain.Tests.Tenants;

public class TenantTests
{
    [Fact]
    public void InviteMember_Should_ReturnConflict_When_RoleIsOwner()
    {
        // Arrange
        Tenant tenant = TenantMother.Create();
        
        var userId = Guid.NewGuid();

        // Act
        Result<TenantMembership> result = tenant.InviteMember(userId, TenantRole.Owner);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TenantErrors.OwnerAlreadyExist);
    }

    
    [Fact]
    public void InviteMember_Should_CreateInvitedMembership_When_UserIsNotMember()
    {
        // Arrange
        Tenant tenant = TenantMother.Create();

        var memberId = Guid.NewGuid();
        
        // Act
        Result<TenantMembership> result = tenant.InviteMember(memberId, TenantRole.Member);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Status.Should().Be(TenantMembershipStatus.Invited);
        result.Value.Role.Should().Be(TenantRole.Member);
        tenant.Memberships.Should().HaveCount(2);
    }
    
    [Fact]
    public void InviteMember_Should_ReturnConflict_When_UserAlreadyMember()
    {
        // Arrange
        Tenant tenant = TenantMother.Create();

        TenantMembership member = tenant.Memberships.First();
        
        // Act
        Result<TenantMembership> result = tenant.InviteMember(member.UserId, TenantRole.Member);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TenantErrors.MemberAlreadyExist);
    }
    
    [Fact]
    public void InviteMember_Should_SetCorrectRole()
    {
        // Arrange
        Tenant tenant = TenantMother.Create();

        var memberId = Guid.NewGuid();
        
        // Act
        Result<TenantMembership> result = tenant.InviteMember(memberId, TenantRole.Admin);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Role.Should().Be(TenantRole.Admin);
    }
    
    [Fact]
    public void InviteMember_Should_SetStatusAsInvited()
    {
        // Arrange
        Tenant tenant = TenantMother.Create();

        var memberId = Guid.NewGuid();
        
        // Act
        Result<TenantMembership> result = tenant.InviteMember(memberId, TenantRole.Member);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Status.Should().Be(TenantMembershipStatus.Invited);
    }
    
    [Fact]
    public void AcceptInvitation_Should_ActivateMembership_When_StatusIsInvited()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        
        Tenant tenant = TenantMother.CreateWithInvitedMemberships(
            new ValueTuple<Guid, TenantRole>(memberId, TenantRole.Member)
        );
        
        TenantMembership invited = tenant.Memberships.Single(x => x.UserId == memberId);

        // Act
        Result result = tenant.AcceptInvitation(invited.Id, memberId);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        invited.Status.Should().Be(TenantMembershipStatus.Active);
    }
    
    [Fact]
    public void AcceptInvitation_Should_ReturnNotFound_When_MembershipDoesNotExist()
    {
        // Arrange
        Tenant tenant = TenantMother.Create();
        
        var membershipId = new TenantMembershipId(Guid.NewGuid());
        
        var userId = Guid.NewGuid();
        
        // Act
        Result result = tenant.AcceptInvitation(membershipId, userId);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TenantMembershipErrors.NotFound(membershipId));
    }
    
    [Fact]
    public void AcceptInvitation_Should_ReturnConflict_When_MembershipIsAlreadyActive()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        
        Tenant tenant = TenantMother.CreateWithActivatedMemberships(
            new ValueTuple<Guid, TenantRole>(memberId, TenantRole.Member)
        );

        // Act
        Result result = tenant.AcceptInvitation(tenant.Memberships.Last().Id, memberId);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TenantMembershipErrors.Invitation.AlreadyAccepted);
    }
    
    [Fact]
    public void AcceptInvitation_Should_ReturnForbidden_When_InvitationDoesNotBelongToUser()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        
        Tenant tenant = TenantMother.CreateWithInvitedMemberships(
            new ValueTuple<Guid, TenantRole>(memberId, TenantRole.Member)
        );

        // Act
        Result result = tenant.AcceptInvitation(tenant.Memberships.Last().Id, Guid.NewGuid());
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TenantMembershipErrors.Invitation.NotForUser);
    }

    [Fact]
    public void Tenant_Should_Not_Allow_DuplicateMemberships_ForSameUser()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        
        Tenant tenant = TenantMother.CreateWithInvitedMemberships(
            new ValueTuple<Guid, TenantRole>(memberId, TenantRole.Member)
        );

        // Act
        Result result = tenant.InviteMember(memberId, TenantRole.Member);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TenantErrors.MemberAlreadyExist);
    }

    [Fact]
    public void Tenant_Memberships_Should_Not_Be_Modifiable_Via_ExposedCollectionReference()
    {
        // Arrange
        Tenant tenant = TenantMother.Create();

        int initialMemberships = tenant.Memberships.Count;

        // Act
        var memberships = tenant.Memberships.ToList();

        memberships.Add(TenantMembership.Invite(tenant.Id, Guid.NewGuid(), TenantRole.Member));

        // Assert
        tenant.Memberships.Should().HaveCount(initialMemberships);
        memberships.Should().HaveCount(initialMemberships + 1);
    }
    
    
}
