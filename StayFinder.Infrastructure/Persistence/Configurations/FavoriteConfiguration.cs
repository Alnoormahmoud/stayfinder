using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFinder.Domain.Entities;

namespace StayFinder.Infrastructure.Persistence.Configurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> b)
    {
        b.ToTable("Favorites");
        b.HasKey(x => new { x.UserId, x.ListingId });

        b.HasOne(x => x.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Listing)
            .WithMany(l => l.Favorites)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
