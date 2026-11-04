using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using StayFinder.Domain.Entities;

namespace StayFinder.Application.Common.Interfaces;

/// <summary>
/// What the Application layer is allowed to know about the database.
/// AppDbContext (Infrastructure) implements it.
/// </summary>
public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Listing> Listings { get; }
    DbSet<ListingImage> ListingImages { get; }
    DbSet<Amenity> Amenities { get; }
    DbSet<ListingAmenity> ListingAmenities { get; }
    DbSet<Booking> Bookings { get; }
    DbSet<BlockedDate> BlockedDates { get; }
    DbSet<Review> Reviews { get; }
    DbSet<Favorite> Favorites { get; }
    DbSet<AuditLog> AuditLogs { get; }

    // Needed later for transactions (for example the serializable booking transaction)
    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
