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
    /// <summary>
    /// Initializes a new User with the specified identifier, name value objects, email, and password hash.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <param name="firstName">The user's first name value object.</param>
    /// <param name="lastName">The user's last name value object.</param>
    /// <param name="email">The user's email value object.</param>
    /// <param name="passwordHash">The hashed password for the user.</param>
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
    /// <summary>
    /// Creates a new User with a generated identifier and the provided name, email, and password hash.
    /// </summary>
    /// <param name="firstName">The user's first name value object.</param>
    /// <param name="lastName">The user's last name value object.</param>
    /// <param name="email">The user's email value object.</param>
    /// <param name="passwordHash">The hashed password for the user.</param>
    /// <returns>A new User instance with a newly generated UserId and the supplied data.</returns>
    public static User Create(UserFirstName firstName, UserLastName lastName, UserEmail email, string passwordHash)
    {
        var id = new UserId(Guid.NewGuid());
        
        return new User(id, firstName, lastName, email, passwordHash);
    }

    /// <summary>
    /// Changes the user's basic information.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <summary>
    /// Updates the user's first and last name.
    /// </summary>
    /// <param name="firstName">The new first name.</param>
    /// <param name="lastName">The new last name.</param>
    public void Change(UserFirstName firstName, UserLastName lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}