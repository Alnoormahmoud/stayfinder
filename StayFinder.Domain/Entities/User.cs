using StayFinder.Domain.Enums;

namespace StayFinder.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public UserRole Role { get; set; } = UserRole.Guest;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}
