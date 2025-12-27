using Domain.Errors;
using Domain.Results;
using FluentAssertions;
using Modules.Tenants.Application.Tenants.GetById;
using Moq;

namespace Modules.Tenants.Application.Tests.Tenants.GetById;

public sealed class GetTenantByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenTenantDoesNotExist()
    {
        // Arrange
        var sut = new GetTenantByIdQueryHandlerSut();
        
        var query = new GetTenantByIdQuery(Guid.NewGuid());

        sut.Sql.Setup(x =>
            x.FirstOrDefaultAsync<TenantResponse>(It.IsAny<string>(), It.IsAny<object?>())
        ).ReturnsAsync((TenantResponse?)null);

        // Act
        Result<TenantResponse> result = await sut.Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnTenant_WhenTenantExists()
    {
        
         // Arrange
         var sut = new GetTenantByIdQueryHandlerSut();
         
         var tenantId = Guid.NewGuid();
           
         var query = new GetTenantByIdQuery(tenantId);

         var response = new TenantResponse
         {
             Id = tenantId,
             Name = "Acme",
             Slug = "acme"
         };

         sut.Sql.Setup(x =>
             x.FirstOrDefaultAsync<TenantResponse>(It.IsAny<string>(), It.IsAny<object?>())
         ).ReturnsAsync(response);

         // Act
         Result<TenantResponse> result = await sut.Handler.Handle(query, CancellationToken.None);

         // Assert
         result.IsSuccess.Should().BeTrue();
         result.Value.Id.Should().Be(tenantId);
         result.Value.Name.Should().Be(response.Name);
         result.Value.Slug.Should().Be(response.Slug);
    }
}
