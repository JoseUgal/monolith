using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Workspaces.Domain.WorkspaceMemberships;
using Modules.Workspaces.Domain.Workspaces;
using Modules.Workspaces.Persistence.Constants;
using Persistence.Constants;

namespace Modules.Workspaces.Persistence.Configurations;

/// <summary>
/// Represents the <see cref="WorkspaceMembership"/> entity configuration.
/// </summary>
internal sealed class WorkspaceMembershipConfiguration : IEntityTypeConfiguration<WorkspaceMembership>
{
    public void Configure(EntityTypeBuilder<WorkspaceMembership> builder)
    {
       ConfigureDataStructure(builder);
       
       ConfigureIndexes(builder);
    }

    private static void ConfigureDataStructure(EntityTypeBuilder<WorkspaceMembership> builder)
    {
        builder.ToTable(TableNames.WorkspaceMemberships);
        
        builder.HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(x => x.Value, v => new WorkspaceMembershipId(v));

        builder
            .Property(x => x.WorkspaceId)
            .IsRequired()
            .HasConversion(x => x.Value, v => new WorkspaceId(v));

        builder
            .Property(x => x.UserId)
            .IsRequired();

        builder
            .Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder
            .Property(x => x.Role)
            .HasConversion<string>()
            .IsRequired();
        
        builder.Property<DateTime>(AuditableProperties.CreatedOnUtc).IsRequired();
        
        builder.Property<DateTime?>(AuditableProperties.ModifiedOnUtc).IsRequired(false);
    }
    
    private static void ConfigureIndexes(EntityTypeBuilder<WorkspaceMembership> builder)
    {
        builder.HasIndex(x => new { x.WorkspaceId, x.UserId }).IsUnique();
    }
}
