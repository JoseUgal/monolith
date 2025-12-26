using Domain.Errors;
using Domain.Results;
using FluentAssertions;
using Modules.Tenants.Application.Tenants.Create;
using Modules.Tenants.Domain.Tenants;
using Moq;

namespace Modules.Tenants.Application.Tests.Tenants.Create;

public sealed class CreateTenantCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnValidationFailure_WhenNameIsInvalid()
    {
        // Arrange
        var sut = new CreateTenantCommandHandlerSut();

        CreateTenantCommand command = sut.ValidCommand() with
        {
            Name = ""
        };
        
        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Failure);

        sut.VerifyPersistedNever();
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationFailure_WhenSlugIsInvalid()
    {
        // Arrange
        var sut = new CreateTenantCommandHandlerSut();

        CreateTenantCommand command = sut.ValidCommand() with
        {
            Slug = ""
        };
        
        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        // Asser
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Failure);
        
        sut.VerifyPersistedNever();
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenSlugIsNotUnique()
    {
        // Arrange
        var sut = new CreateTenantCommandHandlerSut();

        CreateTenantCommand command = sut.ValidCommand() with
        {
            Slug = "not-unique-slug"
        };

        sut.Repository.Setup(x =>
            x.IsSlugUniqueAsync(It.IsAny<TenantSlug>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(false);
        
        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        // Asser
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Conflict);
        
        sut.VerifyPersistedNever();
    }

    [Fact]
    public async Task Handle_ShouldCreateTenant_AndPersist_WhenRequestIsValid()
    {
        // Arrange
        var sut = new CreateTenantCommandHandlerSut();

        CreateTenantCommand command = sut.ValidCommand();
        
        sut.Repository.Setup(x =>
            x.IsSlugUniqueAsync(It.IsAny<TenantSlug>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);

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
