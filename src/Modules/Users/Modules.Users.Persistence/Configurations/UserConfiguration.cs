using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Users;
using Modules.Users.Persistence.Constants;
using Persistence.Constants;
using Persistence.Extensions;

namespace Modules.Users.Persistence.Configurations;

/// <summary>
/// Represents the <see cref="User"/> entity configuration.
/// </summary>
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureDataStructure(builder);
        
        ConfigureIndexes(builder);
    }

    private static void ConfigureDataStructure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.Users);
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(x => x.Value, v => new UserId(v));

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(UserEmail.MaxLength)
            .HasConversion(x => x.Value, v => new UserEmail(v));

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(UserFirstName.MaxLength)
            .HasConversion(x => x.Value, v => new UserFirstName(v));

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(UserLastName.MaxLength)
            .HasConversion(x => x.Value, v => new UserLastName(v));

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(UserPassword.HashMaxLength);

        builder.Property<DateTime>(AuditableProperties.CreatedOnUtc).IsRequired();
        
        builder.Property<DateTime?>(AuditableProperties.ModifiedOnUtc).IsRequired(false);
    }
    
    private static void ConfigureIndexes(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => x.Email).IsUnique();
    }
}
