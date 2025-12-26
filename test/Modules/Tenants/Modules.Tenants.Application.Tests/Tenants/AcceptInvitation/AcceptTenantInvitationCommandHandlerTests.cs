using Domain.Errors;
using Domain.Results;
using FluentAssertions;
using Modules.Tenants.Application.Tenants.AcceptInvitation;
using Modules.Tenants.Domain.TenantMemberships;
using Modules.Tenants.Domain.Tenants;
using Modules.Tenants.Domain.Tests.Common.Mothers;

namespace Modules.Tenants.Application.Tests.Tenants.AcceptInvitation;

public sealed class AcceptTenantInvitationCommandHandlerTests
{

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenTenantDoesNotExist()
    {
        // Arrange
        var sut = new AcceptTenantInvitationCommandHandlerSut();

        AcceptTenantInvitationCommand command = sut.ValidCommand() with
        {
            TenantId = Guid.NewGuid()
        };
        
        sut.SetupGetWithMembershipsReturnsNullable();

        // Act
        Result result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
        
        sut.VerifyPersistedNever();
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenMembershipDoesNotExist()
    {
        // Arrange
        var sut = new AcceptTenantInvitationCommandHandlerSut();

        Tenant tenant = TenantMother.Create();

        AcceptTenantInvitationCommand command = sut.ValidCommand() with
        {
            TenantId = tenant.Id.Value,
            MembershipId = Guid.NewGuid()
        };
        
        sut.SetupGetWithMembershipsReturnsTenant(tenant);

        // Act
        Result result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
        
        sut.VerifyPersistedNever();
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenInvitationAlreadyAccepted()
    {
        // Arrange
        var sut = new AcceptTenantInvitationCommandHandlerSut();

        var userId = Guid.NewGuid();

        Tenant tenant = TenantMother.CreateWithActivatedMemberships(
            (userId, TenantRole.Member)
        );
        
        TenantMembership membership = tenant.Memberships.Single(x => x.UserId == userId);

        var command = new AcceptTenantInvitationCommand(
            TenantId: tenant.Id.Value, 
            MembershipId: membership.Id.Value, 
            UserId: membership.UserId
        );
        
        sut.SetupGetWithMembershipsReturnsTenant(tenant);

        // Act
        Result result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Conflict);
        
        sut.VerifyPersistedNever();
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenInvitationIsNotForCurrentUser()
    {
        // Arrange
        var sut = new AcceptTenantInvitationCommandHandlerSut();

        var userId = Guid.NewGuid();

        Tenant tenant = TenantMother.CreateWithActivatedMemberships(
            (userId, TenantRole.Member)
        );
        
        TenantMembership membership = tenant.Memberships.Single(x => x.UserId == userId);

        var command = new AcceptTenantInvitationCommand(
            TenantId: tenant.Id.Value, 
            MembershipId: membership.Id.Value, 
            UserId: Guid.NewGuid()
        );
        
        sut.SetupGetWithMembershipsReturnsTenant(tenant);

        // Act
        Result result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Forbidden);
        
        sut.VerifyPersistedNever();
    }

    [Fact]
    public async Task Handle_ShouldAcceptInvitation_AndSaveChanges_WhenRequestIsValid()
    {
        // Arrange
        var sut =  new AcceptTenantInvitationCommandHandlerSut(); 
        
        var invitedUserId = Guid.NewGuid();

        Tenant tenant = TenantMother.CreateWithInvitedMemberships(
            (invitedUserId, TenantRole.Member)
        );
        
        TenantMembership membership = tenant.Memberships.Single(x => x.UserId == invitedUserId);

        var command = new AcceptTenantInvitationCommand(
            TenantId: tenant.Id.Value,
            MembershipId: membership.Id.Value,
            UserId: membership.UserId
        );
        
        sut.SetupGetWithMembershipsReturnsTenant(tenant);
        
        sut.SetupUnitOfWork();

        // Act
        Result result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        membership.Status.Should().Be(TenantMembershipStatus.Active);
        
        sut.VerifyPersistedOnce();
    }
}
