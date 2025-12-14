using Modules.Users.Domain.Users;

namespace Modules.Users.UnitTests.Common.Mothers;

internal static class UserMother
{
    public static User Create(
        string? firstName = null,
        string? lastName = null,
        string? email = null,
        string? passwordHash = null
    )
    {
        passwordHash ??= "8a24367a1f46c141048752f2d5bbd14b";
        
        UserFirstName userFirstName = UserFirstName.Create(firstName ?? "Ana").Value;
        UserLastName userLastName = UserLastName.Create(lastName ?? "Garcia").Value;
        UserEmail userEmail = UserEmail.Create(email ?? "ana@demo.es").Value;
        
        return User.Create(userFirstName, userLastName, userEmail, passwordHash);
    }
}
