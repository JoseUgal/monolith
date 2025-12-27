using Domain.Errors;
using Domain.Results;
using FluentAssertions;
using Modules.Tenants.Application.Tenants.GetMembers;
using Modules.Tenants.Domain.TenantMemberships;
using Modules.Tenants.Domain.Tenants;
using Modules.Tenants.Domain.Tests.Common.Mothers;
using Moq;

namespace Modules.Tenants.Application.Tests.Tenants.GetMembers;

public sealed class GetTenantMembersQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenTenantNotExists()
    {
        // Arrange
        var sut = new GetTenantMembersQueryHandlerSut();

        GetTenantMembersQuery query = sut.ValidQuery();

        sut.Repository.Setup(x =>
            x.ExistsAsync(It.IsAny<TenantId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(false);

        sut.Repository.Setup(x =>
            x.GetMembersAsync(It.IsAny<TenantId>(), It.IsAny<CancellationToken>())        
        ).ReturnsAsync([]);

        // Act
        Result<TenantMemberResponse[]> result = await sut.Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
        
        sut.Repository.Verify(x =>
            x.GetWithMembershipsAsync(It.IsAny<TenantId>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async Task Handle_ShouldMapMembers_ToResponses()
    {
        // Arrange
        var sut = new GetTenantMembersQueryHandlerSut();

        Tenant tenant = TenantMother.CreateWithActivatedMemberships(
            (Guid.NewGuid(), TenantRole.Member),
            (Guid.NewGuid(), TenantRole.Admin)
        );

        var query = new GetTenantMembersQuery(tenant.Id.Value);
        
        sut.Repository.Setup(x =>
            x.ExistsAsync(It.IsAny<TenantId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);

        sut.Repository.Setup(x =>
            x.GetMembersAsync(It.IsAny<TenantId>(), It.IsAny<CancellationToken>())        
        ).ReturnsAsync(tenant.Memberships.ToArray());
        
        // Act
        Result<TenantMemberResponse[]> result = await sut.Handler.Handle(query, CancellationToken.None); 
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Length.Should().Be(tenant.Memberships.Count);
        result.Value.Select(x => x.Role).Should().Contain(
            [nameof(TenantRole.Owner).ToLowerInvariant(), nameof(TenantRole.Member).ToLowerInvariant(), nameof(TenantRole.Admin).ToLowerInvariant()]
        );
        result.Value.Select(x => x.Status).Should().Contain(
            [nameof(TenantMembershipStatus.Active).ToLowerInvariant()]
        );
    }
}
