using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFinder.Domain.Entities;

namespace StayFinder.Infrastructure.Persistence.Configurations;

public class BlockedDateConfiguration : IEntityTypeConfiguration<BlockedDate>
{
    public void Configure(EntityTypeBuilder<BlockedDate> b)
    {
        b.ToTable("BlockedDates", t =>
            t.HasCheckConstraint("CK_BlockedDates_Range", "[ToDate] >= [FromDate]"));

        b.HasKey(x => x.Id);
        b.Property(x => x.Reason).HasMaxLength(200);
        b.HasIndex(x => new { x.ListingId, x.FromDate, x.ToDate });

        b.HasOne(x => x.Listing)
            .WithMany(l => l.BlockedDates)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
