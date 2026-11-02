using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFinder.Domain.Entities;

namespace StayFinder.Infrastructure.Persistence.Configurations;

public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> b)
    {
        b.ToTable("Amenities");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).IsRequired().HasMaxLength(50);
        b.Property(x => x.Icon).IsRequired().HasMaxLength(50);
        b.HasIndex(x => x.Name).IsUnique();

        // Seed data, inserted by the first migration
        b.HasData(
            new Amenity { Id = 1, Name = "Wi-Fi", Icon = "wifi" },
            new Amenity { Id = 2, Name = "Parking", Icon = "car" },
            new Amenity { Id = 3, Name = "Kitchen", Icon = "tools-kitchen-2" },
            new Amenity { Id = 4, Name = "Air conditioning", Icon = "temperature-snow" },
            new Amenity { Id = 5, Name = "Washer", Icon = "wash-machine" },
            new Amenity { Id = 6, Name = "TV", Icon = "device-tv" },
            new Amenity { Id = 7, Name = "Pool", Icon = "pool" },
            new Amenity { Id = 8, Name = "Workspace", Icon = "device-laptop" });
    }
}
