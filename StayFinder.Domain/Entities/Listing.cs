using StayFinder.Domain.Enums;

namespace StayFinder.Domain.Entities;

public class Listing
{
    public Guid Id { get; set; }
    public Guid HostId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public PropertyType Type { get; set; }

    public string City { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string Address { get; set; } = default!;

    public int MaxGuests { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }

    // ISO 4217 code (USD, UGX, SDG...). Prices and fees on this listing are in this currency.
    public string Currency { get; set; } = "USD";

    public decimal PricePerNight { get; set; }
    public decimal CleaningFee { get; set; }

    // Soft delete / admin moderation: listings with bookings are never physically deleted
    public bool IsActive { get; set; } = true;

    // Denormalized review stats, updated when a review is added
    public decimal AverageRating { get; set; }
    public int ReviewCount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = default!;

    public User Host { get; set; } = default!;
    public ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();
    public ICollection<ListingAmenity> ListingAmenities { get; set; } = new List<ListingAmenity>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<BlockedDate> BlockedDates { get; set; } = new List<BlockedDate>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}
