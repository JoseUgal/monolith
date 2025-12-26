using Domain.Errors;
using Domain.Results;
using FluentAssertions;
using Modules.Tenants.Application.Tenants.InviteMember;
using Modules.Tenants.Domain.TenantMemberships;
using Modules.Tenants.Domain.Tenants;
using Modules.Tenants.Domain.Tests.Common.Mothers;
using Moq;

namespace Modules.Tenants.Application.Tests.Tenants.InviteMember;

public sealed class InviteTenantMemberCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenTenantDoesNotExist()
    {
        // Arrange
        var sut = new InviteTenantMemberCommandHandlerSut();

        InviteTenantMemberCommand command = sut.ValidCommand() with
        {
            TenantId = Guid.NewGuid(),
        };

        sut.Repository.Setup(x =>
            x.GetWithMembershipsAsync(It.IsAny<TenantId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync((Tenant?)null);

        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
        
        sut.VerifyPersistedNever();
    }

    [Fact]
    public async Task Handle_ShouldReturnValidation_WhenRoleIsInvalid()
    {
        // Arrange
        var sut = new InviteTenantMemberCommandHandlerSut();

        InviteTenantMemberCommand command = sut.ValidCommand() with
        {
            Role = "invalid",
        };
        
        sut.Repository.Setup(x =>
            x.GetWithMembershipsAsync(It.IsAny<TenantId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(TenantMother.Create());

        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Failure);

        sut.VerifyPersistedNever();
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenUserIsAlreadyMemberOrInvited()
    {
        // Arrange
        var sut = new InviteTenantMemberCommandHandlerSut();

        var memberId = Guid.NewGuid();

        InviteTenantMemberCommand command = sut.ValidCommand() with
        {
            UserId = memberId
        };

        Tenant tenant = TenantMother.CreateWithActivatedMemberships(
            (memberId, TenantRole.Member)
        );

        sut.Repository.Setup(x =>
            x.GetWithMembershipsAsync(It.IsAny<TenantId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(tenant);

        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Conflict);

        sut.VerifyPersistedNever();
    }

    [Fact]
    public async Task Handle_ShouldInviteMember_AndSaveChanges_WhenRequestIsValid()
    {
        // Arrange
        var sut = new InviteTenantMemberCommandHandlerSut();

        Tenant tenant = TenantMother.Create();

        InviteTenantMemberCommand command = sut.ValidCommand() with
        {
            TenantId = tenant.Id.Value,
            UserId = Guid.NewGuid()
        };
        
        sut.Repository.Setup(x =>
            x.GetWithMembershipsAsync(It.Is<TenantId>(tenantId => tenantId == tenant.Id), It.IsAny<CancellationToken>())
        ).ReturnsAsync(tenant);

        sut.UnitOfWorkMock.Setup(x =>
            x.SaveChangesAsync(It.IsAny<CancellationToken>())
        ).Returns(Task.CompletedTask);

        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
        
        sut.VerifyPersistedOnce();
    }
}
