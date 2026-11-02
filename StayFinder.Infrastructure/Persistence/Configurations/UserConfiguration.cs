using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StayFinder.Domain.Entities;

namespace StayFinder.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users");
        b.HasKey(x => x.Id);

        b.Property(x => x.FullName).IsRequired().HasMaxLength(100);
        b.Property(x => x.Email).IsRequired().HasMaxLength(256);
        b.Property(x => x.PasswordHash).IsRequired().HasMaxLength(512);
        b.Property(x => x.PhoneNumber).HasMaxLength(30);
        b.Property(x => x.Role).HasConversion<string>().HasMaxLength(20);

        b.HasIndex(x => x.Email).IsUnique();
    }
}
