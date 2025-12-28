using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Tenants.Domain.TenantMemberships;
using Modules.Tenants.Domain.Tenants;
using Modules.Tenants.Persistence.Constants;
using Persistence.Constants;

namespace Modules.Tenants.Persistence.Configurations;

internal sealed class TenantMembershipConfiguration : IEntityTypeConfiguration<TenantMembership>
{
    public void Configure(EntityTypeBuilder<TenantMembership> builder)
    {
        ConfigureDataStructure(builder);
        
        ConfigureIndexes(builder);
    }

    private static void ConfigureDataStructure(EntityTypeBuilder<TenantMembership> builder)
    {
        builder.ToTable(TableNames.TenantMemberships);
        
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(x => x.Value, v => new TenantMembershipId(v));

        builder
            .Property(x => x.TenantId)
            .IsRequired()
            .HasConversion(x => x.Value, v => new TenantId(v));

        builder
            .Property(x => x.UserId)
            .IsRequired();

        builder
            .Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder
            .Property(x => x.MembershipRole)
            .HasConversion<string>()
            .IsRequired();
        
        builder.Property<DateTime>(AuditableProperties.CreatedOnUtc).IsRequired();
        
        builder.Property<DateTime?>(AuditableProperties.ModifiedOnUtc).IsRequired(false);
    }
    
    private static void ConfigureIndexes(EntityTypeBuilder<TenantMembership> builder)
    {
        builder.HasIndex(x => new { x.TenantId, x.UserId }).IsUnique();
    }
}
