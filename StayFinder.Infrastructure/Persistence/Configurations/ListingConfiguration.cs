using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFinder.Domain.Entities;

namespace StayFinder.Infrastructure.Persistence.Configurations;

public class ListingConfiguration : IEntityTypeConfiguration<Listing>
{
    public void Configure(EntityTypeBuilder<Listing> b)
    {
        b.ToTable("Listings", t =>
        {
            t.HasCheckConstraint("CK_Listings_Price", "[PricePerNight] >= 0 AND [CleaningFee] >= 0");
            t.HasCheckConstraint("CK_Listings_MaxGuests", "[MaxGuests] > 0");
            t.HasCheckConstraint("CK_Listings_Currency", "LEN([Currency]) = 3");
        });

        b.HasKey(x => x.Id);

        b.Property(x => x.Title).IsRequired().HasMaxLength(150);
        b.Property(x => x.Description).IsRequired();
        b.Property(x => x.Type).HasConversion<string>().HasMaxLength(20);
        b.Property(x => x.City).IsRequired().HasMaxLength(100);
        b.Property(x => x.Country).IsRequired().HasMaxLength(100);
        b.Property(x => x.Address).IsRequired().HasMaxLength(300);

        b.Property(x => x.Currency).IsRequired().HasMaxLength(3).IsFixedLength();
        b.Property(x => x.PricePerNight).HasPrecision(18, 2);
        b.Property(x => x.CleaningFee).HasPrecision(18, 2);
        b.Property(x => x.AverageRating).HasPrecision(3, 2);
        b.Property(x => x.RowVersion).IsRowVersion();

        b.HasIndex(x => new { x.City, x.IsActive });
        b.HasIndex(x => x.PricePerNight);

        b.HasOne(x => x.Host)
            .WithMany(u => u.Listings)
            .HasForeignKey(x => x.HostId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
