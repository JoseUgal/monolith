using Modules.Users.Application.Users.Update;
using Modules.Users.Domain;
using Modules.Users.Domain.Users;
using Modules.Users.UnitTests.Common.Mothers;
using Moq;

namespace Modules.Users.UnitTests.Application.Users.Update;

internal sealed class UpdateUserCommandHandlerSut
{
    public Mock<IUserRepository> UserRepositoryMock { get; } = new(MockBehavior.Loose);
    
    public Mock<IUnitOfWork> UnitOfWorkMock { get; } = new(MockBehavior.Strict);

    public UpdateUserCommandHandler Handler { get; }

    public UpdateUserCommandHandlerSut()
    {
        Handler = new UpdateUserCommandHandler(
            UserRepositoryMock.Object,
            UnitOfWorkMock.Object
        );
    }

    public UpdateUserCommand ValidCommand(Guid userId) => new(
        userId,
        "Juan",
        "Pérez"
    );

    public void SetupUserNotFound()
    {
        UserRepositoryMock.Setup(x => 
            x.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync((User?)null);
    }

    public void SetupUserExists(User? user = null)
    {
        user ??= UserMother.Create();
        
        UserRepositoryMock.Setup(x =>
            x.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(user); 
    }

    public void VerifyDidNotSave()
    {
        UnitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    public void VerifySavedOnce()
    {
        UnitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    public void VerifyGetByIdCalled(Guid userId)
    {
        UserRepositoryMock.Verify(
            x => x.GetByIdAsync(It.Is<UserId>(id => id.Value == userId), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
