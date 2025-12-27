using Domain.Results;
using FluentAssertions;
using Modules.Tenants.Application.Tenants.GetForUser;
using Moq;

namespace Modules.Tenants.Application.Tests.Tenants.GetForUser;

public sealed class GetTenantsForUserQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnEmptyArray_WhenUserHasNoTenants()
    {
        // Arrange
        var sut = new GetTenantsForUserQueryHandlerSut();
        var query = new GetTenantsForUserQuery(Guid.NewGuid());

        sut.Sql.Setup(x =>
            x.QueryAsync<TenantResponse>(It.IsAny<string>(), It.IsAny<object?>())
        ).ReturnsAsync([]);

        // Act
        Result<TenantResponse[]> result = await sut.Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnTenants_WhenUserHasTenants()
    {
        // Arrange
        var sut = new GetTenantsForUserQueryHandlerSut();
        
        var query = new GetTenantsForUserQuery(Guid.NewGuid());
        
        TenantResponse[] responses =
        [
            new() { Id = Guid.NewGuid(), Name = "Acme", Slug = "acme" },
            new() { Id = Guid.NewGuid(), Name = "Contoso", Slug = "contoso" }
        ];

        sut.Sql.Setup(x =>
            x.QueryAsync<TenantResponse>(It.IsAny<string>(), It.IsAny<object?>())
        ).ReturnsAsync(responses);

        // Act
        Result<TenantResponse[]> result = await sut.Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Length.Should().Be(responses.Length);
        result.Value.Select(x => x.Name).Should().Contain(["Acme", "Contoso"]);
    }
}
