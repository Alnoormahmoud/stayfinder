using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFinder.Domain.Entities;

namespace StayFinder.Infrastructure.Persistence.Configurations;

public class ListingImageConfiguration : IEntityTypeConfiguration<ListingImage>
{
    public void Configure(EntityTypeBuilder<ListingImage> b)
    {
        b.ToTable("ListingImages");
        b.HasKey(x => x.Id);
        b.Property(x => x.Url).IsRequired().HasMaxLength(500);

        b.HasOne(x => x.Listing)
            .WithMany(l => l.Images)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
