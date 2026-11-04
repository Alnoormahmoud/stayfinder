using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFinder.Domain.Entities;

namespace StayFinder.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> b)
    {
        b.ToTable("AuditLogs");
        b.HasKey(x => x.Id);

        b.Property(x => x.Action).IsRequired().HasMaxLength(100);
        b.Property(x => x.EntityType).HasMaxLength(100);
        b.Property(x => x.Details).HasMaxLength(1000);
        b.Property(x => x.IpAddress).HasMaxLength(45);

        b.HasIndex(x => x.CreatedAt);
        b.HasIndex(x => new { x.UserId, x.CreatedAt });
    }
}
