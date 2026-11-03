using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFinder.Domain.Entities;

namespace StayFinder.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> b)
    {
        b.ToTable("Bookings", t =>
        {
            t.HasCheckConstraint("CK_Bookings_Dates", "[CheckOut] > [CheckIn]");
            t.HasCheckConstraint("CK_Bookings_Guests", "[Guests] > 0");
            t.HasCheckConstraint("CK_Bookings_Currency", "LEN([Currency]) = 3");
        });

        b.HasKey(x => x.Id);

        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(x => x.Currency).IsRequired().HasMaxLength(3).IsFixedLength();
        b.Property(x => x.NightlyPrice).HasPrecision(18, 2);
        b.Property(x => x.CleaningFee).HasPrecision(18, 2);
        b.Property(x => x.ServiceFee).HasPrecision(18, 2);
        b.Property(x => x.TotalPrice).HasPrecision(18, 2);
        b.Property(x => x.RowVersion).IsRowVersion();

        // Computed helpers, not columns
        b.Ignore(x => x.Nights);
        b.Ignore(x => x.Stay);

        // Used by the overlap check and the availability search
        b.HasIndex(x => new { x.ListingId, x.CheckIn, x.CheckOut });

        // Used by the background job that expires pending requests
        b.HasIndex(x => new { x.Status, x.ExpiresAt });

        b.HasOne(x => x.Listing)
            .WithMany(l => l.Bookings)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Guest)
            .WithMany(u => u.Bookings)
            .HasForeignKey(x => x.GuestId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
