using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Workspaces.Domain.Workspaces;
using Modules.Workspaces.Persistence.Constants;
using Persistence.Constants;

namespace Modules.Workspaces.Persistence.Configurations;

/// <summary>
/// Represents the <see cref="Workspace"/> entity configuration.
/// </summary>
internal sealed class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        ConfigureDataStructure(builder);
        
        ConfigureRelationships(builder);
        
        ConfigureIndexes(builder);
    }

    private static void ConfigureDataStructure(EntityTypeBuilder<Workspace> builder)
    {
        builder.ToTable(TableNames.Workspaces);
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(x => x.Value, v => new WorkspaceId(v));

        builder.Property(x => x.TenantId)
            .ValueGeneratedNever()
            .IsRequired();
        
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(WorkspaceName.MaxLength)
            .HasConversion(x => x.Value, v => new WorkspaceName(v));
        
        builder.Property<DateTime>(AuditableProperties.CreatedOnUtc).IsRequired();
        
        builder.Property<DateTime?>(AuditableProperties.ModifiedOnUtc).IsRequired(false);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<Workspace> builder)
    {
        builder
            .HasMany(x => x.Memberships)
            .WithOne()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .Navigation(x => x.Memberships)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureIndexes(EntityTypeBuilder<Workspace> builder)
    {
        builder.HasIndex(x => x.TenantId);
    }
}
