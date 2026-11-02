namespace StayFinder.Domain.Entities;

// One review per completed booking (unique BookingId)
public class Review
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public Guid ListingId { get; set; }
    public Guid GuestId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Booking Booking { get; set; } = default!;
    public Listing Listing { get; set; } = default!;
    public User Guest { get; set; } = default!;
}
