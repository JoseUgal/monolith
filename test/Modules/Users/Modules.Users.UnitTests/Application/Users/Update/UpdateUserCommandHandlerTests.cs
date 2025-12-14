using Domain.Results;
using Modules.Users.Application.Users.Update;
using Modules.Users.Domain.Users;
using Modules.Users.UnitTests.Common.Mothers;
using Moq;

namespace Modules.Users.UnitTests.Application.Users.Update;

public sealed class UpdateUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_UserNotFound_ReturnsNotFound_AndDoesNotSave()
    {
        // Arrange
        var sut = new UpdateUserCommandHandlerSut();
        
        var userId = Guid.NewGuid();
        
        UpdateUserCommand command = sut.ValidCommand(userId);

        sut.SetupUserNotFound();

        // Act
        Result result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.NotFound(new UserId(userId)), result.Error);

        sut.VerifyGetByIdCalled(userId);
        sut.VerifyDidNotSave();
    }
    
    [Fact]
    public async Task Handle_FirstNameInvalid_ReturnsFailure_AndDoesNotPersist()
    {
        // Arrange
        UpdateUserCommandHandlerSut sut = new();

        var userId = Guid.NewGuid();

        UpdateUserCommand command = sut.ValidCommand(userId) with { FirstName = "" };

        sut.SetupUserExists();
        
        // Act
        Result result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.FirstName.IsRequired, result.Error);
        
        sut.VerifyDidNotSave();
    }
    
    [Fact]
    public async Task Handle_LastNameInvalid_ReturnsFailure_AndDoesNotPersist()
    {
        // Arrange
        UpdateUserCommandHandlerSut sut = new();

        var userId = Guid.NewGuid();

        UpdateUserCommand command = sut.ValidCommand(userId) with { LastName = "" };

        sut.SetupUserExists();
        
        // Act
        Result result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.LastName.IsRequired, result.Error);
        
        sut.VerifyDidNotSave();
    }
    
    [Fact]
    public async Task Handle_ValidCommand_SavesChanges_AndReturnsSuccess()
    {
        // Arrange
        UpdateUserCommandHandlerSut sut = new();
        
        var userId = Guid.NewGuid();

        UpdateUserCommand command = sut.ValidCommand(userId) with { FirstName = "Juan", LastName = "Perez" };

        User user = UserMother.Create();

        sut.SetupUserExists(user);

        sut.UnitOfWorkMock.Setup(x => 
            x.SaveChangesAsync(It.IsAny<CancellationToken>())
        ).Returns(Task.CompletedTask);

        // Act
        Result result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        
        sut.VerifySavedOnce();
        
        Assert.Equal(command.FirstName, user.FirstName.Value);
        Assert.Equal(command.LastName, user.LastName.Value);
    }
}
