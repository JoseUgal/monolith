using Domain.Errors;

namespace Modules.Users.Domain.Users;

/// <summary>
/// Contains the users errors.
/// </summary>
public static class UserErrors
{
    /// <summary>
    /// Indicates that a user with the specified identifier was not found.
    /// </summary>
    /// <param name="userId">The unique identifier of the user that could not be found.</param>
    public static Error NotFound(UserId userId) => Error.NotFound(
        "User.NotFound",
        $"The user with the identifier '{userId.Value}' was not found."
    );

    /// <summary>
    /// Contains validation errors related to the user's first name.
    /// </summary>
    public static class FirstName
    {
        /// <summary>
        /// Indicates that the user's first name is required.
        /// </summary>
        public static Error IsRequired => Error.Failure(
            "User.FirstName.IsRequired",
            "The user first name cannot be null or empty."
        );
        
        /// <summary>
        /// Indicates that the user's first name exceeds the maximum allowed length.
        /// </summary>
        /// <param name="maxLength">The maximum allowed length for the first name.</param>
        public static Error TooLong(int maxLength) => Error.Failure(
            "User.FirstName.TooLong",
            $"The user first name cannot be longer than {maxLength} characters."
        );
    }
    
    /// <summary>
    /// Contains validation errors related to the user's last name.
    /// </summary>
    public static class LastName
    {
        /// <summary>
        /// Indicates that the user's last name is required.
        /// </summary>
        public static Error IsRequired => Error.Failure(
            "User.LastName.IsRequired",
            "The user last name cannot be null or empty."
        );
        
        /// <summary>
        /// Indicates that the user's last name exceeds the maximum allowed length.
        /// </summary>
        /// <param name="maxLength">The maximum allowed length for the last name.</param>
        public static Error TooLong(int maxLength) => Error.Failure(
            "User.LastName.TooLong",
            $"The user last name cannot be longer than {maxLength} characters."
        );
    }

    /// <summary>
    /// Contains validation and business rule errors related to the user's email.
    /// </summary>
    public static class Email
    {
        /// <summary>
        /// Indicates that the specified email is already in use by another user.
        /// </summary>
        public static Error IsNotUnique => Error.Conflict(
            "User.Email.IsNotUnique", 
            "The specified email is already in use."
        );
        
        /// <summary>
        /// Indicates that the user's email is required.
        /// </summary>
        public static Error IsRequired => Error.Failure(
            "User.Email.IsRequired",
            "The user email cannot be null or empty."
        );
        
        /// <summary>
        /// Indicates that the user's email format is invalid.
        /// </summary>
        public static Error InvalidFormat => Error.Failure(
            "User.Email.InvalidFormat",
            "The user email must be a valid format."
        );
        
        /// <summary>
        /// Indicates that the user's email exceeds the maximum allowed length.
        /// </summary>
        /// <param name="maxLength">The maximum allowed length for the email.</param>
        public static Error TooLong(int maxLength) => Error.Failure(
            "User.Email.TooLong",
            $"The user email cannot be longer than {maxLength} characters."
        );
    }

    /// <summary>
    /// Contains validation errors related to the user's plain password.
    /// </summary>
    public static class Password
    {
        /// <summary>
        /// Indicates that the user's password is required.
        /// </summary>
        public static Error IsRequired => Error.Failure(
            "User.Password.IsRequired",
            "The user password cannot be null or empty."
        );
        
        /// <summary>
        /// Indicates that the user's password is shorter than the minimum required length.
        /// </summary>
        /// <param name="minLength">
        /// The minimum number of characters required for the password.
        /// </param>
        public static Error TooShort(int minLength) => Error.Failure(
            "User.Password.TooShort",
            $"The password must be at least {minLength} characters long."
        );

        /// <summary>
        /// Indicates that the user's password exceeds the maximum allowed length.
        /// </summary>
        /// <param name="maxLength">The maximum allowed length for the password.</param>
        public static Error TooLong(int maxLength) => Error.Failure(
            "User.Password.TooLong",
            $"The password cannot be longer than {maxLength} characters."
        );

        /// <summary>
        /// Indicates that the user's password does not meet the required complexity rules.
        /// </summary>
        /// <remarks>
        /// A valid password must contain at least one uppercase letter,
        /// one lowercase letter, one digit, and one special character.
        /// </remarks>
        public static Error Weak => Error.Failure(
            "User.Password.Weak",
            "The password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character."
        );
    }
}
