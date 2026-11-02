using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFinder.Domain.Entities;

namespace StayFinder.Infrastructure.Persistence.Configurations;

public class ListingAmenityConfiguration : IEntityTypeConfiguration<ListingAmenity>
{
    public void Configure(EntityTypeBuilder<ListingAmenity> b)
    {
        b.ToTable("ListingAmenities");
        b.HasKey(x => new { x.ListingId, x.AmenityId });
        b.HasIndex(x => x.AmenityId);

        b.HasOne(x => x.Listing)
            .WithMany(l => l.ListingAmenities)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Amenity)
            .WithMany(a => a.ListingAmenities)
            .HasForeignKey(x => x.AmenityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
