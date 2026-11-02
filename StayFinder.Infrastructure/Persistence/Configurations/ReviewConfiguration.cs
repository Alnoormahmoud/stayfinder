using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFinder.Domain.Entities;

namespace StayFinder.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> b)
    {
        b.ToTable("Reviews", t =>
            t.HasCheckConstraint("CK_Reviews_Rating", "[Rating] BETWEEN 1 AND 5"));

        b.HasKey(x => x.Id);
        b.Property(x => x.Comment).IsRequired().HasMaxLength(1000);
        b.HasIndex(x => x.ListingId);

        // One-to-one: EF creates a unique index on BookingId, so a booking gets at most one review.
        // All relationships here are Restrict to avoid SQL Server "multiple cascade paths" errors.
        b.HasOne(x => x.Booking)
            .WithOne(bk => bk.Review)
            .HasForeignKey<Review>(x => x.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Listing)
            .WithMany(l => l.Reviews)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Guest)
            .WithMany(u => u.Reviews)
            .HasForeignKey(x => x.GuestId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
