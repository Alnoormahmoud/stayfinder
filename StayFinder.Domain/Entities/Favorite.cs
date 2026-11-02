namespace StayFinder.Domain.Entities;

public class Favorite
{
    public Guid UserId { get; set; }
    public Guid ListingId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = default!;
    public Listing Listing { get; set; } = default!;
}
