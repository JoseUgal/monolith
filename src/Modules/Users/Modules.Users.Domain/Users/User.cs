using Domain.Primitives;

namespace Modules.Users.Domain.Users;

public sealed class User : Entity<UserId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="email">The email.</param>
    /// <param name="passwordHash">The password hash.</param>
    private User(UserId id, UserFirstName firstName, UserLastName lastName, UserEmail email, string passwordHash) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
    }

    /// <summary>
    /// Gets the first name.
    /// </summary>
    public UserFirstName FirstName { get; private set; }
    
    /// <summary>
    /// Gets the last name.
    /// </summary>
    public UserLastName LastName { get; private set; }
    
    /// <summary>
    /// Gets the email.
    /// </summary>
    public UserEmail Email { get; private set; }
    
    /// <summary>
    /// Gets the password hash.
    /// </summary>
    public string PasswordHash { get; private set; }

    /// <summary>
    /// Creates a new <see cref="User"/> entity.
    /// </summary>
    /// <remarks>
    /// The method generates a new <see cref="UserId"/> for the entity.
    /// All value objects should be validated in this method.
    /// </remarks>
    public static User Create(UserFirstName firstName, UserLastName lastName, UserEmail email, string passwordHash)
    {
        var id = new UserId(Guid.NewGuid());
        
        return new User(id, firstName, lastName, email, passwordHash);
    }

    /// <summary>
    /// Changes the user's basic information.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    public void Change(UserFirstName firstName, UserLastName lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
